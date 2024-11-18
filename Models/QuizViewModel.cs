using System.ComponentModel.DataAnnotations;

namespace CodeLab.Models;

public class QuizViewModel
{
    public List<Alternativa>? Alternativas { get; set; }
    public string? PerguntaDescricao { get; set; }
    
    [Required(ErrorMessage = "Selecione uma alternativa")]
    public string? AlternativaSeleciona { get; set; }
    public string? QuizId { get; set; }
}