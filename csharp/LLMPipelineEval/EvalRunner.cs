using System.Runtime.InteropServices;

namespace LLMPipelineEval
{
    /// <summary>
    /// The callback function for when the eval runner wants to compare outputs from LLMs for similar meaning
    /// even if the text is not exact.
    /// </summary>
    /// <param name="text">The text to convert to a vector</param>
    /// <returns>A vector representation of the text</returns>
    public delegate float[] BertCallback(string text);

    public class EvalRunner
    {
        readonly BertCallback bertCallback;
        readonly IntPtr handle;
        public EvalRunner(BertCallback bertCallback)
        {
            this.bertCallback = bertCallback;
            handle = Interop.init(BertCallback);
        }

        public bool Similarity(string input, string output, float threshold = 0.9f)
        {
            var inputRef = input.StringToSByte();
            var outputRef = output.StringToSByte();

            return Interop.similarity(handle, ref inputRef, ref outputRef, threshold).Is;
        }

        IntPtr BertCallback(ref sbyte input)
        {
            unsafe
            {
                // Points to a null-terminated string
                fixed (sbyte* ptr = &input)
                {
                    string text = Marshal.PtrToStringAnsi((IntPtr)ptr)!;
                    var vectors = bertCallback(text);

                    var arrayHandle = GCHandle.Alloc(vectors, GCHandleType.Pinned);
                    var data = new Slicef32(arrayHandle, (ulong)vectors.Length);

                    return GCHandle.Alloc(data, GCHandleType.Pinned).AddrOfPinnedObject();
                }
            }
        }

        ~EvalRunner()
        {
            Marshal.FreeHGlobal(handle);
        }
    }
}