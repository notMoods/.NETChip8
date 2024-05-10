using System.Diagnostics;
using Moody;
using SFML.Audio;

internal class Program
{
    private static void Main(string[] args)
    {
        static short[] CreateBeepSound(uint sampleRate, float durInSeconds)
        {
            var numSamples = (int)(sampleRate * durInSeconds);

            var res = new short[numSamples];

            var freq = 440.0f;
            var amplitude = 30000.0f;
            for(int i = 0; i < numSamples; i++)
            {
                var t = (float)i / sampleRate;
                res[i] = (short)(amplitude * Math.Sin(2 * Math.PI * freq * t));
            }

            return res;
        }
        var window = new CHIP8Window(10);
        window.SetFramerateLimit(60);

        var lastCycleTime = Stopwatch.StartNew();
        float cycleDelay = 16.0f;

        short[] samples = CreateBeepSound(44100, 1.0f);
        Sound sound = new(new SoundBuffer(samples, 1, 44100));
        
        while(window.IsOpen)
        {
            window.ProcessKeyPresses();
                
            var currentTime = Stopwatch.GetTimestamp();
            float dt = (float)(currentTime - lastCycleTime.ElapsedTicks) / Stopwatch.Frequency * 1000;
            
            if (dt > cycleDelay)
            {
                lastCycleTime.Restart();

                window.MainGameCycle();

                if(window.STimerAboveZero)
                {
                    sound.Play();
                }

                window.UpdateDisplay(); 
            }
        }

        window.Dispose();
    }
}