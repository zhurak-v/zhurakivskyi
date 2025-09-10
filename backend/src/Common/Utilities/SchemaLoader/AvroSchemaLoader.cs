namespace Common.Utilities.SchemaLoader;

public class AvroSchemaLoader : IAvroSchemaLoader
{
    private const string SchemasFolder = "Core/Schemas";
    
    public string LoadSchema(string schemaName)
    {
        var fileName = $"{schemaName}.avsc";
        var fullPath = Path.Combine(AppContext.BaseDirectory, SchemasFolder, fileName);
        return File.ReadAllText(fullPath);
    }
}