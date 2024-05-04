using System.Diagnostics;
using Moody;


Console.Write("Enter a video scale: ");
if(!int.TryParse(Console.ReadLine(), out var videoScale))
{
    Console.WriteLine("\nFailed to see scale, using default of 4");
    videoScale = 4;
}

Console.Write("Enter a cycle delay: ");
if(!int.TryParse(Console.ReadLine(), out var cycleDelay))
{
    Console.WriteLine("\nFailed to see scale, using default of 2");
    cycleDelay = 2;
}

using(var window = new Window(64 * videoScale, 32 * videoScale, "CHIP-8 Emulator"))
{
    window.Run();

}



