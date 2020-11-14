using System;

namespace Bf.Run.Io
{
    public class BfIo : IBfIo
    {
        public BfIo()
        {
            //Console.OutputEncoding = Encoding.ASCII;
        }

        public ulong ReadAsUlong()
        {
            return (ulong)Console.Read();
        }

        public void Write(ulong value)
        {
            Console.Write(Convert.ToChar(value));
        }

        public void Write(char value)
        {
            Console.Write(value);
        }

        public void Write(string value)
        {
            Console.Write(value);
        }

        public void ResetPos()
        {
            Console.SetCursorPosition(0,0);
        }

        public void NextLine()
        {
            Console.WriteLine();
        }
    }
}