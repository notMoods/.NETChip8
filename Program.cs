using Moody;

var system = CHIP8.NewSystem();

system.Initialize(5);

system.LoadGame("foo.rom");

while(true)
{
    system.EmulateCycle();

    if(system.DrawFlag)
    {

    }

    system.SetKeys();
}
