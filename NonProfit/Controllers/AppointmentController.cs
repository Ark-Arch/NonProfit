using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NonProfit.Entities;
using NonProfit.Models.Responses;
using NonProfit.Services.Interfaces;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NonProfit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppointmentController(IAppointmentService appointmentService, IHttpContextAccessor httpContextAccessor)
        {
            _appointmentService = appointmentService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<IActionResult> BookAppointment(Appointment appointment)
        {
            try
            {
                var bookedAppointment = await _appointmentService.BookAppointmentAsync(appointment);
                var response = new ApiResponse<Appointment>
                {
                    Status = "success",
                    Message = "Appointment booked successfully.",
                    Data = bookedAppointment
                };
                return CreatedAtAction(nameof(GetAppointmentById), new { id = bookedAppointment.Id }, response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<object>
                {
                    Status = "error",
                    Message = "An error occurred while booking the appointment.",
                    Error = new ApiError
                    {
                        Code = "BOOKING_ERROR",
                        Details = ex.Message
                    }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointmentById(Guid id)
        {
            try
            {
                var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
                if (appointment == null)
                {
                    return NotFound();
                }
                var response = new ApiResponse<Appointment>
                {
                    Status = "success",
                    Message = "Appointment retrieved successfully.",
                    Data = appointment
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<object>
                {
                    Status = "error",
                    Message = "An error occurred while retrieving the appointment.",
                    Error = new ApiError
                    {
                        Code = "RETRIEVAL_ERROR",
                        Details = ex.Message
                    }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetAllAppointmentsForUser()
        {
            try
            {
   
                var appointments = await _appointmentService.GetAllAppointmentsForUserAsync();
                var response = new ApiResponse<IEnumerable<Appointment>>
                {
                    Status = "success",
                    Message = "Appointments for user retrieved successfully.",
                    Data = appointments
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<object>
                {
                    Status = "error",
                    Message = "An error occurred while retrieving appointments for the user.",
                    Error = new ApiError
                    {
                        Code = "RETRIEVAL_ERROR",
                        Details = ex.Message
                    }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAppointments()
        {
            try
            {
                var appointments = await _appointmentService.GetAllAppointmentsAsync();
                var response = new ApiResponse<IEnumerable<Appointment>>
                {
                    Status = "success",
                    Message = "All appointments retrieved successfully.",
                    Data = appointments
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<object>
                {
                    Status = "error",
                    Message = "An error occurred while retrieving all appointments.",
                    Error = new ApiError
                    {
                        Code = "RETRIEVAL_ERROR",
                        Details = ex.Message
                    }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPut("{appointmentId}")]
        public async Task<IActionResult> RescheduleAppointment(Guid appointmentId, [FromBody] DateTime newDate)
        {
            try
            {
                var result = await _appointmentService.RescheduleAppointmentAsync(appointmentId, newDate);
                if (result)
                {
                    var response = new ApiResponse<object>
                    {
                        Status = "success",
                        Message = "Appointment rescheduled successfully."
                    };
                    return NoContent(); // No content for successful update
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<object>
                {
                    Status = "error",
                    Message = "An error occurred while rescheduling the appointment.",
                    Error = new ApiError
                    {
                        Code = "RESCHEDULING_ERROR",
                        Details = ex.Message
                    }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpDelete("{appointmentId}")]
        public async Task<IActionResult> CancelAppointment(Guid appointmentId)
        {
            try
            {
                var result = await _appointmentService.CancelAppointmentAsync(appointmentId);
                if (result)
                {
                    var response = new ApiResponse<object>
                    {
                        Status = "success",
                        Message = "Appointment canceled successfully."
                    };
                    return NoContent(); // No content for successful delete
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<object>
                {
                    Status = "error",
                    Message = "An error occurred while canceling the appointment.",
                    Error = new ApiError
                    {
                        Code = "CANCELLATION_ERROR",
                        Details = ex.Message
                    }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

    }
}
