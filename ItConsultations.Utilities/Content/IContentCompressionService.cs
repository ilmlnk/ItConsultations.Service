namespace ItConsultations.Utilities.Content;

public interface IContentCompressionService
{
    Task<Stream> CompressImageAsync(Stream imageStream, int width, int height);
    
    Task<Stream> CompressImageAsync(Stream imageStream, int quality);
    
    Task<Stream> CompressVideoAsync(Stream videoStream, int bitrate);
    
    Task<Stream> CompressVideoAsync(Stream videoStream, int width, int height);
    
    Task<Stream> CompressAudioAsync(Stream audioStream, int bitrate);
}