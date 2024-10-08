use edge_vector_index::similarity;
pub mod interop;

pub struct EvalRunner<'a> {
    bert_callback: Box<dyn Fn(&str) -> Vec<f32> + 'a>,
    llm_callback: Box<dyn Fn(&str) -> String + 'a>,
}

impl<'a> EvalRunner<'a> {
    pub fn init(
        bert_callback: impl Fn(&str) -> Vec<f32> + 'a,
        llm_callback: impl Fn(&str) -> String + 'a,
    ) -> Self {
        EvalRunner {
            bert_callback: Box::new(bert_callback),
            llm_callback: Box::new(llm_callback),
        }
    }

    pub fn similarity(&self, input: &str, output: &str, threshold: f32) -> bool {
        let input_vec = (self.bert_callback)(input);
        let output_vec = (self.bert_callback)(output);

        let similarity = similarity::cosine_similarity(&input_vec, &output_vec);

        similarity >= threshold
    }
}
