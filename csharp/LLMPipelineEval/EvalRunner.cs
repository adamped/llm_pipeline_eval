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

    /// <summary>
    /// The callback function for when eval runner wants to call an LLM to get a result.
    /// </summary>
    /// <param name="prompt">The prompt to send to the LLM</param>
    /// <returns>The response from the LLM</returns>
    public delegate string LLMCallback(string prompt);

    public class EvalRunner
    {
        readonly BertCallback bertCallback;
        readonly LLMCallback llmCallback;
        readonly IntPtr handle;
        public EvalRunner(BertCallback bertCallback, LLMCallback llmCallback)
        {
            this.bertCallback = bertCallback;
            this.llmCallback = llmCallback;
            handle = Interop.init(BertCallback, LLMCallback);
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

        IntPtr LLMCallback(ref sbyte input)
        {
            unsafe
            {
                // Points to a null-terminated string
                fixed (sbyte* ptr = &input)
                {
                    string prompt = Marshal.PtrToStringAnsi((IntPtr)ptr)!;
                    return Marshal.StringToHGlobalAnsi(llmCallback(prompt));
                }
            }
        }

        ~EvalRunner()
        {
            Marshal.FreeHGlobal(handle);
        }
    }
}