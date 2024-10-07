using System.ComponentModel.DataAnnotations;

namespace CodeLab.Models;

public class LoginViewModel
{
    [Required (ErrorMessage = "O campo email não pode estar vazio.")]
    public string? Username { get; init; }

    [Required(ErrorMessage = "O campo senha não pode estar vazio.")]
    [DataType(DataType.Password)]
    public string? Password { get; init; }
}