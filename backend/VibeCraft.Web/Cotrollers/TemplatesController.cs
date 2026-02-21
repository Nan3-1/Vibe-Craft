using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VibeCraft.Models.ViewModels;
using VibeCraft.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace VibeCraft.Controllers
{
    /// <summary>
    /// Controller for managing templates
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Require authentication for all endpoints
    public class TemplateController : ControllerBase
    {
        private readonly ITemplateService _templateService;
        private readonly ILogger<TemplateController> _logger;

        public TemplateController(
            ITemplateService templateService,
            ILogger<TemplateController> logger)
        {
            _templateService = templateService;
            _logger = logger;
        }

        /// <summary>
        /// Get all templates with optional filtering
        /// </summary>
        /// <param name="filter">Optional filter parameters</param>
        /// <returns>List of templates</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TemplateViewModel>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<TemplateViewModel>>> GetAllTemplates([FromQuery] TemplateFilterViewModel filter = null)
        {
            try
            {
                var templates = await _templateService.GetAllTemplatesAsync(filter);
                return Ok(templates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving templates");
                return StatusCode(500, "An error occurred while retrieving templates");
            }
        }

        /// <summary>
        /// Get a specific template by ID
        /// </summary>
        /// <param name="id">Template ID</param>
        /// <returns>Template details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TemplateDetailsViewModel), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<TemplateDetailsViewModel>> GetTemplateById(int id)
        {
            try
            {
                var template = await _templateService.GetTemplateByIdAsync(id);
                
                if (template == null)
                {
                    return NotFound($"Template with ID {id} not found");
                }

                return Ok(template);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving template with ID {TemplateId}", id);
                return StatusCode(500, "An error occurred while retrieving the template");
            }
        }

        /// <summary>
        /// Create a new template
        /// </summary>
        /// <param name="createModel">Template creation data</param>
        /// <returns>Created template</returns>
        [HttpPost]
        [ProducesResponseType(typeof(TemplateViewModel), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<TemplateViewModel>> CreateTemplate([FromBody] CreateTemplateViewModel createModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var template = await _templateService.CreateTemplateAsync(createModel);
                
                return CreatedAtAction(nameof(GetTemplateById), new { id = template.Id }, template);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Validation error creating template");
                return Conflict(ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid argument creating template");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating template");
                return StatusCode(500, "An error occurred while creating the template");
            }
        }

        /// <summary>
        /// Delete a template
        /// </summary>
        /// <param name="id">Template ID</param>
        /// <returns>No content if successful</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteTemplate(int id)
        {
            try
            {
                var deleted = await _templateService.DeleteTemplateAsync(id);
                
                if (!deleted)
                {
                    return NotFound($"Template with ID {id} not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting template with ID {TemplateId}", id);
                return StatusCode(500, "An error occurred while deleting the template");
            }
        }

        /// <summary>
        /// Get templates by event type
        /// </summary>
        /// <param name="eventType">Event type to filter by</param>
        /// <returns>List of templates for the specified event type</returns>
        [HttpGet("by-event-type/{eventType}")]
        [ProducesResponseType(typeof(IEnumerable<TemplateViewModel>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<TemplateViewModel>>> GetTemplatesByEventType(EventType eventType)
        {
            try
            {
                if (!Enum.IsDefined(typeof(EventType), eventType))
                {
                    return BadRequest($"Invalid event type: {eventType}");
                }

                var templates = await _templateService.GetTemplatesByEventTypeAsync(eventType);
                return Ok(templates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving templates for event type {EventType}", eventType);
                return StatusCode(500, "An error occurred while retrieving templates");
            }
        }

        /// <summary>
        /// Get active templates
        /// </summary>
        /// <returns>List of active templates</returns>
        [HttpGet("active")]
        [ProducesResponseType(typeof(IEnumerable<TemplateViewModel>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<TemplateViewModel>>> GetActiveTemplates()
        {
            try
            {
                var templates = await _templateService.GetActiveTemplatesAsync();
                return Ok(templates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active templates");
                return StatusCode(500, "An error occurred while retrieving active templates");
            }
        }

        /// <summary>
        /// Toggle template active status
        /// </summary>
        /// <param name="id">Template ID</param>
        /// <returns>Success status</returns>
        [HttpPatch("{id}/toggle-status")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ToggleTemplateStatus(int id)
        {
            try
            {
                var toggled = await _templateService.ToggleTemplateStatusAsync(id);
                
                if (!toggled)
                {
                    return NotFound($"Template with ID {id} not found");
                }

                return Ok(new { message = $"Template status toggled successfully for ID {id}" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling status for template with ID {TemplateId}", id);
                return StatusCode(500, "An error occurred while toggling template status");
            }
        }

        /// <summary>
        /// Check if template name is unique
        /// </summary>
        /// <param name="name">Template name to check</param>
        /// <param name="excludeId">Optional ID to exclude from check (for updates)</param>
        /// <returns>True if name is unique</returns>
        [HttpGet("check-name-uniqueness")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<bool>> CheckTemplateNameUniqueness([FromQuery] string name, [FromQuery] int? excludeId = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return BadRequest("Template name cannot be empty");
                }

                var isUnique = await _templateService.IsTemplateNameUniqueAsync(name, excludeId);
                return Ok(isUnique);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking name uniqueness for template: {TemplateName}", name);
                return StatusCode(500, "An error occurred while checking template name uniqueness");
            }
        }

        /// <summary>
        /// Check if a template exists
        /// </summary>
        /// <param name="id">Template ID</param>
        /// <returns>True if template exists</returns>
        [HttpGet("exists/{id}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<bool>> TemplateExists(int id)
        {
            try
            {
                var exists = await _templateService.TemplateExistsAsync(id);
                return Ok(exists);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking existence for template with ID {TemplateId}", id);
                return StatusCode(500, "An error occurred while checking template existence");
            }
        }

        
    }
}