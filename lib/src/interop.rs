use std::{
    ffi::{c_char, c_float, CStr, CString},
    str,
};

use interoptopus::{
    ffi_function, ffi_type, function,
    patterns::{primitives::FFIBool, slice::FFISlice},
    Inventory, InventoryBuilder,
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
pub extern "C" fn similarity(
    handle: EvalRunnerHandle,
    input: *const c_char,
    output: *const c_char,
    threshold: c_float,
) -> FFIBool {
    let runner = unsafe { &mut *(handle.instance) };

    let result = runner.similarity(
        c_char_to_str(input).unwrap(),
        c_char_to_str(output).unwrap(),
        threshold,
    );

    FFIBool::from(result)
}

fn c_char_to_str(c_str: *const c_char) -> Result<&'static str, std::str::Utf8Error> {
    unsafe {
        let c_str = CStr::from_ptr(c_str);

        c_str.to_str()
    }
}

#[ffi_function]
#[no_mangle]
pub extern "C" fn init(
    bert_callback: extern "C" fn(input: *const c_char) -> *const FFISlice<'static, f32>,
) -> EvalRunnerHandle<'static> {
    let rust_callback = move |input: &str| -> Vec<f32> {
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
        .register(function!(similarity))
        .inventory()
}
