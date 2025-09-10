namespace Common.Utilities.SchemaLoader;

public interface IAvroSchemaLoader
{
    string LoadSchema(string schemaName);
}