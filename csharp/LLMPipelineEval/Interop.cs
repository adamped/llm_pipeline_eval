// Automatically generated by Interoptopus.

#pragma warning disable 0105
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LLMPipelineEval;
#pragma warning restore 0105

namespace LLMPipelineEval
{
    public static partial class Interop
    {
        public const string NativeLib = "llm_pipeline_eval";

        static Interop()
        {
        }


        [DllImport(NativeLib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "init")]
        public static extern IntPtr init(InteropDelegate_fn_pconst_i8_rval_pconst_Slicef32 bert_callback, InteropDelegate_fn_pconst_i8_rval_pconst_i8 llm_callback);

        [DllImport(NativeLib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "similarity")]
        public static extern Bool similarity(IntPtr handle, ref sbyte input, ref sbyte output, float threshold);

    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr InteropDelegate_fn_pconst_i8_rval_pconst_i8(ref sbyte x0);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr InteropDelegate_fn_pconst_i8_rval_pconst_Slicef32(ref sbyte x0);

    ///A pointer to an array of data someone else owns which may not be modified.
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Slicef32
    {
        ///Pointer to start of immutable data.
        IntPtr data;
        ///Number of elements.
        ulong len;
    }

    public partial struct Slicef32 : IEnumerable<float>
    {
        public Slicef32(GCHandle handle, ulong count)
        {
            this.data = handle.AddrOfPinnedObject();
            this.len = count;
        }
        public Slicef32(IntPtr handle, ulong count)
        {
            this.data = handle;
            this.len = count;
        }
        public float this[int i]
        {
            get
            {
                if (i >= Count) throw new IndexOutOfRangeException();
                var size = Marshal.SizeOf(typeof(float));
                var ptr = new IntPtr(data.ToInt64() + i * size);
                return Marshal.PtrToStructure<float>(ptr);
            }
        }
        public float[] Copied
        {
            get
            {
                var rval = new float[len];
                for (var i = 0; i < (int) len; i++) {
                    rval[i] = this[i];
                }
                return rval;
            }
        }
        public int Count => (int) len;
        public IEnumerator<float> GetEnumerator()
        {
            for (var i = 0; i < (int)len; ++i)
            {
                yield return this[i];
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }


    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Bool
    {
        byte value;
    }

    public partial struct Bool
    {
        public static readonly Bool True = new Bool { value =  1 };
        public static readonly Bool False = new Bool { value =  0 };
        public Bool(bool b)
        {
            value = (byte) (b ? 1 : 0);
        }
        public bool Is => value == 1;
    }




    public class InteropException<T> : Exception
    {
        public T Error { get; private set; }

        public InteropException(T error): base($"Something went wrong: {error}")
        {
            Error = error;
        }
    }

}
