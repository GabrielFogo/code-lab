using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CodeLab.Models
{
    public class ApplicationUser : MongoIdentityUser<Guid>
    {
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal XpGanho { get; set; } = 0;

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal XpNescessario { get; set; } = 1000;

        [BsonRepresentation(BsonType.Int64)]
        public int NivelXp { get; set; } = 1;

        public int Dinheiro { get; set; } = 0;
    }
}