using System.ComponentModel.DataAnnotations;

namespace CodeLab.Models;

public class RegisterViewModel
{

    [Required(ErrorMessage = "O campo email não pode estar vazio.")]
    [EmailAddress(ErrorMessage = "Digite um email valido.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "O campo senha não pode estar vazio.")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
}