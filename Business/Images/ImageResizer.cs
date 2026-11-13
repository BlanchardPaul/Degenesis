using Degenesis.Shared.DTOs;
using SkiaSharp;

namespace Business.Images;

public static class ImageResizer
{
    private const long MaxDecodedPixels = 40_000_000;

    public static Result<byte[]> ResizeAndCenterCrop(Stream source, int width, int height, string extension, long maxSizeBytes)
    {
        try
        {
            source.Position = 0;
            using var data = SKData.Create(source);
            using var codec = SKCodec.Create(data);
            if (codec is null)
                return new Result<byte[]> { IsError = true, Error = "Invalid image file" };

            var sourceWidth = codec.Info.Width;
            var sourceHeight = codec.Info.Height;
            if ((long)sourceWidth * sourceHeight > MaxDecodedPixels)
                return new Result<byte[]> { IsError = true, Error = "Image dimensions are too large to process" };

            using var sourceBitmap = SKBitmap.Decode(data);
            if (sourceBitmap is null)
                return new Result<byte[]> { IsError = true, Error = "Invalid image file" };

            var sourceAspectRatio = (double)sourceBitmap.Width / sourceBitmap.Height;
            var targetAspectRatio = (double)width / height;
            SKRect sourceRect;

            if (sourceAspectRatio > targetAspectRatio)
            {
                var croppedWidth = (float)(sourceBitmap.Height * targetAspectRatio);
                sourceRect = new SKRect((sourceBitmap.Width - croppedWidth) / 2, 0,
                    (sourceBitmap.Width + croppedWidth) / 2, sourceBitmap.Height);
            }
            else
            {
                var croppedHeight = (float)(sourceBitmap.Width / targetAspectRatio);
                sourceRect = new SKRect(0, (sourceBitmap.Height - croppedHeight) / 2,
                    sourceBitmap.Width, (sourceBitmap.Height + croppedHeight) / 2);
            }

            var imageInfo = new SKImageInfo(width, height, SKColorType.Bgra8888, SKAlphaType.Premul);
            using var targetBitmap = new SKBitmap(imageInfo);
            using (var canvas = new SKCanvas(targetBitmap))
            {
                canvas.Clear(SKColors.Transparent);
                canvas.DrawBitmap(sourceBitmap, sourceRect, new SKRect(0, 0, width, height),
                    new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.None));
            }

            var format = extension.ToLowerInvariant() switch
            {
                ".jpg" or ".jpeg" => SKEncodedImageFormat.Jpeg,
                ".png" => SKEncodedImageFormat.Png,
                ".webp" => SKEncodedImageFormat.Webp,
                _ => throw new ArgumentException("Unsupported image format", nameof(extension))
            };

            using var resizedImage = SKImage.FromBitmap(targetBitmap);
            using var encodedData = resizedImage.Encode(format, 90);
            if (encodedData is null)
                return new Result<byte[]> { IsError = true, Error = "Unable to resize image" };

            var resizedBytes = encodedData.ToArray();
            if (resizedBytes.Length > maxSizeBytes)
                return new Result<byte[]> { IsError = true, Error = "Resized image is too large (max 2 MB)" };

            return new Result<byte[]> { Value = resizedBytes };
        }
        catch (Exception)
        {
            return new Result<byte[]> { IsError = true, Error = "Invalid image file" };
        }
    }
}
