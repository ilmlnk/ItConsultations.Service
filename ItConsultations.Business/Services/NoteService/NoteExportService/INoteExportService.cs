using Org.BouncyCastle.Crypto.Paddings;

namespace ItConsultations.Business.Services.NoteService.NoteExportService;

public interface INoteExportService
{
    Task<byte[]> ExportNoteToPngAsync(long noteId);

    Task<byte[]> ExportNotesToDocxAsync(IEnumerable<long> noteIds);

    Task<byte[]> ExportNotseToPdfAsync(IEnumerable<long> noteIds);
}
