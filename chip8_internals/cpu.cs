namespace Moody;

static partial class CHIP8
{
    private static readonly int VIDEO_WIDTH = 64;
    private static readonly int VIDEO_HEIGHT = 32;
    private partial class Core
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

        public ushort _opCode;
        public byte _delayTimer;
        public byte _soundTimer;

        private readonly Dictionary<byte, Action> _master_table;
        private readonly Dictionary<byte, Action> _table0;
        private readonly Dictionary<byte, Action> _table8;
        private readonly Dictionary<byte, Action> _tableE;
        private readonly Dictionary<byte, Action> _tableF;

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

            for (int i = 0; i < fontset.Length; i++)
                _memory[0x50 + i] = fontset[i];
        }

        private static byte GetRandomByte() => (byte)Random.Shared.Next(255);

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

            _master_table = new Dictionary<byte, Action>
            {
                { 0x0, Table0 },
                { 0x1, OP_1NNN },
                { 0x2, OP_2NNN },
                { 0x3, OP_3XKK },
                { 0x4, OP_4XKK },
                { 0x5, OP_5XY0 },
                { 0x6, OP_6XKK },
                { 0x7, OP_7XKK },
                { 0x8, Table8 },
                { 0x9, OP_9XY0 },
                { 0xA, OP_ANNN },
                { 0xB, OP_BNNN },
                { 0xC, OP_CXKK },
                { 0xD, OP_DXYN },
                { 0xE, TableE },
                { 0xF, TableF }
            };

            _table0 = new Dictionary<byte, Action>()
            {
                { 0x0, OP_00E0 },
                { 0xE, OP_00EE }
            };

            _table8 = new Dictionary<byte, Action>()
            {
                { 0x0, OP_8XY0 },
                { 0x1, OP_8XY1 },
                { 0x2, OP_8XY2 },
                { 0x3, OP_8XY3 },
                { 0x4, OP_8XY4 },
                { 0x5, OP_8XY5 },
                { 0x6, OP_8XY6 },
                { 0x7, OP_8XY7 },
                { 0xE, OP_8XYE }
            };

            _tableE = new Dictionary<byte, Action>()
            {
                {0x1, OP_EXA1},
                {0xE, OP_EX9E},
            };

            _tableF = new Dictionary<byte, Action>()
            {
                { 0x07, OP_FX07 },
                { 0x0A, OP_FX0A },
                { 0x15, OP_FX15 },
                { 0x18, OP_FX18 },
                { 0x1E, OP_FX1E },
                { 0x29, OP_FX29 },
                { 0x33, OP_FX33 },
                { 0x55, OP_FX55 },
                { 0x65, OP_FX65 }
        };
        }

        private void Table0()
        {
            if(_table0.TryGetValue((byte)(_opCode & 0x000F), out var func))
                func();
        }

        private void Table8()
        {
            if(_table8.TryGetValue((byte)(_opCode & 0x000F), out var func))
                func();
        }

        private void TableE()
        {
            if(_tableE.TryGetValue((byte)(_opCode & 0x000F), out var func))
                func();
        }

        private void TableF()
        {
            if(_tableF.TryGetValue((byte)(_opCode & 0x00FF), out var func))
                func();
        }
    }
}