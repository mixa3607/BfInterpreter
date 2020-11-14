namespace Bf.Render
{
    public class BfMemoryConsoleRendererOptions
    {
        public char TopHalfChar { get; set; } = '▀';
        public char BottomHalfChar { get; set; } = '▄';
        public char FilledChar { get; set; } = '█';
        public char EmptyChar { get; set; } = '_';
        public char SeparatorChar { get; set; } = '|';
    }
}