using ItConsultations.Business.Dtos.NoteDtos;

namespace ItConsultations.Business.Services.NoteService;

public interface INoteService
{
    Task<NoteDto> CreateAsync(CreateNoteDto dto, string userConsId);

    Task<NoteDto> GetAsync(long id);

    Task<NoteDto> GetAsync(string noteConsId);

    Task<NoteDto> UpdateAsync(long id, UpdateNoteDto dto, long authorId);

    Task<NoteDto> DeleteAsync(DeleteNoteDto dto, long noteId);

    Task<IEnumerable<NoteDto>> SearchAsync(NoteSearchDto searchDto);

    Task<IEnumerable<NoteDto>> GetUserNotesAsync(long userId);

    Task<IEnumerable<NoteDto>> GetUserNotesAsync(string userConsId);

    Task<IEnumerable<NoteDto>> GetConsultationNotesAsync(long consultationId);

    Task<NoteDto> ArchiveNoteAsync(long id);

    Task<NoteDto> RestoreNoteAsync(long id, long userId);

    Task<IEnumerable<NoteDto>> GetNotesByTagsAsync(IEnumerable<string> tags, long userId);

    Task<string> ExportNotesToJsonAsync(IEnumerable<long> noteIds, long userId);

    // export to PDF
    Task<IEnumerable<NoteDto>> ExportNotesToPdfAsync(IEnumerable<long> noteIds, long userId);

    // export to Docx
    Task<IEnumerable<NoteDto>> ExportNotesToDocxAsync(IEnumerable<long> noteIds, long userId);

    // export to PNG
    Task<NoteDto> ExportNoteToPngAsync(long noteId, long userId);
}