namespace Moody;

static public partial class CHIP8
{
    public static readonly int VIDEO_WIDTH = 64;
    public static readonly int VIDEO_HEIGHT = 32;
    private partial class Core
    {
        private byte[] _memory;
        private byte[] _registers;
        public byte[] _keypad;
        public ushort[] _display;
        private readonly int _row = 32;
        private readonly int _col = 64;
        private Stack<ushort> _stack;
        private ushort _programCounter;
        private ushort _indexRegister;
        private ushort _opCode;
        public byte _delayTimer;
        public byte _soundTimer;

        private readonly Action[] _master_table;
        private readonly Dictionary<byte, Action> _table0;
        private readonly Dictionary<byte, Action> _table8;
        private readonly Dictionary<byte, Action> _tableE;
        private readonly Dictionary<byte, Action> _tableF;

        public Core()
        {
            //0x050(80) - 0x0A0(160) for built-in characters
            //0x200(512) - 0xFFF(4095) for instructions stored by the rom
            _memory = new byte[4096];

            _registers = new byte[16];

            _keypad = new byte[16];

            _display = new ushort[_col * _row];

            _programCounter = 0x200;
            _indexRegister = 0x000;

            _stack = new Stack<ushort>(16);

            _master_table =
            [
                Table0, OP_1NNN, OP_2NNN, OP_3XKK,
                OP_4XKK, OP_5XY0, OP_6XKK, OP_7XKK,
                Table8, OP_9XY0, OP_ANNN, OP_BNNN,
                OP_CXKK, OP_DXYN, TableE, TableF 
            ];

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

        private static byte GetRandomByte() => (byte)Random.Shared.Next(255);

        public void InitializeMemory(Span<byte> buffer)
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

        private void Table0()
        {
            if (_table0.TryGetValue((byte)(_opCode & 0x000F), out var func))
                func();
        }

        private void Table8()
        {
            if (_table8.TryGetValue((byte)(_opCode & 0x000F), out var func))
                func();
        }

        private void TableE()
        {
            if (_tableE.TryGetValue((byte)(_opCode & 0x000F), out var func))
                func();
        }

        private void TableF()
        {
            if (_tableF.TryGetValue((byte)(_opCode & 0x00FF), out var func))
                func();
        }

        private void UpdateTimers()
        {
            //Decrement delay timer if it has been set
            if(_delayTimer > 0) --_delayTimer;

            //Decrement sound timer if it has been set
            if(_soundTimer > 0) --_soundTimer;
        }

        public void Cycle()
        {
            //Fetch the opCode
            _opCode = (ushort)((_memory[_programCounter] << 8) | _memory[_programCounter + 1]);

            //Increment the counter before we execute anything
            _programCounter += 2;

            //Decode and execute
            _master_table[(_opCode & 0xF000) >> 12]();


            UpdateTimers();
        }
    }
}