namespace Bf.Run
{
    public class BfInstruction
    {
        public BfOpCodeType Type { get; set; }
        public ulong Value { get; set; }

        public BfInstruction(BfOpCodeType type,  ulong value)
        {
            Type = type;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Type}, {Value}";
        }
    }
}