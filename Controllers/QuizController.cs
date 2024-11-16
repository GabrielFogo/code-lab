using CodeLab.Models;
using CodeLab.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeLab.Controllers;

public class QuizController : Controller
{
    private readonly IPerguntaRepository _perguntaRepository;

    public QuizController(IPerguntaRepository perguntaRepository)
    {
        _perguntaRepository = perguntaRepository;
    }

    // GET
    public async Task<IActionResult> Index([FromQuery] string nivel = "1", string lang = "html", int page = 1)
    {
        var pergunta = await _perguntaRepository.GetPaginatedAsync(lang, nivel, page, 1);
        if (pergunta.Count <= 0)
        {
            return View("QuizConcluido");
        }
        
        var viewModel = new QuizViewModel()
        {
            Pergunta = pergunta[0]
        };

        return View(viewModel);
    }
}