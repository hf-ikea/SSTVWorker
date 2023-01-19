using System.Drawing;
using System.Drawing.Imaging;
using Wave_Fun;
using Integration;

namespace SSTVWorker;

internal class Program
{
    public static Bitmap bitmap;
    public static float[] brightnessArray;
    public static int bitmapWidth;
    public static int bitmapHeight;
    static void Main(string[] args)
    {
        getBitmap();
        Console.WriteLine("Got Bitmap");
        getLightArray();
        Console.WriteLine("Got Light Array\nBeginning calcs...");

        WaveGenerator wave = new WaveGenerator(WaveType.Test);
        wave.Save("test.wav");

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

    public static double f(float t, Bitmap bitmap)
    {
        return 1;
        // if(t<0) {
        //     return 0;
        // }

        // int xy = (int)Math.Floor(t);
        
        // try {
        //     return brightnessArray[xy];
        // } catch(IndexOutOfRangeException e) {
        //     Console.WriteLine(xy);
        //     return 0;
        // }
        
    }

    // https://www.andrewhoefling.com/Blog/Post/basic-image-manipulation-in-c-sharp
    public static Image Resize(byte[] data, int width, int height)
    {
        using (var stream = new MemoryStream(data))
        {
            var image = Image.FromStream(stream);

            //var height = (width * image.Height) / image.Width;
            var thumbnail = image.GetThumbnailImage(width, height, null, IntPtr.Zero);

            return thumbnail;
        }
    }

    public static void getBitmap()
    {
        int martin1Height = 320;
        int martin1Width = 256;

        MemoryStream stream = new MemoryStream();
        Image imageInput = Image.FromFile("test.png");
        imageInput.Save(stream, ImageFormat.Jpeg);

        bitmap = new Bitmap(Resize(stream.ToArray(), martin1Width, martin1Height));
    }
    
    public static void getLightArray()
    {
        bitmapHeight = bitmap.Size.Height;
        bitmapWidth = bitmap.Size.Width;
        int bitmapSize = bitmap.Size.Width * bitmap.Size.Height;
        brightnessArray = new float[bitmap.Size.Width * bitmap.Size.Height];

        for(int i = 0; i < bitmap.Size.Height;) {
            for(int j = 0; j < bitmap.Size.Width;) {
                brightnessArray[i] = bitmap.GetPixel(j, i).GetBrightness();
                j++;
            }
            i++;
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