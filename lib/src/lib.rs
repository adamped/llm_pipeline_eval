pub mod interop;

struct EvalRunner<'a> {
    bert_callback: Box<dyn Fn(String) -> Vec<f32> + 'a>,
}

impl<'a> EvalRunner<'a> {
    fn init(bert_callback: impl Fn(String) -> Vec<f32> + 'a) -> Self {
        EvalRunner {
            bert_callback: Box::new(bert_callback),
        }
    }
}
