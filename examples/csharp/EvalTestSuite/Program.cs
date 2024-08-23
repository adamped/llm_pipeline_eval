using LLMPipelineEval;

Console.WriteLine("Evaluation Test Suite");

BertCallback bertCallback = (string text) => [0.1f, 0.2f];

var runner = new EvalRunner(bertCallback);

Console.WriteLine($"Result: {runner.Similarity("The cat sat on a bench.", "On a bench there sat a cat.")}");



