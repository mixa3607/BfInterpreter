using System;
using System.Diagnostics;
using System.Text;

namespace Bf.Render
{
    public class BfMemoryConsoleRenderer
    {
        /// <summary>
        /// Reverse byte order
        /// </summary>
        public bool ReverseByteOrder { get; set; } = false;

        /// <summary>
        /// Add separator char between words
        /// </summary>
        public bool AddSeparator { get; set; } = true;

        /// <summary>
        /// Bits count for store value. Max 64.
        /// </summary>
        public byte BitsInWord { get; }

        /// <summary>
        /// Count of words in one line
        /// </summary>
        public uint WordsInRow { get; }

        /// <summary>
        /// Rows
        /// </summary>
        public uint Rows { get; }

        /// <summary>
        /// Flag for add additional line with frame compute time (C) and draw to console time (D)
        /// </summary>
        public bool AddFrameTime { get; set; }

        private readonly BfMemoryConsoleRendererOptions _charsOptions;
        private readonly Stopwatch _stopwatch = new Stopwatch();

        private StringBuilder _frame;

        public BfMemoryConsoleRenderer(byte bitsInWord, uint wordsInRow, uint rows)
        {
            BitsInWord = bitsInWord;
            WordsInRow = wordsInRow;
            Rows = rows;
            _charsOptions = new BfMemoryConsoleRendererOptions()
            {
                EmptyChar = ' '
            };
        }

        public void DrawFrame(ulong[] mem, bool twoLineMode = true, ulong wordsOffset = 0)
        {
            _stopwatch.Restart();

            if (Rows * WordsInRow > mem.Length)
            {
                _stopwatch.Stop();
                throw new Exception("Input array smallest then expected");
            }

            _frame = new StringBuilder(BitsInWord * (int) WordsInRow * (int) Rows);
            for (ulong row = 0; row < Rows; row++)
            {
                if (!twoLineMode)
                {
                    DrawLine(mem, wordsOffset + (row * WordsInRow));
                }
                else
                {
                    DrawTwoLine(mem, wordsOffset + (row * WordsInRow));
                    row++;
                }
            }

            var frameText = _frame.ToString();
            var computeTime = _stopwatch.ElapsedMilliseconds;

            Console.SetCursorPosition(0, 0); //
            Console.Write(frameText); //

            var drawTime = _stopwatch.ElapsedMilliseconds;
            if (AddFrameTime)
            {
                Console.Write($"C:{computeTime} ms\tD:{drawTime} ms  ");
            }

            _stopwatch.Stop();
        }

        public void DrawTwoLine(ulong[] mem, ulong startIdx)
        {
            var wordIdx = startIdx;
            for (ulong col = 0; col < WordsInRow; col++)
            {
                for (int selectedBit = 0; selectedBit < BitsInWord; selectedBit++)
                {
                    var topBit = ((mem[wordIdx + col] >> selectedBit) & 0x1) == 1;
                    var bottomBit = ((mem[wordIdx + col + WordsInRow] >> selectedBit) & 0x1) == 1;

                    var oChar = _charsOptions.EmptyChar;
                    if (topBit && bottomBit)
                        oChar = _charsOptions.FilledChar;
                    else if (topBit)
                        oChar = _charsOptions.TopHalfChar;
                    else if (bottomBit)
                        oChar = _charsOptions.BottomHalfChar;

                    _frame.Append(oChar);
                }

                if (AddSeparator && col + 1 < WordsInRow)
                    _frame.Append(_charsOptions.SeparatorChar);
            }

            _frame.Append('\n');
        }

        public void DrawLine(ulong[] mem, ulong startIdx)
        {
            var wordIdx = startIdx;
            for (ulong col = 0; col < WordsInRow; col++)
            {
                DrawNum(mem[wordIdx + col], BitsInWord);
                if (AddSeparator && col + 1 < WordsInRow)
                    _frame.Append(_charsOptions.SeparatorChar);
            }

            _frame.Append('\n');
        }

        public void DrawNum(ulong num, byte bitsInWord)
        {
            if (!ReverseByteOrder)
            {
                for (int selectedBit = 0; selectedBit < bitsInWord; selectedBit++)
                {
                    _frame.Append(
                        ((num >> selectedBit) & 0x1) == 1 ? _charsOptions.FilledChar : _charsOptions.EmptyChar);
                }
            }
            else
            {
                for (int selectedBit = bitsInWord - 1; selectedBit >= 0; selectedBit--)
                {
                    _frame.Append(
                        ((num >> selectedBit) & 0x1) == 1 ? _charsOptions.FilledChar : _charsOptions.EmptyChar);
                }
            }
        }
    }
}