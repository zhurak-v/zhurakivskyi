namespace Common.Utilities.AvroPayload;

using System;
using System.IO;
using System.Linq;
using Avro;
using Avro.Generic;
using Avro.IO;

public class PayloadBuilder : IPayloadBuilder
{
    public byte[] Serialize(string schemaJson, Action<GenericRecordWrapper> fillRecord)
    {
        var avroSchema = Schema.Parse(schemaJson);
        var recordSchema = (RecordSchema)avroSchema;

        var record = new GenericRecord(recordSchema);
        var wrapper = new GenericRecordWrapper(record);

        fillRecord(wrapper);

        using var ms = new MemoryStream();
        var encoder = new BinaryEncoder(ms);
        var writer = new GenericWriter<GenericRecord>(recordSchema);
        writer.Write(record, encoder);
        encoder.Flush();

        return ms.ToArray();
    }

    public GenericRecord Deserialize(string schemaJson, byte[] payload)
    {
        var avroSchema = Schema.Parse(schemaJson);
        var recordSchema = (RecordSchema)avroSchema;

        using var ms = new MemoryStream(payload);
        var decoder = new BinaryDecoder(ms);
        var reader = new GenericReader<GenericRecord>(recordSchema, recordSchema);
        var record = reader.Read(null, decoder);

        return record;
    }
}

public class GenericRecordWrapper
{
    private readonly GenericRecord _record;

    public GenericRecordWrapper(GenericRecord record)
    {
        _record = record;
    }
    
    private GenericRecord Record => _record;

    public void Add(string fieldName, object? value)
    {
        var field = ((RecordSchema)_record.Schema).Fields.First(f => f.Name == fieldName);
        _record.Add(fieldName, ProcessValue(field.Schema, value));
    }

    private object? ProcessValue(Schema schema, object? value)
    {
        switch (schema.Tag)
        {
            case Schema.Type.Null:
                return null;

            case Schema.Type.String:
                return value?.ToString() ?? string.Empty;

            case Schema.Type.Int:
                return Convert.ToInt32(value);

            case Schema.Type.Long:
                return Convert.ToInt64(value);

            case Schema.Type.Float:
                return Convert.ToSingle(value);

            case Schema.Type.Double:
                return Convert.ToDouble(value);

            case Schema.Type.Boolean:
                return Convert.ToBoolean(value);

            case Schema.Type.Enumeration:
                var enumSchema = (EnumSchema)schema;
                return new GenericEnum(enumSchema, value?.ToString() ?? enumSchema.Symbols.First());

            case Schema.Type.Union:
                var unionSchema = (UnionSchema)schema;
                if (value == null) return null;
                var actualSchema = unionSchema.Schemas.First(s => s.Tag != Schema.Type.Null);
                return ProcessValue(actualSchema, value);

            case Schema.Type.Record:
                if (value is GenericRecordWrapper wrapper)
                    return wrapper.Record;
                return value;

            default:
                return value;
        }
    }
}
