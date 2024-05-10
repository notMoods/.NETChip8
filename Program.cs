using System.Diagnostics;
using Moody;

internal class Program
{
    private static void Main(string[] args)
    {
        var window = new CHIP8Window(10);
        window.SetFramerateLimit(60);


        var lastCycleTime = Stopwatch.StartNew();
        float cycleDelay = 16.0f;
        
        while (window.IsOpen)
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
                    //do beep sound
                }

                window.UpdateDisplay(); // Update display with emulator's output
            }
        }

        window.Dispose();
    }
}