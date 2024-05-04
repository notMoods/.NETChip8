namespace Moody;

static public partial class CHIP8
{
    public class CHIP8System
    {
        private readonly Core _core;
        
        public byte[] Keys; 

        public ushort[] Display;

        public void Cycle() => _core.Cycle();

        public CHIP8System()
        {
            _core = new Core();
            Keys = _core._keypad;
            Display = _core._display;
        }

        public void LoadGame(string path)
        {
            using FileStream fs = new(path, FileMode.Open, FileAccess.Read);
            var size = fs.Length;

            var buffer = new byte[size];

            fs.Read(buffer, 0, buffer.Length);

            //initializes memory of core
            _core.InitializeMemory(buffer.AsSpan());
        }
    }
}
