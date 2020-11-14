using PowerArgs;

namespace Bf
{
    class Program
    {
        static void Main(string[] args)
        {
            Args.InvokeAction<Options>(args);
            return;
            //var parser = new BrainfuckParser(new BrainfuckParserOptions()
            //    {EnableMemPtrOpOptimization = true, EnableMemValueOpOptimization = true});
            //var opCodes = parser.Parse(
            //    @"+[-->-[>>+>-----<<]<--<---]>-.>>>+.>>..+++[.>]<<<<.+++.------.<<-.>>>>+.");
            //
            //var bf = new BrainfuckRuntime(new BrainfuckRuntimeOptions()
            //{
            //    MemSlotSizeBits = 8,
            //    InstrPtrSizeBits = 8,
            //    SlotsCount = 1024 * 100
            //}, opCodes, new BrainfuckIO());
            //bf.Execute();
            //new BadApple(16, 16, 192)
            //{
            //    FramesPerSec = 10,
            //    ImagesLoadParallelism = 20,
            //    ImagesDir = "Images/0.1s"
            //}.Run();

            new ImagesAsBfMemConsoleRenderer(16, 16, 196)// 4, 96)
            {
                FramesPerSec = 20,
                ImagesLoadParallelism = 20,
                ImagesDir = "Images/20fps"
            }.Run();
        }
    }
}