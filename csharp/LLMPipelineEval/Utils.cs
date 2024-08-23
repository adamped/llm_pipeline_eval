using System.Text;

namespace LLMPipelineEval
{
    public static class Utils
    {
        public static sbyte StringToSByte(this string text)
        {
            var bytes = Encoding.ASCII.GetBytes(text);

            return (sbyte)bytes[0];
        }
    }
}