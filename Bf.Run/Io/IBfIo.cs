namespace Bf.Run.Io
{
    public interface IBfIo
    {
        ulong ReadAsUlong();
        void Write(ulong value);
        void Write(char value);
        void Write(string value);
        void ResetPos();
        void NextLine();
    }
}