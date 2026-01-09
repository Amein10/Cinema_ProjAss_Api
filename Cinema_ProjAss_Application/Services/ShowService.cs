using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cinema_ProjAss_Application.DTOs.Seats;
using Cinema_ProjAss_Application.DTOs.Shows;
using Cinema_ProjAss_Application.Exceptions;
using Cinema_ProjAss_Domain.Entities;
using Cinema_ProjAss_Domain.Interfaces;

namespace Cinema_ProjAss_Application.Services
{
    public class ShowService : IShowService
    {
        private readonly IShowRepository _shows;
        private readonly IMovieRepository _movies;

        // ✅ NYE repositories til seat-select
        private readonly ISeatRepository _seats;
        private readonly IBookingRepository _bookings;

        // ✅ OPDATERET constructor (injekter seats + bookings)
        public ShowService(
            IShowRepository shows,
            IMovieRepository movies,
            ISeatRepository seats,
            IBookingRepository bookings)
        {
            _shows = shows;
            _movies = movies;
            _seats = seats;
            _bookings = bookings;
        }

        public async Task<ShowDto> GetByIdAsync(int id)
        {
            var show = await _shows.GetByIdAsync(id);
            if (show == null) throw new NotFoundException($"Show with id={id} not found.");
            return MapToDto(show);
        }

        public async Task<IEnumerable<ShowDto>> GetShowsByMovieAsync(int movieId)
        {
            var list = await _shows.GetShowsByMovieAsync(movieId);
            return list.Select(MapToDto);
        }

        public async Task<IEnumerable<ShowDto>> GetUpcomingShowsAsync(DateTime fromDate)
        {
            var list = await _shows.GetUpcomingShowsAsync(fromDate);
            return list.Select(MapToDto);
        }

        public async Task<ShowDto> CreateAsync(CreateShowDto dto)
        {
            Validate(dto);

            var movie = await _movies.GetByIdAsync(dto.MovieId);
            if (movie == null) throw new ValidationException($"MovieId={dto.MovieId} does not exist.");

            var show = new Show
            {
                StartTime = dto.StartTime,
                Price = dto.Price,
                MovieId = dto.MovieId,
                AuditoriumId = dto.AuditoriumId
            };

            var created = await _shows.AddAsync(show);

            var loaded = await _shows.GetByIdAsync(created.Id);
            if (loaded == null) throw new NotFoundException("Show was created but could not be loaded.");

            return MapToDto(loaded);
        }

        public async Task UpdateAsync(int id, CreateShowDto dto)
        {
            if (id <= 0) throw new ValidationException("id must be > 0.");
            Validate(dto);

            var existing = await _shows.GetByIdAsync(id);
            if (existing == null) throw new NotFoundException($"Show with id={id} not found.");

            var movie = await _movies.GetByIdAsync(dto.MovieId);
            if (movie == null) throw new ValidationException($"MovieId={dto.MovieId} does not exist.");

            existing.StartTime = dto.StartTime;
            existing.Price = dto.Price;
            existing.MovieId = dto.MovieId;
            existing.AuditoriumId = dto.AuditoriumId;

            await _shows.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0) throw new ValidationException("id must be > 0.");

            var existing = await _shows.GetByIdAsync(id);
            if (existing == null) throw new NotFoundException($"Show with id={id} not found.");

            await _shows.DeleteAsync(id);
        }

        // ✅ NY: bruges af Angular seat-select til at få rigtige seats + booked status
        public async Task<IEnumerable<SeatStatusDto>> GetSeatStatusForShowAsync(int showId)
        {
            if (showId <= 0) throw new ValidationException("showId must be > 0.");

            var show = await _shows.GetByIdAsync(showId);
            if (show == null) throw new NotFoundException($"Show with id={showId} not found.");

            // 1) Alle seats i showets auditorium
            var allSeats = await _seats.GetByAuditoriumAsync(show.AuditoriumId);

            // 2) Hvilke seatIds er allerede booket for dette show (ignore cancelled)
            var bookedSeatIds = await _bookings.GetBookedSeatIdsForShowAsync(showId);
            var booked = bookedSeatIds.ToHashSet();

            // 3) Kombinér til seat-status DTO
            return allSeats
                .OrderBy(s => s.Row)
                .ThenBy(s => s.Number)
                .Select(s => new SeatStatusDto
                {
                    SeatId = s.Id,
                    Row = s.Row,
                    Number = s.Number,
                    IsBooked = booked.Contains(s.Id)
                })
                .ToList();
        }

        private static void Validate(CreateShowDto dto)
        {
            if (dto == null) throw new ValidationException("Body is required.");
            if (dto.MovieId <= 0) throw new ValidationException("MovieId must be > 0.");
            if (dto.AuditoriumId <= 0) throw new ValidationException("AuditoriumId must be > 0.");
            if (dto.Price < 0) throw new ValidationException("Price must be >= 0.");
            if (dto.StartTime == default) throw new ValidationException("StartTime is required.");
        }

        private static ShowDto MapToDto(Show s)
        {
            return new ShowDto
            {
                Id = s.Id,
                StartTime = s.StartTime,
                Price = s.Price,
                MovieId = s.MovieId,
                MovieTitle = s.Movie?.Title ?? string.Empty,
                AuditoriumId = s.AuditoriumId,
                AuditoriumName = s.Auditorium?.Name ?? string.Empty
            };
        }
    }
}
