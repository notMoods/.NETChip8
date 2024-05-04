using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using static Moody.CHIP8;

namespace Moody;

class Window : GameWindow
{
    private CHIP8System _chip8;
    public Window(int width, int height, string title, CHIP8System? chip8 = default) : base(GameWindowSettings.Default, new NativeWindowSettings(){ClientSize = (width, height), Title = title})
    {
        _chip8 = chip8 ?? new CHIP8System();
        FileDrop += On_FileDrop;
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
    }

    private void On_FileDrop(FileDropEventArgs args)
    {
        _chip8.LoadGame(args.FileNames[0]);
    }

    protected override void OnKeyDown(KeyboardKeyEventArgs e)
    {
        switch (e.Key)
        {
            case Keys.D1:
                KeySetter(1);
                break;
            case Keys.D2:
                KeySetter(2);
                break;
            case Keys.D3:
                KeySetter(3);
                break;
            case Keys.D4:
                KeySetter(0xC);
                break;
            case Keys.Q:
                KeySetter(4);
                break;
            case Keys.W:
                KeySetter(5);
                break;
            case Keys.E:
                KeySetter(6);
                break;
            case Keys.R:
                KeySetter(0xD);
                break;
            case Keys.A:
                KeySetter(7);
                break;
            case Keys.S:
                KeySetter(8);
                break;
            case Keys.D:
                KeySetter(9);
                break;
            case Keys.F:
                KeySetter(0xE);
                break;
            case Keys.Z:
                KeySetter(0xA);
                break;
            case Keys.X:
                KeySetter(0);
                break;
            case Keys.C:
                KeySetter(0xB);
                break;
            case Keys.V:
                KeySetter(0xF);
                break;
        }
        
    }

    private void KeySetter(int num)
    {
        if (num < 0 || num >= _chip8.Keys.Length)
            return;
        _chip8.Keys[num] = 0xFF;
    }
}