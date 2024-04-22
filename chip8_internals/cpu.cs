namespace Moody;

partial class CHIP8
{
    private class Core
    {
        public byte[] _memory;
        public byte[] _registers;
        public byte[] _keypad;
        public uint[] _display;
        private readonly int _row = 32;
        private readonly int _col = 64;
        public Stack<ushort> _stack;

        public ushort _programCounter;
        public ushort _indexRegister;
        public byte _delayTimer;
        public byte _soundTimer;

        public void InitializeMemory(byte[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
                _memory[0x200 + i] = buffer[i];

            byte[] fontset = [
                0xF0, 0x90, 0x90, 0x90, 0xF0, // 0
	            0x20, 0x60, 0x20, 0x20, 0x70, // 1
	            0xF0, 0x10, 0xF0, 0x80, 0xF0, // 2
	            0xF0, 0x10, 0xF0, 0x10, 0xF0, // 3
	            0x90, 0x90, 0xF0, 0x10, 0x10, // 4
            	0xF0, 0x80, 0xF0, 0x10, 0xF0, // 5
             	0xF0, 0x80, 0xF0, 0x90, 0xF0, // 6
    	        0xF0, 0x10, 0x20, 0x40, 0x40, // 7
            	0xF0, 0x90, 0xF0, 0x90, 0xF0, // 8
	            0xF0, 0x90, 0xF0, 0x10, 0xF0, // 9
	            0xF0, 0x90, 0xF0, 0x90, 0x90, // A
            	0xE0, 0x90, 0xE0, 0x90, 0xE0, // B
            	0xF0, 0x80, 0x80, 0x80, 0xF0, // C
	            0xE0, 0x90, 0x90, 0x90, 0xE0, // D
	            0xF0, 0x80, 0xF0, 0x80, 0xF0, // E
	            0xF0, 0x80, 0xF0, 0x80, 0x80  // F
            ];

            for(int i = 0; i < fontset.Length; i++)
                _memory[0x50 + i] = fontset[i];
        }

        private static byte GetRandomByte => (byte)Random.Shared.Next(255);

        public Core()
        {
            //0x050(80) - 0x0A0(160) for built-in characters
            //0x200(512) - 0xFFF(4095) for instructions stored by the rom
            _memory = new byte[4096];

            _registers = new byte[16];

            _keypad = new byte[16];

            _display = new uint[_col * _row];

            _programCounter = 0x200;
            _indexRegister = 0x000;

            _stack = new Stack<ushort>(16);
        }
    }
}