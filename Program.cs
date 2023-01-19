using System.Drawing;
using System.Drawing.Imaging;
using Wave_Fun;
using Integration;

namespace SSTVWorker;

internal class Program
{
    static void Main(string[] args)
    {
        WaveGenerator wave = new WaveGenerator(WaveType.Test);
        wave.Save("test.wav");

        // int martin1Height = 320;
        // int martin1Width = 256;

        // MemoryStream stream = new MemoryStream();
        // Image imageInput = Image.FromFile("test.png");
        // imageInput.Save(stream, ImageFormat.Jpeg);

        // byte[] resizedImage = Resize(stream.ToArray(), martin1Height, martin1Width);

        // int samples = 1000;
        // double[] doubleArray = new double[samples];
        // string text = "";

        // for(int time = 0; time < samples; time++) {
        //     doubleArray[time] = s(time, 1);
        //     //doubleArray[time] = Integration.Integrators.f(time);
        //     text += (time + "," + doubleArray[time] + "\n");
        // }

        // WriteAllText.WriteText(text).Wait();
    }

    Wave_Fun.WaveFormatChunk format = new Wave_Fun.WaveFormatChunk();

    public double s(float t, float freq, float deviation) {
        double j = ((Math.PI * 2 * freq) / (format.dwSamplesPerSec * format.wChannels));
        return Math.Cos(j * (2 * Math.PI * freq * t) + deviation * (Integration.Integrators.CompositeSimpsonsIntegrate(t - 2, t, 10, 4)));
        //return MathF.Cos(j * (2 * MathF.PI * freq * t) + deviation * ((float)Integration.Integrators.LRAMIntegrate(t - 2, t, 10)));
    }

    // https://www.andrewhoefling.com/Blog/Post/basic-image-manipulation-in-c-sharp
    public static byte[] Resize(byte[] data, int width, int height)
    {
        using (var stream = new MemoryStream(data))
        {
            var image = Image.FromStream(stream);

            //var height = (width * image.Height) / image.Width;
            var thumbnail = image.GetThumbnailImage(width, height, null, IntPtr.Zero);

            using (var thumbnailStream = new MemoryStream())
            {
                thumbnail.Save(thumbnailStream, ImageFormat.Jpeg);
                return thumbnailStream.ToArray();
            }
        }
    }
}

class WriteAllText
{
    public static async Task WriteText(string text)
    {
        await File.WriteAllTextAsync("WriteText.txt", text);
    }
}

// Console.WriteLine("Hello World!");

// int martin1Height = 320;
// int martin1Width = 256;

// MemoryStream stream = new MemoryStream();
// Image imageInput = Image.FromFile("test.png");
// imageInput.Save(stream, ImageFormat.Jpeg);

// // MemoryStream resizedImage = new MemoryStream(Resize(stream.ToArray(), martin1Height, martin1Width));
// // Image outImage = Image.FromStream(resizedImage);
// // outImage.Save("out.png");

// byte[] resizedImage = Resize(stream.ToArray(), martin1Height, martin1Width);

// WaveGenerator wave = new WaveGenerator(WaveType.SSTV);
// wave.Save("out.wav");