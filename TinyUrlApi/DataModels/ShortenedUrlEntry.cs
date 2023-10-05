using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TinyUrlApi.DataModels
{
    public class ShortenedUrlEntry
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public string FullUrl { get; set; }
        public string ShortUrl { get; set; }
    }
}
