namespace Moody;

partial class CHIP8
{
    public static ICHIP8System NewSystem()
    {
        throw new NotImplementedException();
    }

    private class CHIP8System : ICHIP8System
    {
        private readonly Core _core = new();
        public bool DrawFlag { get => throw new NotImplementedException(); private set => throw new NotImplementedException(); }

        public void EmulateCycle()
        {
            throw new NotImplementedException();
        }

        public void Initialize(int scale)
        {
            throw new NotImplementedException();
        }

        public void LoadGame(string dir)
        {
            using FileStream fs = new(dir, FileMode.Open, FileAccess.Read);
            var size = fs.Length;

            var buffer = new byte[size];

            fs.Read(buffer, 0, buffer.Length);

            //initializes memory of core
            _core.InitializeMemory(buffer);
        }

        public void SetKeys()
        {
            throw new NotImplementedException();
        }
    }
}

interface ICHIP8System
{
    public void Initialize(int scale);
    public void EmulateCycle();

    public void SetKeys();

    public void LoadGame(string dir);

    bool DrawFlag { get;}
}