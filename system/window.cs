using SFML.Graphics;
using SFML.Window;
using static Moody.CHIP8;

namespace Moody;

class CHIP8Window : RenderWindow
{
    private readonly CHIP8System _chip8;
    private readonly int _scale;

    private bool isGameLoaded;

    public bool STimerAboveZero => _chip8.STimerAboveZero;
    public CHIP8Window(int scale) : base(new VideoMode((uint)(scale * VIDEO_WIDTH), (uint)(scale * VIDEO_HEIGHT)), "Chippy-8")
    {
        _scale = scale;

        _chip8 = new CHIP8System();
        isGameLoaded = false;
    }

    public void MainGameCycle()
    {
        if (!isGameLoaded) return;
        _chip8.Cycle();
    }

    public void UpdateDisplay()
    {
        if (!isGameLoaded) return;
        Clear();
        var disp_buffer = _chip8.Display;

        for (int i = 0; i < disp_buffer.Length; i++)
        {
            if (disp_buffer[i] != 0)
            {
                var (yCord, xCord) = (i / VIDEO_WIDTH, i - (i / VIDEO_WIDTH * VIDEO_WIDTH));

                var (scaledY, scaledX) = (yCord * _scale, xCord * _scale);

                RectangleShape pixel = new(new SFML.System.Vector2f(_scale, _scale))
                {
                    Position = new(scaledX, scaledY)
                };

                Draw(pixel);
            }
        }
        Display();
    }

    public void ProcessKeyPresses()
    {
        static bool TryGetGamePath(out string path)
        {
            var foo = Clipboard.Contents;

            if (foo[^3..] == "ch8")
            {
                path = foo;
                return true;
            }

            path = "";
            return false;
        }


        if (PollEvent(out Event _event))
        {
            switch (_event.Type)
            {
                case EventType.Closed:
                    Close();
                    break;
                case EventType.MouseButtonReleased:
                    if (_event.MouseButton.Button == Mouse.Button.Left)
                    {
                        if (_event.MouseButton.X >= 0 && _event.MouseButton.X < Size.X
                        && _event.MouseButton.Y >= 0 && _event.MouseButton.Y < Size.Y)
                        {
                            if (TryGetGamePath(out var path))
                            {
                                _chip8.LoadGame(path);
                                isGameLoaded = true;
                            }
                        }
                    }
                    break;
                case EventType.KeyPressed:
                    switch (_event.Key.Code)
                    {
                        case Keyboard.Key.Escape:
                            Close();
                            break;
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
                    break;
                case EventType.KeyReleased:
                    switch (_event.Key.Code)
                    {
                        case Keyboard.Key.Num1:
                            KeySetter(1, false);
                            break;
                        case Keyboard.Key.Num2:
                            KeySetter(2, false);
                            break;
                        case Keyboard.Key.Num3:
                            KeySetter(3, false);
                            break;
                        case Keyboard.Key.Num4:
                            KeySetter(0xC, false);
                            break;
                        case Keyboard.Key.Q:
                            KeySetter(4, false);
                            break;
                        case Keyboard.Key.W:
                            KeySetter(5, false);
                            break;
                        case Keyboard.Key.E:
                            KeySetter(6, false);
                            break;
                        case Keyboard.Key.R:
                            KeySetter(0xD, false);
                            break;
                        case Keyboard.Key.A:
                            KeySetter(7, false);
                            break;
                        case Keyboard.Key.S:
                            KeySetter(8, false);
                            break;
                        case Keyboard.Key.D:
                            KeySetter(9, false);
                            break;
                        case Keyboard.Key.F:
                            KeySetter(0xE, false);
                            break;
                        case Keyboard.Key.Z:
                            KeySetter(0xA, false);
                            break;
                        case Keyboard.Key.X:
                            KeySetter(0, false);
                            break;
                        case Keyboard.Key.C:
                            KeySetter(0xB, false);
                            break;
                        case Keyboard.Key.V:
                            KeySetter(0xF, false);
                            break;
                    }
                    break;
            }
        }
    }

    private void KeySetter(int num, bool withValue = true)
    {
        if (num < 0 || num >= _chip8.Keys.Length)
            return;
        _chip8.Keys[num] = (byte)(withValue ? 0xFF : 0);
    }
}