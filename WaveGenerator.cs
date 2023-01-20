using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSTVWorker;
using Integration;

namespace Wave_Fun
{
    public enum WaveType
    {
        ExampleSineWave = 0,
        SSTV = 1,
        Test = 2
    }
    public class WaveGenerator
    {
        // Header, Format, Data chunks
        WaveHeader header;
        WaveFormatChunk format;
        WaveDataChunk data;

        /// <snip>
        public WaveGenerator(WaveType type)
        {
            // Init chunks
            header = new WaveHeader();
            format = new WaveFormatChunk();
            data = new WaveDataChunk();

            int amplitude = 32760; // Max amplitude for 16-bit audio

            // Fill the data array with sample data
            switch (type)
            {
                case WaveType.ExampleSineWave:

                    // Number of samples = sample rate * channels * bytes per sample
                    uint numSamples = format.dwSamplesPerSec * format.wChannels;

                    // Initialize the 16-bit array
                    data.shortArray = new short[numSamples];

                    double freq = 440.0f;   // Concert A: 440Hz

                    // The "angle" used in the function, adjusted for the number of channels and sample rate.
                    // This value is like the period of the wave.
                    double t = (Math.PI * 2 * freq) / (format.dwSamplesPerSec * format.wChannels);

                    for (uint j = 0; j < numSamples - 1; j++)
                    {
                        // Fill with a simple sine wave at max amplitude
                        for (int channel = 0; channel < format.wChannels; channel++)
                        {
                            data.shortArray[j + channel] = Convert.ToInt16(amplitude * Math.Sin(t * j));
                        }
                    }

                    // Calculate data chunk size in bytes
                    data.dwChunkSize = (uint)(data.shortArray.Length * (format.wBitsPerSample / 8));

                    break;
                
                case WaveType.SSTV:
                    // uint sstvSamples = format.dwSamplesPerSec * 150 * format.wChannels; // seconds

                    // data.shortArray = new short[sstvSamples];

                    // VISGenerate.VISReturn returnValue = VISGenerate.VISGenerator(sstvSamples, format.dwSamplesPerSec, format.wChannels, amplitude);

                    // data.shortArray = returnValue.data;
                    // int i = returnValue.i;

                    // double twelveHundredAngle = (Math.PI * 2 * 1200.0f) / (format.dwSamplesPerSec * format.wChannels);
                    // double separatorAngle = (Math.PI * 2 * 1500.0f) / (format.dwSamplesPerSec * format.wChannels);
                    // double sampleAngle = (Math.PI * 2 * 1600.0f) / (format.dwSamplesPerSec * format.wChannels);

                    // // end VIS code

                    // // Calculate data chunk size in bytes
                    // data.dwChunkSize = (uint)(data.shortArray.Length * (format.wBitsPerSample / 8));
                    
                    break;
                case WaveType.Test:
                    uint samples = format.dwSamplesPerSec * format.wChannels * 120; // 228456448

                    data.shortArray = new short[samples];
                    //float deviation = 50;
                    int pixelPerLine = SSTVWorker.Program.bitmapWidth;
                    int lines = 256; //SSTVWorker.Program.bitmapHeight;

                    Program p = new Program();
                    double[] angles = SSTVWorker.Program.RGBtoAngle(204, 136, 153);

                    double twelveHundredAngle = (Math.PI * 2 * 1200.0f) / (format.dwSamplesPerSec);  // * format.wChannels);
                    double separatorAngle     = (Math.PI * 2 * 1500.0f) / (format.dwSamplesPerSec);  // * format.wChannels);
                    double redAngle           = angles[0];
                    double greenAngle         = angles[1];
                    double blueAngle          = angles[2];

                    // VIS Code
                    data.shortArray = new short[samples];

                    VISGenerate.VISReturn returnValue = VISGenerate.VISGenerator(samples, format.dwSamplesPerSec, format.wChannels, amplitude);

                    data.shortArray = returnValue.data;
                    int i = returnValue.i;
                    // End VIS Code

                    for (int j = 0; j < lines - 1; j++){
                        Console.WriteLine("i: " + i);
                        // sync pulse
                        for (int k = 0; k < (0.004862 * format.dwSamplesPerSec); k++)
                        {
                            short stuff = Convert.ToInt16(amplitude * Math.Sin(twelveHundredAngle * k));
                            // Fill with a simple sine wave at max amplitude
                            for (int channel = 0; channel < format.wChannels; channel++)
                            {
                                data.shortArray[i + channel] = stuff;
                                i++;
                            }
                        }
                        
                        // separator pulse
                        for (uint k = 0; k < (0.000572 * format.dwSamplesPerSec); k++)
                        {
                            // Fill with a simple sine wave at max amplitude
                            for (int channel = 0; channel < format.wChannels; channel++)
                            {
                                data.shortArray[i + channel] = Convert.ToInt16(amplitude * Math.Sin(separatorAngle * k));
                                i++;
                            }
                        }
                        
                        // green 
                        for(int b = 0; b < 320; b++)
                        {
                            //greenAngle = SSTVWorker.Program.ByteToAngle(SSTVWorker.Program.f(b, j, "g"));
                            for (uint pixSamp = 0; pixSamp < (0.0004576 * format.dwSamplesPerSec); pixSamp++)
                            {
                                // Fill with a simple sine wave at max amplitude
                                for (int channel = 0; channel < format.wChannels; channel++)
                                {
                                    data.shortArray[i + channel] = Convert.ToInt16(amplitude * Math.Sin(greenAngle * pixSamp * b));
                                    i++;
                                }
                            }
                        }
                        
                        // separator pulse
                        for (uint k = 0; k < (0.000572 * format.dwSamplesPerSec); k++)
                        {
                            // Fill with a simple sine wave at max amplitude
                            for (int channel = 0; channel < format.wChannels; channel++)
                            {
                                data.shortArray[i + channel] = Convert.ToInt16(amplitude * Math.Sin(separatorAngle * k));
                                i++;
                            }
                        }

                        // blue 
                        for(int b = 0; b < 320; b++)
                        {
                            //blueAngle = SSTVWorker.Program.ByteToAngle(SSTVWorker.Program.f(b, j, "b"));
                            for (uint pixSamp = 0; pixSamp < (0.0004576 * format.dwSamplesPerSec); pixSamp++)
                            {
                                // Fill with a simple sine wave at max amplitude
                                for (int channel = 0; channel < format.wChannels; channel++)
                                {
                                    data.shortArray[i + channel] = Convert.ToInt16(amplitude * Math.Sin(blueAngle * pixSamp * b));
                                    i++;
                                }
                            }
                        }
                        // separator pulse
                        for (uint k = 0; k < (0.000572 * format.dwSamplesPerSec); k++)
                        {
                            // Fill with a simple sine wave at max amplitude
                            for (int channel = 0; channel < format.wChannels; channel++)
                            {
                                data.shortArray[i + channel] = Convert.ToInt16(amplitude * Math.Sin(separatorAngle * k));
                                i++;
                            }
                        }

                        // red 
                        for(int b = 0; b < 320; b++)
                        {
                            //blueAngle = SSTVWorker.Program.ByteToAngle(SSTVWorker.Program.f(b, j, "r"));
                            for (uint pixSamp = 0; pixSamp < (0.0004576 * format.dwSamplesPerSec); pixSamp++)
                            {
                                // Fill with a simple sine wave at max amplitude
                                for (int channel = 0; channel < format.wChannels; channel++)
                                {
                                    data.shortArray[i + channel] = Convert.ToInt16(amplitude * Math.Sin(blueAngle * pixSamp * b));
                                    i++;
                                }
                            }
                        }
                        // separator pulse
                        for (uint k = 0; k < (0.000572 * format.dwSamplesPerSec); k++)
                        {
                            // Fill with a simple sine wave at max amplitude
                            for (int channel = 0; channel < format.wChannels; channel++)
                            {
                                data.shortArray[i + channel] = Convert.ToInt16(amplitude * Math.Sin(separatorAngle * k));
                                i++;
                            }
                        }
                        i += 892892 - 9724 - 1144 - 292864 - 1144 - 292864 - 1144 - 292864 - 1144;
                        //            9724 - 1144 - 292864 - 1144 - 292864 - 1144 - 292864 - 1144; //892892 samples total
                        
                    }

                    //441.584

                    // for(uint j = 0; j < samples - 1;)
                    // {
                    //     for (int channel = 0; channel < format.wChannels; channel++)
                    //     {
                    //         data.shortArray[j + channel] = Convert.ToInt16(amplitude * p.s(j, 0.26f * deviation, deviation));
                    //     }
                    //     j++;
                    // }

                    data.dwChunkSize = (uint)(data.shortArray.Length * (format.wBitsPerSample / 8));

                    break;

            }
        }
        public void Save(string filePath)
        {
            // Create a file (it always overwrites)
            FileStream fileStream = new FileStream(filePath, FileMode.Create);

            // Use BinaryWriter to write the bytes to the file
            BinaryWriter writer = new BinaryWriter(fileStream);

            // Write the header
            writer.Write(header.sGroupID.ToCharArray());
            writer.Write(header.dwFileLength);
            writer.Write(header.sRiffType.ToCharArray());

            // Write the format chunk
            writer.Write(format.sChunkID.ToCharArray());
            writer.Write(format.dwChunkSize);
            writer.Write(format.wFormatTag);
            writer.Write(format.wChannels);
            writer.Write(format.dwSamplesPerSec);
            writer.Write(format.dwAvgBytesPerSec);
            writer.Write(format.wBlockAlign);
            writer.Write(format.wBitsPerSample);

            // Write the data chunk
            writer.Write(data.sChunkID.ToCharArray());
            writer.Write(data.dwChunkSize);
            foreach (short dataPoint in data.shortArray)
            {
                writer.Write(dataPoint);
            }

            writer.Seek(4, SeekOrigin.Begin);
            uint filesize = (uint)writer.BaseStream.Length;
            writer.Write(filesize - 8);

            // Clean up
            writer.Close();
            fileStream.Close();
        }
    }
}