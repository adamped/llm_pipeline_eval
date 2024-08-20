use std::{
    ffi::{c_char, CString},
    mem,
};

use interoptopus::{
    ffi_function, ffi_type, function, patterns::slice::FFISlice, Inventory, InventoryBuilder,
};

use crate::EvalRunner;

#[ffi_type(opaque)]
#[repr(C)]
pub struct EvalRunnerHandle<'a> {
    instance: *mut EvalRunner<'a>,
}

impl EvalRunnerHandle<'_> {
    fn out_of_rust(value: EvalRunner) -> EvalRunnerHandle {
        EvalRunnerHandle {
            instance: Box::into_raw(Box::new(value)),
        }
    }
}

#[ffi_function]
#[no_mangle]
pub extern "C" fn test(handle: EvalRunnerHandle) -> FFISlice<f32> {
    let runner = unsafe { &mut *(handle.instance) };
    let vec = runner.test();

    let slice = unsafe { std::slice::from_raw_parts(vec.as_ptr(), vec.len()) };
    mem::forget(vec);
    FFISlice::from_slice(slice)
}

#[ffi_function]
#[no_mangle]
pub extern "C" fn init(
    bert_callback: extern "C" fn(input: *const c_char) -> *const FFISlice<'static, f32>,
) -> EvalRunnerHandle<'static> {
    let rust_callback = move |input: String| -> Vec<f32> {
        let c_string = CString::new(input).unwrap();
        let c_str_ptr = c_string.as_ptr();

        let result_ptr = { bert_callback(c_str_ptr) };

        let slice = unsafe { &*result_ptr };
        slice.to_vec()
    };

    EvalRunnerHandle::out_of_rust(EvalRunner::init(rust_callback))
}

pub fn create_inventory() -> Inventory {
    InventoryBuilder::new()
        .register(function!(init))
        .register(function!(test))
        .inventory()
}
