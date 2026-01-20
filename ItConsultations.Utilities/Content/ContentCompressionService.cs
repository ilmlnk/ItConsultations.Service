using System.Diagnostics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace ItConsultations.Utilities.Content;

public class ContentCompressionService : IContentCompressionService
{
    #region Image Compression

    public Task<Stream> CompressImageAsync(Stream imageStream, int width, int height)
    {
        return ProcessImageAsync(imageStream, image => image.Mutate(x => x.Resize(width, height)));
    }

    public Task<Stream> CompressImageAsync(Stream imageStream, int quality)
    {
        var encoder = new JpegEncoder { Quality = quality };
        return ProcessImageAsync(imageStream, image => {}, encoder);
    }

    private async Task<Stream> ProcessImageAsync(Stream imageStream, Action<Image> processAction, JpegEncoder? encoder = null)
    {
        imageStream.Position = 0;
        using var image = await Image.LoadAsync(imageStream);
        
        processAction(image);

        var outputStream = new MemoryStream();
        
        if (encoder != null)
        {
            await image.SaveAsync(outputStream, encoder);
        }
        else
        {
            await image.SaveAsJpegAsync(outputStream);
        }
        
        outputStream.Position = 0;
        return outputStream;
    }

    #endregion

    #region Video and Audio Compression

    public Task<Stream> CompressVideoAsync(Stream videoStream, int bitrate)
    {
        var ffmpegArgs = $"-i pipe:0 -b:v {bitrate}k -f matroska pipe:1";
        return RunFFmpegCompressionAsync(videoStream, ffmpegArgs);
    }

    public Task<Stream> CompressVideoAsync(Stream videoStream, int width, int height)
    {
        var ffmpegArgs = $"-i pipe:0 -vf scale={width}:{height} -f matroska pipe:1";
        return RunFFmpegCompressionAsync(videoStream, ffmpegArgs);
    }

    public Task<Stream> CompressAudioAsync(Stream audioStream, int bitrate)
    {
        var ffmpegArgs = $"-i pipe:0 -b:a {bitrate}k -f mp3 pipe:1";
        return RunFFmpegCompressionAsync(audioStream, ffmpegArgs);
    }

    private async Task<Stream> RunFFmpegCompressionAsync(Stream inputStream, string ffmpegArgs)
    {
        var process = new Process
        {
            StartInfo =
            {
                FileName = "ffmpeg",
                Arguments = ffmpegArgs,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true 
            }
        };

        process.Start();
        
        var inputTask = inputStream.CopyToAsync(process.StandardInput.BaseStream);
        process.StandardInput.Close();
        await inputTask;

        var outputStream = new MemoryStream();
        await process.StandardOutput.BaseStream.CopyToAsync(outputStream);

        var errors = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException($"FFmpeg failed with exit code {process.ExitCode}: {errors}");
        }

        outputStream.Position = 0;
        return outputStream;
    }

    #endregion
}
