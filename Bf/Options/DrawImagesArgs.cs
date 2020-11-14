using System;
using PowerArgs;

namespace Bf
{
    public class DrawImagesArgs
    {
        [ArgRequired, ArgShortcut("b"), ArgRange(1, sizeof(ulong)*8)]
        public byte BitsInWord { get; set; }

        [ArgRequired, ArgShortcut("w"), ArgRange(1, Double.MaxValue)]
        public uint WordsInRow { get; set; }

        [ArgRequired, ArgShortcut("r"), ArgRange(1, Double.MaxValue)]
        public uint Rows { get; set; }

        [ArgRequired, ArgShortcut("d")] public string ImagesDir { get; set; }

        [ArgShortcut("loadThreads"), ArgRange(1, int.MaxValue), ArgDefaultValue(8)]
        public int ImagesLoadParallelism { get; set; } = 8;

        [ArgShortcut("fps"), ArgShortcut("f"), ArgRange(1, int.MaxValue), ArgDefaultValue(10)]
        public int FramesPerSec { get; set; } = 10;
    }
}