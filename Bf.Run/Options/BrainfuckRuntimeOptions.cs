namespace Bf.Run.Options
{
    public class BrainfuckRuntimeOptions
    {
        public int SlotsCount { get; set; }
        public int MemSlotSizeBits { get; set; }
        public int MemPtrSizeBits { get; set; }
        public ulong InitialIP { get; set; }
    }
}