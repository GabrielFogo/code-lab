using System.ComponentModel.DataAnnotations;

namespace CodeLab.Models
{
    public class CriaPerguntaViewModel
    {
        [Required(ErrorMessage = "A linguagem é obrigatória.")]
        public string? Linguagem { get; set; }

        [Required(ErrorMessage = "O nível é obrigatório.")]
        public string? Nivel { get; set; }

        [Required(ErrorMessage = "A descrição da pergunta é obrigatória.")]
        [StringLength(200, ErrorMessage = "A descrição não pode exceder 200 caracteres.")]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "A alternativa A é obrigatória.")]
        public string? Alternativa1 { get; set; }

        [Required(ErrorMessage = "A alternativa B é obrigatória.")]
        public string? Alternativa2 { get; set; }

        [Required(ErrorMessage = "A alternativa C é obrigatória.")]
        public string? Alternativa3 { get; set; }

        [Required(ErrorMessage = "A alternativa D é obrigatória.")]
        public string? Alternativa4 { get; set; }

        [Required(ErrorMessage = "A alternativa correta é obrigatória.")]
        public string? AlternativaCorreta { get; set; }
    }
}
