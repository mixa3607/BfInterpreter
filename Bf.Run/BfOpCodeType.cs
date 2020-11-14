namespace Bf.Run
{
    public enum BfOpCodeType : byte
    {
        None,
        LoopStart,
        LoopEnd,
        SlotValInc,
        SlotValDec,
        SlotPtrInc,
        SlotPtrDec,
        Read,
        Write,
        Nop
    }
}