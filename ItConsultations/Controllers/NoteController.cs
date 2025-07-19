using ItConsultations.Business.Dtos.NoteDtos;
using ItConsultations.Business.Services.NoteService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace ItConsultations.Controllers;

[ApiController]
[Route("api/notes")]
[Authorize]
public class NoteController : ControllerBase
{
    private readonly INoteService _noteService;
    private readonly ILogger<NoteController> _logger;

    public NoteController(INoteService noteService, ILogger<NoteController> logger)
    {
        _noteService = noteService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateNote([FromBody] CreateNoteDto dto)
    {
        try
        {
            var userId = GetCurrentUserId();
            var note = await _noteService.CreateAsync(dto, userId);
            
            _logger.LogInformation("Note created by user {UserId}", userId);
            return CreatedAtAction(nameof(GetNote), new { id = note.Id }, note);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating note");
            return StatusCode(500, "Internal server error when creating note");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetNote(long id)
    {
        try
        {
            var userId = GetCurrentUserId();
            
            if (!await _noteService.CanUserAccessNoteAsync(id, userId))
            {
                return Forbid();
            }

            var note = await _noteService.GetAsync(id);
            if (note == null)
            {
                return NotFound("Note not found");
            }

            await _noteService.MarkAsViewedAsync(id, userId);
            
            return Ok(note);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting note {NoteId}", id);
            return StatusCode(500, "Internal server error when getting note");
        }
    }

    [HttpGet("cons/{consId}")]
    public async Task<IActionResult> GetNoteByConsId(string consId)
    {
        try
        {
            var userId = GetCurrentUserId();
            var note = await _noteService.GetByConsIdAsync(consId);
            
            if (note == null)
            {
                return NotFound("Note not found");
            }

            if (!await _noteService.CanUserAccessNoteAsync(note.Id, userId))
            {
                return Forbid();
            }

            await _noteService.MarkAsViewedAsync(note.Id, userId);
            
            return Ok(note);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting note by cons ID {ConsId}", consId);
            return StatusCode(500, "Internal server error when getting note");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNote(long id, [FromBody] UpdateNoteDto dto)
    {
        try
        {
            var userId = GetCurrentUserId();
            
            if (!await _noteService.CanUserEditNoteAsync(id, userId))
            {
                return Forbid();
            }

            var note = await _noteService.UpdateAsync(id, dto, userId);
            
            _logger.LogInformation("Note {NoteId} updated by user {UserId}", id, userId);
            return Ok(note);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating note {NoteId}", id);
            return StatusCode(500, "Internal server error when updating note");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNote(long id)
    {
        try
        {
            var userId = GetCurrentUserId();
            
            if (!await _noteService.CanUserDeleteNoteAsync(id, userId))
            {
                return Forbid();
            }

            var success = await _noteService.SoftDeleteAsync(id, userId);
            
            if (success)
            {
                _logger.LogInformation("Note {NoteId} deleted by user {UserId}", id, userId);
                return Ok(new { message = "Note deleted successfully" });
            }
            else
            {
                return NotFound("Note not found");
            }
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting note {NoteId}", id);
            return StatusCode(500, "Internal server error when deleting note");
        }
    }

    [HttpPost("search")]
    public async Task<IActionResult> SearchNotes([FromBody] NoteSearchDto searchDto)
    {
        try
        {
            var notes = await _noteService.SearchAsync(searchDto);
            return Ok(notes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching notes");
            return StatusCode(500, "Internal server error when searching notes");
        }
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetUserNotes([FromQuery] int pageSize = 20, [FromQuery] int pageNumber = 1)
    {
        try
        {
            var userId = GetCurrentUserId();
            var notes = await _noteService.GetUserNotesAsync(userId, pageSize, pageNumber);
            return Ok(notes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user notes");
            return StatusCode(500, "Internal server error when getting user notes");
        }
    }

    [HttpGet("consultation/{consultationId}")]
    public async Task<IActionResult> GetConsultationNotes(long consultationId)
    {
        try
        {
            var userId = GetCurrentUserId();
            var notes = await _noteService.GetConsultationNotesAsync(consultationId, userId);
            return Ok(notes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting consultation notes for consultation {ConsultationId}", consultationId);
            return StatusCode(500, "Internal server error when getting consultation notes");
        }
    }

    [HttpGet("coach/{coachId}")]
    public async Task<IActionResult> GetCoachNotes(long coachId)
    {
        try
        {
            var userId = GetCurrentUserId();
            var notes = await _noteService.GetCoachNotesAsync(coachId, userId);
            return Ok(notes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting coach notes for coach {CoachId}", coachId);
            return StatusCode(500, "Internal server error when getting coach notes");
        }
    }

    [HttpGet("student/{studentId}")]
    public async Task<IActionResult> GetStudentNotes(long studentId)
    {
        try
        {
            var userId = GetCurrentUserId();
            var notes = await _noteService.GetStudentNotesAsync(studentId, userId);
            return Ok(notes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting student notes for student {StudentId}", studentId);
            return StatusCode(500, "Internal server error when getting student notes");
        }
    }
    
    [HttpPost("{id}/pin")]
    public async Task<IActionResult> PinNote(long id)
    {
        try
        {
            var userId = GetCurrentUserId();
            var note = await _noteService.PinNoteAsync(id, userId);
            
            _logger.LogInformation("Note {NoteId} pinned by user {UserId}", id, userId);
            return Ok(note);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error pinning note {NoteId}", id);
            return StatusCode(500, "Internal server error when pinning note");
        }
    }

    [HttpPost("{id}/unpin")]
    public async Task<IActionResult> UnpinNote(long id)
    {
        try
        {
            var userId = GetCurrentUserId();
            var note = await _noteService.UnpinNoteAsync(id, userId);
            
            _logger.LogInformation("Note {NoteId} unpinned by user {UserId}", id, userId);
            return Ok(note);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error unpinning note {NoteId}", id);
            return StatusCode(500, "Internal server error when unpinning note");
        }
    }

    [HttpPost("{id}/archive")]
    public async Task<IActionResult> ArchiveNote(long id)
    {
        try
        {
            var userId = GetCurrentUserId();
            var note = await _noteService.ArchiveNoteAsync(id, userId);
            
            _logger.LogInformation("Note {NoteId} archived by user {UserId}", id, userId);
            return Ok(note);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error archiving note {NoteId}", id);
            return StatusCode(500, "Internal server error when archiving note");
        }
    }

    [HttpPost("{id}/restore")]
    public async Task<IActionResult> RestoreNote(long id)
    {
        try
        {
            var userId = GetCurrentUserId();
            var note = await _noteService.RestoreNoteAsync(id, userId);
            
            _logger.LogInformation("Note {NoteId} restored by user {UserId}", id, userId);
            return Ok(note);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error restoring note {NoteId}", id);
            return StatusCode(500, "Internal server error when restoring note");
        }
    }

    [HttpGet("tags/popular")]
    public async Task<IActionResult> GetPopularTags()
    {
        try
        {
            var userId = GetCurrentUserId();
            var tags = await _noteService.GetPopularTagsAsync(userId);
            return Ok(tags);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting popular tags");
            return StatusCode(500, "Internal server error when getting popular tags");
        }
    }

    [HttpGet("tags/{tag}")]
    public async Task<IActionResult> GetNotesByTag(string tag)
    {
        try
        {
            var userId = GetCurrentUserId();
            var notes = await _noteService.GetNotesByTagAsync(tag, userId);
            return Ok(notes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting notes by tag {Tag}", tag);
            return StatusCode(500, "Internal server error when getting notes by tag");
        }
    }

    [HttpGet("stats/count")]
    public async Task<IActionResult> GetUserNotesCount()
    {
        try
        {
            var userId = GetCurrentUserId();
            var count = await _noteService.GetUserNotesCountAsync(userId);
            return Ok(new { count });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user notes count");
            return StatusCode(500, "Internal server error when getting notes count");
        }
    }

    [HttpGet("consultation/{consultationId}/count")]
    public async Task<IActionResult> GetConsultationNotesCount(long consultationId)
    {
        try
        {
            var count = await _noteService.GetConsultationNotesCountAsync(consultationId);
            return Ok(new { count });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting consultation notes count for consultation {ConsultationId}", consultationId);
            return StatusCode(500, "Internal server error when getting consultation notes count");
        }
    }

    [HttpGet("stats/by-type")]
    public async Task<IActionResult> GetNotesByTypeStats()
    {
        try
        {
            var userId = GetCurrentUserId();
            var stats = await _noteService.GetNotesByTypeStatsAsync(userId);
            return Ok(stats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting notes by type stats");
            return StatusCode(500, "Internal server error when getting notes stats");
        }
    }

    [HttpPost("export/text")]
    public async Task<IActionResult> ExportNotesToText([FromBody] ExportNotesDto exportDto)
    {
        try
        {
            var userId = GetCurrentUserId();
            var text = await _noteService.ExportNotesToTextAsync(exportDto.NoteIds, userId);
            
            var fileName = $"notes_export_{DateTime.UtcNow:yyyyMMdd_HHmmss}.txt";
            
            _logger.LogInformation("Notes exported to text by user {UserId}", userId);
            return File(Encoding.UTF8.GetBytes(text), "text/plain", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting notes to text");
            return StatusCode(500, "Internal server error when exporting notes");
        }
    }

    [HttpPost("export/json")]
    public async Task<IActionResult> ExportNotesToJson([FromBody] ExportNotesDto exportDto)
    {
        try
        {
            var userId = GetCurrentUserId();
            var json = await _noteService.ExportNotesToJsonAsync(exportDto.NoteIds, userId);
            
            var fileName = $"notes_export_{DateTime.UtcNow:yyyyMMdd_HHmmss}.json";
            
            _logger.LogInformation("Notes exported to JSON by user {UserId}", userId);
            return File(Encoding.UTF8.GetBytes(json), "application/json", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting notes to JSON");
            return StatusCode(500, "Internal server error when exporting notes");
        }
    }

    private long GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("Invalid user ID");
        }
        return userId;
    }
}

public class ExportNotesDto
{
    public List<long> NoteIds { get; set; } = new();
} 