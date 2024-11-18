using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Numerics;

namespace CodeLab.Models
{
    public class Quiz
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? Linguagem { get; set; }
        public string? UserId { get; set; }
        public List<string> PerguntasAcertadas { get; set; } = new List<string>();
        public List<string> PerguntasErradas { get; set; } = new List<string>();

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal XpGanho { get; set; }
        public int NumeroDeAcertos => PerguntasAcertadas.Count;
        public int NumeroDeErros => PerguntasErradas.Count;

        public string? Nivel { get; set; }
    }
}
