using PowerArgs;

namespace Bf
{
    public class ExecuteArgs
    {
        [ArgShortcut("b"), ArgRange(1, sizeof(ulong)*8), ArgDefaultValue(8)]
        public byte BitsInWord { get; set; } = 8;

        [ArgShortcut("w"), ArgRange(1, int.MaxValue), ArgDefaultValue(30_000)]
        public int Words { get; set; } = 30_000;

        [ArgShortcut("mem-bits"), ArgRange(1, sizeof(ulong)*8), ArgDefaultValue(8)]
        public byte BitsInMemPtr { get; set; } = 8;

        [ArgRequired, ArgShortcut("f")] 
        public string BfFile { get; set; }
    }
}