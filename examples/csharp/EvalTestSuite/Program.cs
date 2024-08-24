using LLMPipelineEval;

Console.WriteLine("Evaluation Test Suite");

BertCallback bertCallback = (string text) => [0.1f, 0.2f];

LLMCallback llmCallback = (string prompt) => "Response";

var runner = new EvalRunner(bertCallback, llmCallback);

Console.WriteLine($"Result: {runner.Similarity("The cat sat on a bench.", "On a bench there sat a cat.")}");



