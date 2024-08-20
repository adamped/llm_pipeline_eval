using System.Runtime.InteropServices;
using LLMPipelineEval;

Console.WriteLine("Evaluation Test Suite");

var handle = Interop.init(MyCallbackFunction);

Console.WriteLine(string.Join(",", Interop.test(handle)));

// When finished
Marshal.FreeHGlobal(handle);

static IntPtr MyCallbackFunction(ref sbyte input)
{
    unsafe
    {
        // Points to a null-terminated string
        fixed (sbyte* ptr = &input)
        {
            string prompt = Marshal.PtrToStringAnsi((IntPtr)ptr)!;
            var vectors = new float[] { 0.1f, 0.2f, 0.3f };

            var arrayHandle = GCHandle.Alloc(vectors, GCHandleType.Pinned);
            var data = new Slicef32(arrayHandle, (ulong)vectors.Length);

            return GCHandle.Alloc(data, GCHandleType.Pinned).AddrOfPinnedObject();
        }
    }


}