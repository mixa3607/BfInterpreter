using System;
using Bf.Run.Options;

namespace Bf.Run
{
    public class BfParser
    {
        private readonly BrainfuckParserOptions _options;

        public BfParser(BrainfuckParserOptions options)
        {
            _options = options;
        }

        public BfInstruction[] Parse(string program)
        {
            var opCodes = new BfInstruction[program.Length];
            var opCodeCounter = 0;
            for (int i = 0; i < program.Length; i++)
            {
                var currentOpCodeType = GetOpCodeType(program[i]);
                if (currentOpCodeType == BfOpCodeType.None)
                    continue;

                if (opCodeCounter > 0
                    && opCodes[opCodeCounter - 1].Type == currentOpCodeType
                    && (((currentOpCodeType == BfOpCodeType.SlotValDec || currentOpCodeType == BfOpCodeType.SlotValInc)
                         && _options.EnableMemValueOpOptimization)
                        || ((currentOpCodeType == BfOpCodeType.SlotPtrInc || currentOpCodeType == BfOpCodeType.SlotPtrDec)
                            && _options.EnableMemPtrOpOptimization)
                    ))
                {
                    opCodes[opCodeCounter - 1].Value++;
                }
                else
                {
                    var opCode = new BfInstruction(currentOpCodeType, 1);
                    opCodes[opCodeCounter] = opCode;
                    opCodeCounter++;
                }
            }

            Array.Resize(ref opCodes, opCodeCounter);
            return opCodes;
        }

        public BfOpCodeType GetOpCodeType(char opChar)
        {
            return opChar switch
            {
                '+' => BfOpCodeType.SlotValInc,
                '-' => BfOpCodeType.SlotValDec,
                '>' => BfOpCodeType.SlotPtrInc,
                '<' => BfOpCodeType.SlotPtrDec,
                '[' => BfOpCodeType.LoopStart,
                ']' => BfOpCodeType.LoopEnd,
                '.' => BfOpCodeType.Write,
                ',' => BfOpCodeType.Read,
                'N' => BfOpCodeType.Nop,
                _ => BfOpCodeType.None
            };
        }
    }
}