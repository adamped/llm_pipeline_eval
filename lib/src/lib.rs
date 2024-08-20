pub mod interop;

pub struct EvalRunner<'a> {
    bert_callback: Box<dyn Fn(String) -> Vec<f32> + 'a>,
}

impl<'a> EvalRunner<'a> {
    pub fn init(bert_callback: impl Fn(String) -> Vec<f32> + 'a) -> Self {
        EvalRunner {
            bert_callback: Box::new(bert_callback),
        }
    }

    pub fn test(&self) -> Vec<f32> {
        (self.bert_callback)(String::from("test"))
    }
}
