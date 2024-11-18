using System.Security.Claims;
using System.Text.Json;
using CodeLab.Models;
using CodeLab.Repositories;
using CodeLab.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeLab.Controllers;

[Authorize]
public class QuizController : Controller
{
    private readonly IPerguntaRepository _perguntaRepository;
    private readonly IQuizRepository _quizRepository;

    public QuizController(IPerguntaRepository perguntaRepository, IQuizRepository quizRepository)
    {
        _perguntaRepository = perguntaRepository;
        _quizRepository = quizRepository;
    }

    public async Task<IActionResult> Index([FromQuery] string nivel = "1", string lang = "html", int page = 1, string quizId = "")
    {
        var pergunta = await _perguntaRepository.GetPaginatedAsync(lang, nivel, page, 1);

        if (pergunta.Count <= 0)
        {
            return View("QuizConcluido");
        }

        if (string.IsNullOrEmpty(quizId)) {
            var quiz = new Quiz()
            {
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Nivel = nivel,
                Linguagem = lang,
            };

            var quizCriado = await _quizRepository.CreateAsync(quiz);
            quizId = quizCriado.Id!;
        }
        
        TempData["Pergunta"] =  JsonSerializer.Serialize(pergunta[0]);
        
        var viewModel = new QuizViewModel()
        {
            PerguntaDescricao = pergunta[0].Description,
            Alternativas = pergunta[0].Alternativas,
            QuizId = quizId
        };
        
        ViewData["Page"] = page;
        ViewData["Lang"] = lang;
        ViewData["Nivel"] = nivel;
        
        return View(viewModel);
    }

    public async Task<IActionResult> Responder([Bind("AlternativaSeleciona", "QuizId")] QuizViewModel model, int page, string lang, string nivel,string quizId)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("Index", new { lang, nivel, page = page, quizId });

        var perguntaJson = TempData["Pergunta"] as string;
        var pergunta = JsonSerializer.Deserialize<Pergunta>(perguntaJson!);
        var quiz = await _quizRepository.GetQuizsAsync(quizId);
    
        if (model.AlternativaSeleciona != pergunta!.AlternativaCorreta)
        {
            quiz.PerguntasErradas.Add(pergunta.Id);
        }
        else
        {
            quiz.PerguntasAcertadas.Add(pergunta.Id);
        }
        
        await _quizRepository.UpdateAsync(quizId, quiz);
        
        return RedirectToAction("Index", new { lang, nivel, page = page + 1, quizId });
    }

}