namespace Moody;

static partial class CHIP8
{
    private partial class Core
    {
        private void OP_00E0()
        {
            //clears the display
            for(int i = 0; i < _display.Length; i++)
                _display[i] = 0;
        }

        private void OP_00EE()
        {
            //returns
            _programCounter = _stack.Pop();
        }

        private void OP_1NNN()
        {
            //jumps to location nnn
            _programCounter = (ushort)(_opCode & 0x0FFF);
        }

        private void OP_2NNN()
        {
            //calls subroutine at nnn
            _stack.Push(_programCounter);
            _programCounter = (ushort)(_opCode & 0x0FFF);
        }

        private void OP_3XKK()
        {
            //skips next instruction if register Vx == kk
            var Vx = (_opCode & 0x0F00) >> 8;
            byte kk = (byte)(_opCode & 0x00FF);

            if(_registers[Vx] == kk) _programCounter += 2;
        }

        private void OP_4XKK()
        {
            var Vx = (_opCode & 0x0F00) >> 8;
            byte kk = (byte)(_opCode & 0x00FF);

            if(_registers[Vx] != kk) _programCounter += 2;
        }

        private void OP_5XY0()
        {
            var Vx = (_opCode & 0x0F00) >> 8;
            var Vy = (_opCode & 0x00F0) >> 4;

            if(_registers[Vx] == _registers[Vy]) _programCounter += 2;
        }

        private void OP_6XKK()
        {
            var Vx = (_opCode & 0x0F00) >> 8;
            _registers[Vx] = (byte)(_opCode & 0x00FF);
        }

        private void OP_7XKK()
        {
            var Vx = (_opCode & 0x0F00) >> 8;
            _registers[Vx] += (byte)(_opCode & 0x00FF);
        }

        private void OP_8XY0()
        {
            var Vx = (_opCode & 0x0F00) >> 8;
            var Vy = (_opCode & 0x00F0) >> 4;
            _registers[Vx] = _registers[Vy];
        }

        private void OP_8XY1()
        {
            var Vx = (_opCode & 0x0F00) >> 8;
            var Vy = (_opCode & 0x00F0) >> 4;

            _registers[Vx] |= _registers[Vy];
        }

        private void OP_8XY2()
        {
            var Vx = (_opCode & 0x0F00) >> 8;
            var Vy = (_opCode & 0x00F0) >> 4;

            _registers[Vx] &= _registers[Vy];
        }

        private void OP_8XY3()
        {
            var Vx = (_opCode & 0x0F00) >> 8;
            var Vy = (_opCode & 0x00F0) >> 4;

            _registers[Vx] ^= _registers[Vy];
        }

        private void OP_8XY4()
        {
            var Vx = (_opCode & 0x0F00) >> 8;
            var Vy = (_opCode & 0x00F0) >> 4;

            var sum = _registers[Vx] + _registers[Vy];

            if(sum > 255) _registers[0xF] = 1;
            else _registers[0xF] = 0;

            _registers[Vx] = (byte)(sum & 0xFF);
        }

        private void OP_8XY5()
        {
            var Vx = (_opCode & 0x0F00) >> 8;
            var Vy = (_opCode & 0x00F0) >> 4;

            if(_registers[Vx] > _registers[Vy]) _registers[0xF] = 1;
            else _registers[0xF] = 0;

            _registers[Vx] -= _registers[Vy];
        }

        private void OP_8XY6()
        {
            var Vx = (_opCode & 0x0F00) >> 8;

            _registers[0xF]= (byte)(_registers[Vx] & 0x01); 
            
            _registers[Vx] >>= 1;
        }

        private void OP_8XY7()
        {
            var Vx = (_opCode & 0x0F00) >> 8;
            var Vy = (_opCode & 0x00F0) >> 4;

            if(_registers[Vy] > _registers[Vx]) _registers[0xF] = 1;
            else _registers[0xF] = 0;

            _registers[Vx] = (byte)(_registers[Vy] - _registers[Vx]);
        }

