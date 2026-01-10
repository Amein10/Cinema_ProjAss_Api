using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cinema_ProjAss_Application.DTOs.Bookings;
using Cinema_ProjAss_Application.Exceptions;
using Cinema_ProjAss_Application.Services;
using Cinema_ProjAss_Domain.Entities;
using Cinema_ProjAss_Domain.Interfaces;
using Moq;
using Xunit;

namespace Cinema_ProjAss_Test
{
    public class BookingServiceTests
    {
        private static BookingService CreateSut(
            Mock<IBookingRepository> bookingsMock,
            Mock<IShowRepository> showsMock)
        {
            return new BookingService(bookingsMock.Object, showsMock.Object);
        }

        private static Show MakeShow(int id = 10, decimal price = 120m)
        {
            return new Show
            {
                Id = id,
                Price = price,
                StartTime = DateTime.UtcNow.AddDays(1),
                Movie = new Movie { Title = "Test Movie" },
                Auditorium = new Auditorium { Name = "A1" }
            };
        }

        private static Booking MakeBooking(int id = 1, string userId = "user-1", int showId = 10, BookingStatus status = BookingStatus.Pending)
        {
            return new Booking
            {
                Id = id,
                UserId = userId,
                ShowId = showId,
                Status = status,
                CreatedAt = DateTime.UtcNow,
                Show = MakeShow(showId, 120m),
                BookingSeats = new List<BookingSeat>
                {
                    new BookingSeat { SeatId = 1, PriceAtBooking = 120m, Seat = new Seat { Row = "A", Number = 1 } }
                }
            };
        }

        // 1) GetByIdAsync - found => returns dto
        [Fact]
        public async Task GetByIdAsync_WhenBookingExists_ReturnsDto()
        {
            var bookings = new Mock<IBookingRepository>();
            var shows = new Mock<IShowRepository>();
            var sut = CreateSut(bookings, shows);

            var booking = MakeBooking(id: 5);
            bookings.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(booking);

            var dto = await sut.GetByIdAsync(5);

            Assert.Equal(5, dto.Id);
            Assert.Equal(booking.UserId, dto.UserId);
            Assert.Equal(booking.ShowId, dto.ShowId);
            Assert.Equal(booking.Status.ToString(), dto.Status);
        }

        // 2) GetByIdAsync - not found => throws NotFoundException
        [Fact]
        public async Task GetByIdAsync_WhenMissing_ThrowsNotFoundException()
        {
            var bookings = new Mock<IBookingRepository>();
            var shows = new Mock<IShowRepository>();
            var sut = CreateSut(bookings, shows);

            bookings.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Booking?)null);

