using SFML.Graphics;
using SFML.Window;
using static Moody.CHIP8;

namespace Moody;

class CHIP8Window : RenderWindow
{
    private readonly CHIP8System _chip8;
    private readonly int _scale;
    public CHIP8Window(int scale) : base(new VideoMode((uint)(scale * VIDEO_WIDTH), (uint)(scale * VIDEO_HEIGHT)), "Chippy-8")
    {
        _scale = scale;
        
        _chip8 = new CHIP8System();
        KeyPressed += On_KeyPressed;
        Closed += On_Closed;
    }

    private void On_Closed(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void On_KeyPressed(object? sender, KeyEventArgs e)
    {
        if (e.Code == Keyboard.Key.Escape) Close();

        switch (e.Code)
        {
            case Keyboard.Key.Num1:
                KeySetter(1);
                break;
            case Keyboard.Key.Num2:
                KeySetter(2);
                break;
            case Keyboard.Key.Num3:
                KeySetter(3);
                break;
            case Keyboard.Key.Num4:
                KeySetter(0xC);
                break;
            case Keyboard.Key.Q:
                KeySetter(4);
                break;
            case Keyboard.Key.W:
                KeySetter(5);
                break;
            case Keyboard.Key.E:
                KeySetter(6);
                break;
            case Keyboard.Key.R:
                KeySetter(0xD);
                break;
            case Keyboard.Key.A:
                KeySetter(7);
                break;
            case Keyboard.Key.S:
                KeySetter(8);
                break;
            case Keyboard.Key.D:
                KeySetter(9);
                break;
            case Keyboard.Key.F:
                KeySetter(0xE);
                break;
            case Keyboard.Key.Z:
                KeySetter(0xA);
                break;
            case Keyboard.Key.X:
                KeySetter(0);
                break;
            case Keyboard.Key.C:
                KeySetter(0xB);
                break;
            case Keyboard.Key.V:
                KeySetter(0xF);
                break;
        }

        void KeySetter(int num)
        {
            if (num < 0 || num >= _chip8.Keys.Length)
                return;
            _chip8.Keys[num] = 0xFF;
        }
    }

    public void MainGameCycle()
    {
        Clear();
        _chip8.Cycle();

        if(_chip8.STimerAboveZero)
        {
            //make windows beep here
        }

        var disp_buffer = _chip8.Display;

        //using disp_buffer, draw the sprites 
        //to screen

        for(int i = 0; i < disp_buffer.Length; i++)
        {
            if(disp_buffer[i] != 0)
            {
                var (yCord, xCord) = (i / VIDEO_WIDTH, i - (i / VIDEO_WIDTH * VIDEO_WIDTH));

                var (scaledY, scaledX) = (yCord * _scale, xCord * _scale);

                //the shape to draw is a rectangle pixel 10 x 10
                //starting from scaledX, scaledY as top left corner
            }
        }

        Display();
    }


}