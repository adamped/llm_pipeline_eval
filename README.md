# LLM Pipeline Eval

A testing framework for LLM and RAG pipelines to help ensure your RAG and LLMs perform at expected levels.

Built in Rust for speed, but interop libraries to be developed for integration into various languages.

## Platform Support

| Architecture | Rust | C# | Python |
|----------|----------|----------|----------|
| Linux (x64) | ✓ | ✓ | Soon |
| Windows | - | - | - |

## Evaluations

| Evaluation | BERT | LLM | RAG |
|----------|----------|----------|---------|
| Similarity | ✓ | ✓ | ✓ |


## Development Notes

- Rust library was not built with async in mind, it is expected the interop libraries will handle async operations due to the differences in how languages handles threads.