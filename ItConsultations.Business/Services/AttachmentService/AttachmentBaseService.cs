namespace ItConsultations.Business.Services.AttachmentService;

public abstract class AttachmentBaseService {
    /*protected readonly IFileService _fileService;
    protected readonly IImageReduceService _imageReduceService;
    protected readonly IRepository<AttachmentBase, long> _repository;

    public AttachmentBaseService(
        IFileService fileService, 
        IImageReduceService imageReduceService,
        IRepository<AttachmentBase, long> repository)
    {
        _fileService = fileService;
        _imageReduceService = imageReduceService;
        _repository = repository;
    }

    protected async Task<AttachmentDto> UploadAsync(FileDto dto)
    {
        var file = await _fileService.CreateAsync(dto);
        var thumbnailId = await CreateThumbnailAsync(file);

        var attachment = new Attachment 
        {
            Id = file.Id,
            Name = file.Name,
            FileName = file.FileName,
            CreatedAt = file.CreatedAt,
            ThumbnailId = thumbnailId
        };

        await _repository.CreateAsync(attachment);

        return created;
    }

    protected async Task<AttachmentDto> UpdateAsync(AttachmentDto dto) 
    {
        var attachment = await _repository.GetAsync(dto.Id);
        Guard.NotNull(attachment);

        var file = await _fileService.UpdateAsync(dto.File);
        var thumbnailId = await CreateThumbnailAsync(file);
    }*/
}