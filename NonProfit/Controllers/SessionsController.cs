using Microsoft.AspNetCore.Mvc;
using NonProfit.DTOs;
using NonProfit.Entities;
using NonProfit.Services.Session;
using System.Security.Claims;

namespace NonProfit.Controllers
{
        [ApiController]
        [Route("api/[controller]")]
        public class SessionsController : ControllerBase
        {
            private readonly ISessionService _sessionService;
            private readonly IBookingService _bookingService;

            public SessionsController(ISessionService sessionService, IBookingService bookingService)
            {
                _sessionService = sessionService;
                _bookingService = bookingService;
            }

            [HttpPost]
       
        public async Task<IActionResult> CreateSession([FromBody] CreateSessionDTO sessionDto)
        {
            var therapistId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var session = new Session
            {
                Title = sessionDto.Title,
                Description = sessionDto.Description,
                StartTime = sessionDto.StartTime,
                EndTime = sessionDto.EndTime,
                Location = sessionDto.Location,
                TherapistId = therapistId, 
                IsBooked = false 
            };

            var response = new SessionCreationResponse
            {
                Success = true,
                Message = "Session created successfully.",
                SessionId = session.Id
            };

            return Ok(response);
        }


        [HttpGet("{id}")]
            public async Task<IActionResult> GetSession(int id)
            {
                var session = await _sessionService.GetSessionByIdAsync(id);
                if (session == null) return NotFound();

                return Ok(session);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateSession(int id, Session session)
            {
                if (id != session.Id) return BadRequest();

                var updated = await _sessionService.UpdateSessionAsync(session);
                if (!updated) return NotFound();

                return NoContent();
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteSession(int id)
            {
                var deleted = await _sessionService.DeleteSessionAsync(id);
                if (!deleted) return NotFound();

                return NoContent();
            }
        [HttpPost("book-session/{sessionId}")]

        public async Task<IActionResult> BookSession(int sessionId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Booking booking;
            try
            {
                booking = await _bookingService.BookSessionAsync(sessionId, userId);
            }
            catch (InvalidOperationException ex)
            {
                var errorResponse = new BookingResponse
                {
                    Success = false,
                    Message = ex.Message,
                    Booking = null,
                    Errors = new List<string> { ex.Message }
                };
                return BadRequest(errorResponse);
            }

            var bookingDto = new BookingDto
            {
                Id = booking.Id,
                SessionId = booking.SessionId,
                BookingTime = booking.BookingTime
            };

            var successResponse = new BookingResponse
            {
                Success = true,
                Message = "Session booked successfully.",
                Booking = bookingDto,
                Errors = new List<string>()
            };

            return Ok(successResponse);
        }



        [HttpGet("my-bookings")]
            public async Task<IActionResult> GetMyBookings()
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var bookings = await _bookingService.GetBookingsForUserAsync(userId);
                return Ok(bookings);
            }
        }

    }

