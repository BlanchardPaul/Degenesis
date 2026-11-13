using SkiaSharp;

namespace Business;

public static class ImageProcessing
{
    public static byte[]? ResizeCover(byte[] sourceBytes, int width, int height, out string? error)
    {
        error = null;
        try
        {
            using var original = SKBitmap.Decode(sourceBytes);
            if (original is null)
            {
                error = "Invalid image file";
                return null;
            }

            using var resized = new SKBitmap(width, height);
            using var canvas = new SKCanvas(resized);

            float scale = Math.Max((float)width / original.Width, (float)height / original.Height);
            float w = original.Width * scale;
            float h = original.Height * scale;
            var dest = new SKRect((width - w) / 2, (height - h) / 2, (width + w) / 2, (height + h) / 2);

            canvas.Clear(SKColors.White);
            canvas.DrawBitmap(original, dest, new SKSamplingOptions(SKCubicResampler.Mitchell));
            canvas.Flush();

            using var image = SKImage.FromBitmap(resized);
            using var data = image.Encode(SKEncodedImageFormat.Jpeg, 90);
            return data.ToArray();
        }
        catch
        {
            error = "Invalid image file";
            return null;
        }
    }
}