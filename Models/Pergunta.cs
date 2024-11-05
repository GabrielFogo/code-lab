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

        public static PerguntaViewModel ToPerguntaViewModel(Pergunta pergunta)
        {
            return new PerguntaViewModel()
            {
                Linguagem = pergunta.Linguagem,
                AlternativaCorreta = pergunta.AlternativaCorreta,
                Alternativa1 = pergunta.Alternativas[0].Description,
                Alternativa2 = pergunta.Alternativas[1].Description,
                Alternativa3 = pergunta.Alternativas[2].Description,
                Descricao = pergunta.Description,
                Nivel = pergunta.Nivel
            };
        }
    }
}
