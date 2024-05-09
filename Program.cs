using System.Diagnostics;
using System.Timers;
using Moody;
using SFML.Window;


// Console.Write("Enter a video scale: ");
// if(!int.TryParse(Console.ReadLine(), out var videoScale))
// {
//     Console.WriteLine("\nFailed to see scale, using default of 4");
//     videoScale = 4;
// }

// Console.Write("Enter a cycle delay: ");
// if(!int.TryParse(Console.ReadLine(), out var cycleDelay))
// {
//     Console.WriteLine("\nFailed to see scale, using default of 2");
//     cycleDelay = 2;
// }

internal class Program
{
    private static void Main(string[] args)
    {
        var window = new CHIP8Window(10);
        window.SetFramerateLimit(60);


        void OnTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            window.MainGameCycle();
        }


        var timer = new System.Timers.Timer(16);
        timer.Elapsed += OnTimerElapsed;
        timer.AutoReset = true;
        timer.Start();


        while (window.IsOpen)
        {
            window.DispatchEvents();
        }


        timer.Stop();
        timer.Dispose();
    }
}