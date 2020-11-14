using System.Collections.Generic;
using Bf.Run.Io;
using Bf.Run.Options;

namespace Bf.Run
{
    public class BfRuntime
    {
        private ulong _iPtr;
        private readonly BfInstruction[] _instructions;

        private readonly ulong[] _mem;
        private readonly int _memSlotShiftClear;
        private ulong _memPtr;
        private readonly int _memPtrShiftClear;

        private readonly Stack<ulong> _loopPointers;
        private readonly Dictionary<ulong, ulong> _loopCache;

        private readonly BrainfuckRuntimeOptions _options;
        private readonly IBfIo _io;

        public ulong OpCounter { get; private set; }

        public BfRuntime(BrainfuckRuntimeOptions options, BfInstruction[] instructions, IBfIo io)
        {
            _options = options;
            _iPtr = _options.InitialIP;
            _instructions = instructions;
            _mem = new ulong[_options.SlotsCount];

            _loopPointers = new Stack<ulong>();
            _loopCache = new Dictionary<ulong, ulong>();

            _io = io;

            _memSlotShiftClear = sizeof(ulong) * 8 - _options.MemSlotSizeBits;
            _memPtrShiftClear = sizeof(ulong) * 8 - _options.MemPtrSizeBits;
        }

        private void ClearNum(ref ulong value, int bitsCount)
        {
            value <<= bitsCount;
            value >>= bitsCount;
        }

        private ulong ClearNum(ulong value, int bitsCount)
        {
            value <<= bitsCount;
            value >>= bitsCount;
            return value;
        }

        public void Execute()
        {
            // Run
            while (_iPtr < (ulong) _instructions.Length)
            {
                var currInstr = _instructions[_iPtr];
                var val = currInstr.Value;
                switch (currInstr.Type)
                {
                    case BfOpCodeType.SlotPtrInc:
                        _memPtr += val;
                        ClearNum(ref _memPtr, _memPtrShiftClear);
                        break;
                    case BfOpCodeType.SlotPtrDec:
                        _memPtr -= val;
                        ClearNum(ref _memPtr, _memPtrShiftClear);
                        break;
                    case BfOpCodeType.SlotValInc:
                        _mem[_memPtr] += val;
                        ClearNum(ref _mem[_memPtr], _memSlotShiftClear);
                        break;
                    case BfOpCodeType.SlotValDec:
                        _mem[_memPtr] -= val;
                        ClearNum(ref _mem[_memPtr], _memSlotShiftClear);
                        break;
                    case BfOpCodeType.Write: // .
                        _io.Write(_mem[_memPtr]);
                        break;
                    case BfOpCodeType.Read: // ,
                        _mem[_memPtr] = _io.ReadAsUlong();
                        break;
                    case BfOpCodeType.LoopStart: // [
                        if (_mem[_memPtr] != 0)
                        {
                            _loopPointers.Push(_iPtr);
                        }
                        else
                        {
                            if (_loopCache.ContainsKey(_iPtr))
                            {
                                _iPtr = _loopCache[_iPtr];
                            }
                            else
                            {
                                _iPtr++;

                                // Skip the loop.
                                var currentPointer = _iPtr;
                                var depth = 1;

                                for (var p = _iPtr; p < (ulong) _instructions.Length; p++)
                                {
                                    switch (_instructions[p].Type)
                                    {
                                        case BfOpCodeType.LoopStart:
                                            depth++;
                                            break;
                                        case BfOpCodeType.LoopEnd:
                                            depth--;
                                            break;
                                    }

                                    if (depth == 0)
                                    {
                                        _loopCache[currentPointer] = p;
                                        _iPtr = p;
                                        break;
                                    }
                                }
                            }
                        }

                        break;

                    case BfOpCodeType.LoopEnd: // ]
                        var oldPointer = _iPtr;

                        if (_loopPointers.TryPop(out _iPtr))
                        {
                            _loopCache[_iPtr] = oldPointer;
                            _iPtr--;
                        }

                        break;
                }

                _iPtr++;
                OpCounter++;
            }
        }
    }
}