        private void OP_8XYE()
        {
            var Vx = (_opCode & 0x0F00) >> 8;

            _registers[0xF]= (byte)((_registers[Vx] & 0x80) >> 7); 
            
            _registers[Vx] <<= 1;
        } 

        private void OP_9XY0()
        {
            var Vx = (_opCode & 0x0F00) >> 8;
            var Vy = (_opCode & 0x00F0) >> 4;

            if(_registers[Vx] != _registers[Vy]) _programCounter += 2;
        }

        private void OP_ANNN()
        {
            ushort address = (ushort)(_opCode & 0x0FFF);
            _indexRegister = address;
        }

        private void OP_BNNN()
        {
            ushort address = (ushort)(_opCode & 0x0FFF);
            _programCounter = (ushort)(_registers[0x0] + address);
        }

        private void OP_CXKK()
        {
            var Vx = (_opCode & 0x0F00) >> 8;
            var kk = (byte)(_opCode & 0x00FF);

            _registers[Vx] = (byte)(GetRandomByte() & kk);
        }

        private void OP_DXYN()
        {
            var xPos = _registers[(_opCode & 0x0F00) >> 8];
            var yPos = _registers[(_opCode & 0x00F0) >> 4];

            byte height = (byte)(_opCode & 0x000F);

            _registers[0xF] = 0;

            for(uint row = 0; row < height; row++)
            {
                var spriteByte = _memory[_indexRegister + row];

                for(byte col = 0; col < 8; col++)
                {
                    byte curSpritePixel = (byte)(spriteByte & (0x80 >> col));
                    ref uint curScreenPixel = ref _display[((yPos + row) * VIDEO_WIDTH) + (xPos + col)];

                    if(curSpritePixel == 1)
                    {
                        if(curScreenPixel == 0xFFFFFFFF) _registers[0xF] = 1;

                        curScreenPixel ^= 0xFFFFFFFF;
                    }
                }
            }
        }

        private void OP_EX9E()
        {
            var key = _registers[(_opCode & 0x0F00) >> 8];

            if(_keypad[key] != 0) _programCounter += 2;
        }

        private void OP_EXA1()
        {
            var key = _registers[(_opCode & 0x0F00) >> 8];

            if(_keypad[key] == 0) _programCounter += 2;
        }

        private void OP_FX07()
        {
            var Vx = (_opCode & 0x0F00) >> 8;

            _registers[Vx] = _delayTimer;
        }

        private void OP_FX0A()
        {
            var Vx = (_opCode & 0x0F00) >> 8;

            for(byte i = 0; i < _keypad.Length; i++)
            {
                if(_keypad[i] != 0)
                {
                    _registers[Vx] = i;
                    return;
                }
            }

            _programCounter -= 2;
        }

        private void OP_FX15()
        {
            var Vx = (_opCode & 0x0F00) >> 8;

            _delayTimer = _registers[Vx];
        }

        private void OP_FX18()
        {
            var Vx = (_opCode & 0x0F00) >> 8;
            _soundTimer = _registers[Vx];
        }

        private void OP_FX1E()
        {
            var Vx = (_opCode & 0x0F00) >> 8;

            _indexRegister += _registers[Vx];
        }

        private void OP_FX29()
        {
            var digit = _registers[(_opCode & 0x0F00) >> 8];
            _indexRegister = (ushort)(0x50 + (5 * digit));
        }

        private void OP_FX33()
        {
            var value = _registers[(_opCode & 0x0F00) >> 8];

            _memory[_indexRegister + 2] = (byte)(value % 10);
            value /= 10;

            _memory[_indexRegister + 1] = (byte)(value % 10);
            value /= 10;

            _memory[_indexRegister] = (byte)(value % 10);
        }

        private void OP_FX55()
        {
            var Vx = (_opCode & 0x0F00) >> 8;

            for(byte i = 0; i <= Vx; i++)
                _memory[_indexRegister + i] = _registers[i];
        }

        private void OP_FX65()
        {
            var Vx = (_opCode & 0x0F00) >> 8;

            for(byte i = 0; i <= Vx; i++)
                _registers[i] = _memory[_indexRegister + i];
        }
    }
}