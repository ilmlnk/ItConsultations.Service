using ItConsultations.Business.DataAccess.Interfaces;
using ItConsultations.Business.Entities.Notes;

namespace ItConsultations.Business.Services.NoteService.NoteExportService;

public class NoteExportService : INoteExportService
{
    private readonly IRepository<Note, long> _repository;

    public NoteExportService(IRepository<Note, long> repository)
    {
        _repository = repository;
    }

    public async Task<byte[]> ExportNoteToPngAsync(long noteId)
    {
        /*var note = await _repository.GetAsync(noteId);
        var noteDto =*/
        throw new NotImplementedException();
    }

    public Task<byte[]> ExportNotesToDocxAsync(IEnumerable<long> noteIds)
    {
        throw new NotImplementedException();
    }

    public Task<byte[]> ExportNotseToPdfAsync(IEnumerable<long> noteIds)
    {
        throw new NotImplementedException();
    }
}
