using Wave_Fun;

namespace SSTVWorker;

internal class VISGenerate
{
    public class VISReturn
    {
        public short[] data;
        public int i;
    }
    public static VISReturn VISGenerator(uint samples, uint samplesPerSec, uint channels, int amplitude)
    {
        short[] data = new short[samples];
        int i = 0;

        double leaderAngle = (Math.PI * 2 * 1900.0f) / (samplesPerSec * channels);
        double twelveHundredAngle = (Math.PI * 2 * 1200.0f) / (samplesPerSec * channels);
        double oneAngle = (Math.PI * 2 * 1100.0f) / (samplesPerSec * channels);
        double zeroAngle = (Math.PI * 2 * 1300.0f) / (samplesPerSec * channels);

        // for leader
        for (uint k = 0; k < (0.3 * samplesPerSec * channels) - 1; k++)
        {
            // Fill with a simple sine wave at max amplitude
            for (int channel = 0; channel < channels; channel++)
            {
                data[i + channel] = Convert.ToInt16(amplitude * Math.Sin(leaderAngle * k));
                i++;
            }
        }

        // for break
        for (uint k = 0; k < (0.03 * samplesPerSec * channels) - 1; k++)
        {
            // Fill with a simple sine wave at max amplitude
            for (int channel = 0; channel < channels; channel++)
            {
                data[i + channel] = Convert.ToInt16(amplitude * Math.Sin(twelveHundredAngle * k));
                i++;
            }
        }

        // for leader
        for (uint k = 0; k < (0.3 * samplesPerSec * channels) - 1; k++)
        {
            // Fill with a simple sine wave at max amplitude
            for (int channel = 0; channel < channels; channel++)
            {
                data[i + channel] = Convert.ToInt16(amplitude * Math.Sin(leaderAngle * k));
                i++;
            }
        }

        // for start
        for (uint k = 0; k < (0.03 * samplesPerSec * channels) - 1; k++)
        {
            // Fill with a simple sine wave at max amplitude
            for (int channel = 0; channel < channels; channel++)
            {
                data[i + channel] = Convert.ToInt16(amplitude * Math.Sin(twelveHundredAngle * k));
                i++;
            }
        }

        int[] visCode = {0, 1, 0, 1, 1, 0, 0};

        foreach(int bit in visCode) {
            if (bit == 0) {
                for (uint k = 0; k < (0.03 * samplesPerSec * channels) - 1; k++)
                {
                    // Fill with a simple sine wave at max amplitude
                    for (int channel = 0; channel < channels; channel++)
                    {
                        data[i + channel] = Convert.ToInt16(amplitude * Math.Sin(zeroAngle * k));
                        i++;
                    }
                }
            } else {
                for (uint k = 0; k < (0.03 * samplesPerSec * channels) - 1; k++)
                {
                    // Fill with a simple sine wave at max amplitude
                    for (int channel = 0; channel < channels; channel++)
                    {
                        data[i + channel] = Convert.ToInt16(amplitude * Math.Sin(oneAngle * k));
                        i++;
                    }
                }
            }
        }

        // for stop
        for (uint k = 0; k < (0.03 * samplesPerSec * channels) - 1; k++)
        {
            // Fill with a simple sine wave at max amplitude
            for (int channel = 0; channel < channels; channel++)
            {
                data[i + channel] = Convert.ToInt16(amplitude * Math.Sin(twelveHundredAngle * k));
                i++;
            }
        }

        VISReturn returnValue = new VISReturn();
        returnValue.data = data;
        returnValue.i = i;

        return returnValue;
    }
}