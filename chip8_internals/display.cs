namespace Moody;

partial class CHIP8
{
    private class Display
    {
        private uint[] _display;
        private int _scale;
        private int _row;
        private int _col;
        public Display(int scale)
        {
            _col = 64;
            _row = 32;
            _scale = scale;

            _display = new uint[_col * _row];
        }

        public void SetPixel(int x, int y)
        {

        }
    }
}