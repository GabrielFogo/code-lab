using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CodeLab.Models
{
    public class Pergunta
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? Description { get; set; }

        public int NumeroDeErros { get; set; }

        public string? Linguagem { get; set; }

        public List<Alternativa> Alternativas { get; set; } = new List<Alternativa>();

        public string? Nivel { get; set; }
        public string? AlternativaCorreta { get; set; }
    }
}
