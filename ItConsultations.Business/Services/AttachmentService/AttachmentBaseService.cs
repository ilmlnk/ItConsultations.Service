using ItConsultations.Business.AutoMapperConfiguration;
using ItConsultations.Business.DataAccess.Interfaces;
using ItConsultations.Business.Dtos.AttachmentDtos;
using ItConsultations.Business.Dtos.FileDtos;
using ItConsultations.Business.Entities.Attachments;
using ItConsultations.Business.Services.FileService;
using ItConsultations.Utilities.Content;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ItConsultations.Business.Services.AttachmentService;

public abstract class AttachmentBaseService {
    protected readonly IFileService _fileService;
    protected readonly IContentCompressionService _contentCompressionService;
    protected readonly IRepository<AttachmentBase, long> _repository;
    
    private const int ThumbnailWidth = 150;

    public AttachmentBaseService(
        IFileService fileService, 
        IContentCompressionService contentCompressionService,
        IRepository<AttachmentBase, long> repository)
    {
        _fileService = fileService;
        _contentCompressionService = contentCompressionService;
        _repository = repository;
    }

    protected async Task<AttachmentDto> UploadAsync(FileDto dto)
    {
        /*var file = await _fileService.CreateAsync(dto);
        var thumbnailId = await CreateThumbnailAsync(file);

        var attachment = MapperManager.Map<Attachment>(file);
        attachment.ThumbnailId = thumbnailId;

        var createdAttachment = await _repository.CreateAsync(attachment);

        return MapperManager.Map<AttachmentDto>(createdAttachment);*/
        return null;
    }

    protected async Task<AttachmentDto> UpdateAsync(AttachmentDto dto) 
    {
        /*var attachment = await _repository.GetAsync(dto.Id);

        if (dto.File == null)
        {
            return MapperManager.Map<AttachmentDto>(attachment);
        }
        
        dto.File.Id = attachment.Id;
        var file = await _fileService.UpdateAsync(dto.File);
        
        var thumbnailId = await CreateThumbnailAsync(file);
        
        if (attachment.ThumbnailId.HasValue && !thumbnailId.HasValue)
        {
            await _fileService.DeleteAsync(attachment.ThumbnailId.Value);
        }

        attachment.ThumbnailId = thumbnailId;
        await _repository.UpdateAsync(attachment);
        

        return MapperManager.Map<AttachmentDto>(attachment);*/
        return null;
    }
    
    private async Task<long?> CreateThumbnailAsync(FileDto originalFileDto)
    {
        /*if (!originalFileDto.ContentType.StartsWith("image/"))
        {
            return null;
        }

        using var image = await Image.LoadAsync(originalFileDto.FilePath);
        
        var newHeight = 0;
        if (image.Width > ThumbnailWidth)
        {
            var options = new ResizeOptions
            {
                Size = new Size(ThumbnailWidth, newHeight),
                Mode = ResizeMode.Max
            };
            image.Mutate(x => x.Resize(options));
        }

        await using var ms = new MemoryStream();
        await image.SaveAsJpegAsync(ms); // Можна обрати інший формат, наприклад, Png
        var thumbnailBytes = ms.ToArray();

        var thumbnailFileDto = new FileDto
        {
            Name = $"thumb_{originalFileDto.Name}",
            Content = thumbnailBytes,
            ContentType = "image/jpeg",
            IsPublic = originalFileDto.IsPublic
        };

        var createdThumbnail = await _fileService.CreateAsync(thumbnailFileDto);
        return createdThumbnail.Id;*/
        return 0;
    }
}