namespace CHIP8;


class Core
{
    private byte[] _memory;
    private byte[] _varRegisters;

    private int _programCounter;

    public Core()
    {
        _memory = new byte[4096];
        _varRegisters = new byte[16];
        _programCounter = 0;

    }
}