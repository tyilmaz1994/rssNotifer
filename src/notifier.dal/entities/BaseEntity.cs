using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using notifier.dal.attributes;
using System;

namespace notifier.dal.entities
{
    public class BaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public short Active { get; set; } = 1; // 1 is represent notifier.bl.enums.Active.Yes

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
