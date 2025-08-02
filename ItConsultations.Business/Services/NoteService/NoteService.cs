using ItConsultations.Business.AutoMapperConfiguration;
using ItConsultations.Business.DataAccess.Interfaces;
using ItConsultations.Business.Dtos.NoteDtos;
using ItConsultations.Business.Entities.Notes;
using ItConsultations.Business.Exceptions;
using ItConsultations.Business.SharedTypes.Enums.Consultation;
using System.Text.Json;

namespace ItConsultations.Business.Services.NoteService;

public class NoteService : INoteService
{
    private readonly IRepository<Note, long> _noteRepository;

    public NoteService(IRepository<Note, long> noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public async Task<NoteDto> CreateAsync(CreateNoteDto dto, string userConsId)
    {
        var note = MapperManager.Map<Note>(dto);
        note.UserConsId = userConsId;
        note = await _noteRepository.CreateAsync(note);
        return MapperManager.Map<NoteDto>(note);
    }

    public async Task<NoteDto> GetAsync(long id)
    {
        var note = await _noteRepository.GetAsync(id);

        if (note == null)
        {
            throw new ConsultationsNotFoundException();
        }
            
        return MapperManager.Map<NoteDto>(note);
    }

    public async Task<NoteDto> GetAsync(string noteConsId)
    {
        var note = _noteRepository.Get(n => n.NoteConsId == noteConsId && n.DeletedAt == null).FirstOrDefault();

        if (note == null)
        {
            throw new ConsultationsNotFoundException();
        }

        return MapperManager.Map<NoteDto>(note);
    }

    public async Task<NoteDto> UpdateAsync(long id, UpdateNoteDto dto, long authorId)
    {
        var originalNote = await _noteRepository.GetAsync(id);
        var resultNote = MapperManager.Map(dto, originalNote);
        await _noteRepository.UpdateAsync(resultNote);
        return MapperManager.Map<NoteDto>(resultNote);
    }

    public async Task<NoteDto> DeleteAsync(DeleteNoteDto dto, long noteId)
    {
        var note = await _noteRepository.GetAsync(noteId);
        var mappedNote = MapperManager.Map<NoteDto>(note);
        await _noteRepository.DeleteAsync(note);
        return mappedNote;
    }

    public async Task<IEnumerable<NoteDto>> SearchAsync(NoteSearchDto searchDto)
    {
        var query = _noteRepository.Get(n => n.DeletedAt == null);

        if (!string.IsNullOrEmpty(searchDto.SearchText))
        {
            var searchText = searchDto.SearchText.ToLower();
            query = query.Where(n => n.Title.ToLower().Contains(searchText) ||
                                   n.Content.ToLower().Contains(searchText));
        }

        if (!string.IsNullOrEmpty(searchDto.Title))
        {
            query = query.Where(n => n.Title.Contains(searchDto.Title));
        }

        if (!string.IsNullOrEmpty(searchDto.Content))
        {
            query = query.Where(n => n.Content.Contains(searchDto.Content));
        }

        if (searchDto.Type.HasValue)
        {
            query = query.Where(n => n.Type == searchDto.Type.Value);
        }

        if (searchDto.Visibility.HasValue)
        {
            query = query.Where(n => n.Visibility == searchDto.Visibility.Value);
        }

        if (searchDto.Priority.HasValue)
        {
            query = query.Where(n => n.Priority == searchDto.Priority.Value);
        }

        if (searchDto.Status.HasValue)
        {
            query = query.Where(n => n.Status == searchDto.Status.Value);
        }

        if (searchDto.ConsultationId.HasValue)
        {
            query = query.Where(n => n.ConsultationId == searchDto.ConsultationId.Value);
        }

        if (searchDto.CoachId.HasValue)
        {
            query = query.Where(n => n.CoachId == searchDto.CoachId.Value);
        }

        if (searchDto.StudentId.HasValue)
        {
            query = query.Where(n => n.StudentId == searchDto.StudentId.Value);
        }

        if (searchDto.AuthorId.HasValue)
        {
            query = query.Where(n => n.AuthorId == searchDto.AuthorId.Value);
        }

        if (searchDto.Tags.Any())
        {
            query = query.Where(n => n.Tags.Any(tag => searchDto.Tags.Contains(tag)));
        }

        if (searchDto.CreatedFrom.HasValue)
        {
            query = query.Where(n => n.CreatedAt >= searchDto.CreatedFrom.Value);
        }

        if (searchDto.CreatedTo.HasValue)
        {
            query = query.Where(n => n.CreatedAt <= searchDto.CreatedTo.Value);
        }

        if (searchDto.UpdatedFrom.HasValue)
        {
            query = query.Where(n => n.UpdatedAt >= searchDto.UpdatedFrom.Value);
        }

        if (searchDto.UpdatedTo.HasValue)
        {
            query = query.Where(n => n.UpdatedAt <= searchDto.UpdatedTo.Value);
        }

        if (searchDto.ScheduledFrom.HasValue)
        {
            query = query.Where(n => n.ScheduledFor >= searchDto.ScheduledFrom.Value);
        }

        if (searchDto.ScheduledTo.HasValue)
        {
            query = query.Where(n => n.ScheduledFor <= searchDto.ScheduledTo.Value);
        }

        if (!string.IsNullOrEmpty(searchDto.Location))
        {
            query = query.Where(n => n.Location == searchDto.Location);
        }

        if (!string.IsNullOrEmpty(searchDto.Source))
        {
            query = query.Where(n => n.Source == searchDto.Source);
        }

        if (searchDto.IsPinned.HasValue)
        {
            query = query.Where(n => n.IsPinned == searchDto.IsPinned.Value);
        }

        query = searchDto.SortBy?.ToLower() switch
        {
            "title" => searchDto.SortDirection == "asc" ? query.OrderBy(n => n.Title) : query.OrderByDescending(n => n.Title),
            "createdat" => searchDto.SortDirection == "asc" ? query.OrderBy(n => n.CreatedAt) : query.OrderByDescending(n => n.CreatedAt),
            "updatedat" => searchDto.SortDirection == "asc" ? query.OrderBy(n => n.UpdatedAt) : query.OrderByDescending(n => n.UpdatedAt),
            "priority" => searchDto.SortDirection == "asc" ? query.OrderBy(n => n.Priority) : query.OrderByDescending(n => n.Priority),
            "type" => searchDto.SortDirection == "asc" ? query.OrderBy(n => n.Type) : query.OrderByDescending(n => n.Type),
            _ => query.OrderByDescending(n => n.CreatedAt)
        };

        var notes = query.ToList();
        return notes.Select(MapperManager.Map<NoteDto>).ToList();
    }

    public async Task<IEnumerable<NoteDto>> GetUserNotesAsync(long userId)
    {
        var notes = _noteRepository.Get(n => n.AuthorId == userId && n.DeletedAt == null)
                                 .OrderByDescending(n => n.CreatedAt)
                                 .ToList();

        return notes.Select(MapperManager.Map<NoteDto>).ToList();
    }

    public async Task<IEnumerable<NoteDto>> GetUserNotesAsync(string userConsId)
    {
        var notes = _noteRepository.Get(x => x.UserConsId == userConsId).ToList();
        return notes.Select(MapperManager.Map<NoteDto>).ToList();
    }

    public async Task<IEnumerable<NoteDto>> GetConsultationNotesAsync(long consultationId)
    {
        var notes = _noteRepository.Get(n => n.ConsultationId == consultationId && n.DeletedAt == null)
                                 .OrderByDescending(n => n.CreatedAt)
                                 .ToList();

        return notes.Select(MapperManager.Map<NoteDto>).ToList();
    }

    public async Task<NoteDto> ArchiveNoteAsync(long id)
    {
        var noteDto = await GetAsync(id);

        noteDto.Status = NoteStatus.Archived;
        noteDto.UpdatedAt = DateTime.UtcNow;

        var note = MapperManager.Map<Note>(noteDto);

        await _noteRepository.UpdateAsync(note);
        return MapperManager.Map<NoteDto>(note);
    }

    public async Task<NoteDto> RestoreNoteAsync(long id, long userId)
    {
        var noteDto = await GetAsync(id);

        noteDto.Status = NoteStatus.Active;
        noteDto.UpdatedAt = DateTime.UtcNow;

        var note = MapperManager.Map<Note>(noteDto);

        note = await _noteRepository.UpdateAsync(note);
        return MapperManager.Map<NoteDto>(note);
    }

    public async Task<IEnumerable<NoteDto>> GetNotesByTagsAsync(IEnumerable<string> tags, long userId)
    {
        var notes = _noteRepository.Get(n => n.AuthorId == userId &&
                                           n.Tags.Any(tag => tags.ToList().Contains(tag)) &&
                                           n.DeletedAt == null)
                                 .OrderByDescending(n => n.CreatedAt)
                                 .ToList();

        return notes.Select(MapperManager.Map<NoteDto>).ToList();
    }

    public async Task<string> ExportNotesToJsonAsync(IEnumerable<long> noteIds, long userId)
    {
        var noteDtos = new List<NoteDto>();
        var notes = _noteRepository.Get(n => noteIds.Contains(n.Id) && n.DeletedAt == null).ToList();

        foreach (var note in notes)
        {
            noteDtos.Add(MapperManager.Map<NoteDto>(note));
        }

        var exportData = new
        {
            ExportedAt = DateTime.UtcNow,
            NotesCount = noteDtos.Count,
            Notes = noteDtos
        };

        return JsonSerializer.Serialize(exportData, new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }

    public Task<IEnumerable<NoteDto>> ExportNotesToPdfAsync(IEnumerable<long> noteIds, long userId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<NoteDto>> ExportNotesToDocxAsync(IEnumerable<long> noteIds, long userId)
    {
        throw new NotImplementedException();
    }

    public Task<NoteDto> ExportNoteToPngAsync(long noteId, long userId)
    {
        throw new NotImplementedException();
    }
}