            await Assert.ThrowsAsync<NotFoundException>(() => sut.GetByIdAsync(123));
        }

        // 3) GetForUserAsync - trims userId and calls repo with trimmed
        [Fact]
        public async Task GetForUserAsync_TrimsUserId_BeforeRepoCall()
        {
            var bookings = new Mock<IBookingRepository>();
            var shows = new Mock<IShowRepository>();
            var sut = CreateSut(bookings, shows);

            bookings.Setup(r => r.GetBookingsForUserAsync("user-1"))
                    .ReturnsAsync(new List<Booking> { MakeBooking(id: 1, userId: "user-1") });

            var result = await sut.GetForUserAsync("  user-1  ");

            bookings.Verify(r => r.GetBookingsForUserAsync("user-1"), Times.Once);
            Assert.Single(result);
        }

        // 4) GetForUserAsync - empty => throws ValidationException
        [Fact]
        public async Task GetForUserAsync_WhenEmpty_ThrowsValidationException()
        {
            var bookings = new Mock<IBookingRepository>();
            var shows = new Mock<IShowRepository>();
            var sut = CreateSut(bookings, shows);

            await Assert.ThrowsAsync<ValidationException>(() => sut.GetForUserAsync("   "));
        }

        // 5) CreateAsync - null dto => throws ValidationException
        [Fact]
        public async Task CreateAsync_WhenDtoNull_ThrowsValidationException()
        {
            var bookings = new Mock<IBookingRepository>();
            var shows = new Mock<IShowRepository>();
            var sut = CreateSut(bookings, shows);

            await Assert.ThrowsAsync<ValidationException>(() => sut.CreateAsync(null!));
        }

        // 6) CreateAsync - show missing => throws ValidationException
        [Fact]
        public async Task CreateAsync_WhenShowMissing_ThrowsValidationException()
        {
            var bookings = new Mock<IBookingRepository>();
            var shows = new Mock<IShowRepository>();
            var sut = CreateSut(bookings, shows);

            shows.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Show?)null);

            var dto = new CreateBookingDto
            {
                UserId = "user-1",
                ShowId = 999,
                SeatIds = new List<int> { 1, 2 }
            };

            await Assert.ThrowsAsync<ValidationException>(() => sut.CreateAsync(dto));
        }

        // 7) CreateAsync - distinct seatIds + price set from show => verify booking passed to repo
        [Fact]
        public async Task CreateAsync_DeduplicatesSeatIds_AndSetsPriceFromShow()
        {
            var bookings = new Mock<IBookingRepository>();
            var shows = new Mock<IShowRepository>();
            var sut = CreateSut(bookings, shows);

            var show = MakeShow(id: 10, price: 150m);
            shows.Setup(r => r.GetByIdAsync(10)).ReturnsAsync(show);

            Booking? captured = null;

            bookings.Setup(r => r.CreateAsync(It.IsAny<Booking>()))
                    .Callback<Booking>(b => captured = b)
                    .ReturnsAsync(new Booking { Id = 77 });

            bookings.Setup(r => r.GetByIdAsync(77))
                    .ReturnsAsync(new Booking
                    {
                        Id = 77,
                        UserId = "user-1",
                        ShowId = 10,
                        Status = BookingStatus.Pending,
                        CreatedAt = DateTime.UtcNow,
                        Show = show,
                        BookingSeats = new List<BookingSeat>
                        {
                            new BookingSeat { SeatId = 1, PriceAtBooking = 150m },
                            new BookingSeat { SeatId = 2, PriceAtBooking = 150m }
                        }
                    });

            var dto = new CreateBookingDto
            {
                UserId = " user-1 ",
                ShowId = 10,
                SeatIds = new List<int> { 1, 1, 2 } // duplicates
            };

            var result = await sut.CreateAsync(dto);

            Assert.NotNull(captured);
            Assert.Equal("user-1", captured!.UserId); // trimmed
            Assert.Equal(2, captured.BookingSeats.Count); // deduplicated
            Assert.All(captured.BookingSeats, bs => Assert.Equal(150m, bs.PriceAtBooking));

            Assert.Equal(77, result.Id);
        }

        // 8) UpdateStatusAsync - invalid status string => throws ValidationException
        [Fact]
        public async Task UpdateStatusAsync_WhenStatusInvalid_ThrowsValidationException()
        {
            var bookings = new Mock<IBookingRepository>();
            var shows = new Mock<IShowRepository>();
            var sut = CreateSut(bookings, shows);

            await Assert.ThrowsAsync<ValidationException>(() => sut.UpdateStatusAsync(1, "NotARealStatus"));
        }

        // 9) UpdateStatusAsync - booking missing => throws NotFoundException
        [Fact]
        public async Task UpdateStatusAsync_WhenBookingMissing_ThrowsNotFoundException()
        {
            var bookings = new Mock<IBookingRepository>();
            var shows = new Mock<IShowRepository>();
            var sut = CreateSut(bookings, shows);

            bookings.Setup(r => r.GetByIdAsync(5)).ReturnsAsync((Booking?)null);

            await Assert.ThrowsAsync<NotFoundException>(() => sut.UpdateStatusAsync(5, "Confirmed"));
        }

        // 10) UpdateStatusAsync - valid => calls repo with parsed enum (case-insensitive)
        [Fact]
        public async Task UpdateStatusAsync_WhenValid_CallsRepositoryWithParsedEnum()
        {
            var bookings = new Mock<IBookingRepository>();
            var shows = new Mock<IShowRepository>();
            var sut = CreateSut(bookings, shows);

            bookings.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(MakeBooking(id: 5));
            bookings.Setup(r => r.UpdateStatusAsync(5, BookingStatus.Confirmed)).Returns(Task.CompletedTask);

            await sut.UpdateStatusAsync(5, "confirmed"); // lower-case on purpose

            bookings.Verify(r => r.UpdateStatusAsync(5, BookingStatus.Confirmed), Times.Once);
        }
    }
}
