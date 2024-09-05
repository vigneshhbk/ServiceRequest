using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceRequestDemo.Models;
using ServiceRequestDemo.Service.Interfaces;


namespace ServiceRequestDemo.Controllers
{
    [ApiController]
    public class ServiceRequestController : ControllerBase
    {
        private readonly IServiceRequestService _serviceRequestService;
        private readonly ILogger<ServiceRequestController> _logger;

        public ServiceRequestController(IServiceRequestService serviceRequestService, ILogger<ServiceRequestController> logger)
        {
            _serviceRequestService = serviceRequestService;
            _logger = logger;
        }

        [HttpGet("api/servicerequest")]
        public ActionResult<List<ServiceRequestDTO>> Get()
        {
            try
            {
                List<ServiceRequestDTO>? list = _serviceRequestService.GetServiceRequests();
                if (list == null)
                    return NoContent();
                else
                    return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(500);
            }
            
        }

        [HttpGet("api/servicerequest/{id}")]
        public ActionResult<ServiceRequestDTO> GetById(Guid id)
        {
            try
            {
                ServiceRequestDTO? sr = _serviceRequestService.GetServiceRequestById(id);
                if (sr == null)
                    return NotFound();
                else
                    return Ok(sr);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(500);
            }
        }

        [HttpPost("api/servicerequest")]
        public IActionResult AddServiceRequest([FromBody] ServiceRequestDTO serviceRequestDTO)
        {
            try
            {
                if (!ModelState.IsValid || serviceRequestDTO == null)
                    return BadRequest(ModelState);

                ServiceRequestDTO? sr = _serviceRequestService.AddServiceRequest(serviceRequestDTO);

                if (sr == null)
                    return StatusCode(500);

                return CreatedAtAction("AddServiceRequest", new { Id = sr.Id }, sr);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(500);
            }
        }

        [HttpPut("api/servicerequest/{id}")]
        public IActionResult UpdateServiceRequest(Guid id, [FromBody] ServiceRequestDTO serviceRequestDTO)
        {
            try
            {
                if (!ModelState.IsValid || serviceRequestDTO == null ||  id != serviceRequestDTO.Id || !serviceRequestDTO.Id.HasValue)
                    return BadRequest();

                ServiceRequestDTO? updatedRequest = _serviceRequestService.UpdateServiceRequest(serviceRequestDTO);

                if (updatedRequest != null)
                    return Ok(updatedRequest);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(500);
            }
        }

        [HttpDelete("api/servicerequest/{id}")]
        public IActionResult DeleteServiceRequest(Guid id)
        {
            try
            {
                if (_serviceRequestService.DeleteServiceRequest(id))
                    return Ok();
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(500);
            }
        }
    }
}
