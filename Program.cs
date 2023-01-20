using System.Drawing;
using System.Drawing.Imaging;
using Wave_Fun;
using Integration;

namespace SSTVWorker;

internal class Program
{
    public static Bitmap bitmap;
    public static int[] redArray;
    public static int[] greenArray;
    public static int[] blueArray;
    public static int bitmapWidth;
    public static int bitmapHeight;
    static void Main(string[] args)
    {
        getBitmap();
        Console.WriteLine("Got Bitmap");
        getColorArray();
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

    static Wave_Fun.WaveFormatChunk format = new Wave_Fun.WaveFormatChunk();

    // public double s(float t, float freq, float deviation) {
    //     double j = ((Math.PI * 2 * freq) / (format.dwSamplesPerSec * format.wChannels));
    //     return Math.Cos(j * (2 * Math.PI * freq * t) + deviation * (Integration.Integrators.CompositeSimpsonsIntegrate(t - 2, t, 10, 4)));
    //     //return MathF.Cos(j * (2 * MathF.PI * freq * t) + deviation * ((float)Integration.Integrators.LRAMIntegrate(t - 2, t, 10)));
    // }

    public static int f(int x, int y, string color)
    {
        if(x < 0) {
            return 0;
        }

        int xy = x * y;
        switch(color){
            case "r":
                try {
                    return redArray[xy];
                } catch(IndexOutOfRangeException e) {
                    Console.WriteLine(e);
                    return 0;
                }
            case "g":
                try {
                    return greenArray[xy];
                } catch(IndexOutOfRangeException e) {
                    Console.WriteLine(e);
                    return 0;
                }
            case "b":
                try {
                    return blueArray[xy];
                } catch(IndexOutOfRangeException e) {
                    Console.WriteLine(e);
                    return 0;
                }
        }
        return 0;
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
    
    public static void getColorArray()
    {
        bitmapHeight = bitmap.Size.Height;
        bitmapWidth = bitmap.Size.Width;
        int bitmapSize = bitmap.Size.Width * bitmap.Size.Height;

        redArray = new int[bitmap.Size.Width * bitmap.Size.Height];
        greenArray = new int[bitmap.Size.Width * bitmap.Size.Height];
        blueArray = new int[bitmap.Size.Width * bitmap.Size.Height];

        for(int i = 0; i < bitmap.Size.Height;) {
            for(int j = 0; j < bitmap.Size.Width;) {
                Color color = bitmap.GetPixel(j, i);
                redArray[i] = color.R;
                greenArray[i] = color.G;
                blueArray[i] = color.B;
                j++;
            }
            i++;
        }
    }

    public static double[] RGBtoAngle(int red, int green, int blue)
    {
        if(red > 256 || green > 256 || blue > 256)
        {
            return new double[0];
        }
        float redFreq   = red * 3.125f + 1500f;
        float greenFreq = green * 3.125f + 1500f;
        float blueFreq  = blue * 3.125f + 1500f;

        double redAngle   = (Math.PI * 2 * redFreq) / (format.dwSamplesPerSec);  // * format.wChannels);
        double greenAngle = (Math.PI * 2 * greenFreq) / (format.dwSamplesPerSec);  // * format.wChannels);
        double blueAngle  = (Math.PI * 2 * blueFreq) / (format.dwSamplesPerSec);  // * format.wChannels);
        double[] returnVal = new double[3];

        returnVal[0] = redAngle;
        returnVal[1] = greenAngle;
        returnVal[2] = blueAngle;

        return returnVal;
    }

    public static double ByteToAngle(int num)
    {
        if(num > 256)
        {
            return 0;
        }
        float freq = num * 3.125f + 1500f;

        return (Math.PI * 2 * freq) / (format.dwSamplesPerSec);
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