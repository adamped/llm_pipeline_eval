use interoptopus::util::NamespaceMappings;
use interoptopus::{Error, Interop};

#[test]
#[cfg_attr(miri, ignore)]
fn bindings_csharp() -> Result<(), Error> {
    use interoptopus_backend_csharp::{Config, Generator};

    Generator::new(
        Config {
            class: "Interop".to_string(),
            dll_name: "llm_pipeline_eval".to_string(),
            namespace_mappings: NamespaceMappings::new("LLMPipelineEval"),
            ..Config::default()
        },
        llm_pipeline_eval::interop::create_inventory(),
    )
    .write_file("../csharp/LLMPipelineEval/Interop.cs")?;

    Ok(())
}
