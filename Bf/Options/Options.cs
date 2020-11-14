using System.IO;
using Bf.Run;
using Bf.Run.Io;
using Bf.Run.Options;
using PowerArgs;

namespace Bf
{
    [ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
    public class Options
    {
        [HelpHook, ArgShortcut("-?"), ArgDescription("Shows this help")]
        public bool Help { get; set; }

        [ArgActionMethod, ArgShortcut("draw")]
        public void DrawImages(DrawImagesArgs args)
        {
            new ImagesAsBfMemConsoleRenderer(args.BitsInWord, args.WordsInRow, args.Rows)
            {
                FramesPerSec = args.FramesPerSec,
                ImagesLoadParallelism = args.ImagesLoadParallelism,
                ImagesDir = args.ImagesDir
            }.Run();
        }

        [ArgActionMethod, ArgShortcut("execute")]
        public void ExecuteBf(ExecuteArgs args)
        {
            var parser = new BfParser(new BrainfuckParserOptions()
            {
                EnableMemPtrOpOptimization = true,
                EnableMemValueOpOptimization = true
            });
            var opCodes = parser.Parse(File.ReadAllText(args.BfFile));

            var bf = new BfRuntime(new BrainfuckRuntimeOptions()
            {
                MemSlotSizeBits = args.BitsInWord,
                MemPtrSizeBits = args.BitsInMemPtr,
                SlotsCount = args.Words
            }, opCodes, new BfIo());
            bf.Execute();
        }
    }
}