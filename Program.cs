using Moody;

var system = CHIP8.NewSystem();

system.Initialize(5);

system.LoadGame("C:\\Users\\HP PAVILION 14\\Documents\\Docs\\Coding_Stuff\\2024 folder\\c#_chip8\roms\\IBM Logo.ch8");

while(true)
{
    system.EmulateCycle();

    if(system.DrawFlag)
    {

    }

    system.SetKeys();
}
