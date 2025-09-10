namespace Common.Utilities.AvroPayload;

using Avro.Generic;

public interface IPayloadBuilder
{
    byte[] Serialize(string schemaJson, Action<GenericRecordWrapper> fillRecord);
    GenericRecord Deserialize(string schemaJson, byte[] payload);
}