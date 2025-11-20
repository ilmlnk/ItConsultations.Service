using ItConsultations.Business.Dtos.NoteDtos;
using ItConsultations.Business.Services.NoteService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    [HttpPost("note/{userConsId}")]
    public async Task<IActionResult> CreateNote([FromBody] CreateNoteDto dto, string userConsId)
    {
        var note = await _noteService.CreateAsync(dto, userConsId);
        return Ok(note);
    }

    [HttpGet("{noteId}")]
    public async Task<IActionResult> GetNote(long noteId)
    {
        var note = await _noteService.GetAsync(noteId);
        return Ok(note);
    }

    [HttpGet("note/{noteConsId}")]
    public async Task<IActionResult> GetNote(string noteConsId)
    {
        var note = await _noteService.GetAsync(noteConsId);
        return Ok(note);
    }

    [HttpGet("all-notes/{userConsId}")]
    public async Task<IActionResult> GetUserNotes(string userConsId)
    {
        var notes = await _noteService.GetUserNotesAsync(userConsId);
        return Ok(notes);
    }

    [HttpGet("cons/{consId}")]
    public async Task<IActionResult> GetNoteByConsId(string consId)
    {
        var note = await _noteService.GetAsync(consId);

        // TODO: add another method for accessing note
        return Ok(note);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNote(long id, [FromBody] UpdateNoteDto dto, long authorId)
    {
        var note = await _noteService.UpdateAsync(id, dto, authorId);
        return Ok(note);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNote([FromBody] DeleteNoteDto dto, long noteId)
    {
        var deletedNote = await _noteService.DeleteAsync(dto, noteId);
        return Ok(deletedNote);
    }

    [HttpPost("search")]
    public async Task<IActionResult> SearchNotes([FromBody] NoteSearchDto searchDto)
    {
        var notes = await _noteService.SearchAsync(searchDto);
        return Ok(notes);
    }

    [HttpGet("consultation/{consultationId}")]
    public async Task<IActionResult> GetConsultationNotes(long consultationId)
    {
        var notes = await _noteService.GetConsultationNotesAsync(consultationId);
        return Ok(notes);
    }

    [HttpPost("archive/{noteId}")]
    public async Task<IActionResult> ArchiveNote(long noteId)
    {
        var note = await _noteService.ArchiveNoteAsync(noteId);
        return Ok(note);
    }

    [HttpPost("restore/{userId}/{noteId}")]
    public async Task<IActionResult> RestoreNote(long noteId, long userId)
    {
        var note = await _noteService.RestoreNoteAsync(noteId, userId);
        return Ok(note);
    }

    [HttpGet("note/tags/{userId}")]
    public async Task<IActionResult> GetNotesByTag([FromQuery] IEnumerable<string> tags, long userId)
    {
        var notes = await _noteService.GetNotesByTagsAsync(tags, userId);
        return Ok(notes);
    }

    [HttpPost("export/{userId}/json")]
    public async Task<IActionResult> ExportNotesToJson([FromBody] ExportNotesDto exportDto, long userId)
    {
        var json = await _noteService.ExportNotesToJsonAsync(exportDto.NoteIds, userId);
        var fileName = $"notes_export_{DateTime.UtcNow:yyyyMMdd_HHmmss}.json";
        return File(Encoding.UTF8.GetBytes(json), "application/json", fileName);
    }

    /*[HttpPost("export/pdf")]
    public async Task<IActionResult> ExportNotesToPdf([FromBody] ExportNotesDto exportDto)
    {
        var 
    }*/
}

