using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using notifier.dal.attributes;
using System;

namespace notifier.dal.entities
{
    [MongoCollection(Name = "NotiferLog")]
    public class BaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public short Active { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
