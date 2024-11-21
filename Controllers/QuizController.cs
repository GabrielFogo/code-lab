using System.Security.Claims;
using System.Text.Json;
using CodeLab.Models;
using CodeLab.Repositories;
using CodeLab.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CodeLab.Controllers;

[Authorize]
public class QuizController : Controller
{
    private readonly IPerguntaRepository _perguntaRepository;
    private readonly IQuizRepository _quizRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public QuizController(IPerguntaRepository perguntaRepository, IQuizRepository quizRepository, UserManager<ApplicationUser> userManager)
    {
        _perguntaRepository = perguntaRepository;
        _quizRepository = quizRepository;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index(
        [FromQuery] string nivel = "1",
        string lang = "html",
        int page = 1,
        string quizId = "",
        bool respondida = false,
        bool correta = false
        )
    {
        var pergunta = await _perguntaRepository.GetPaginatedAsync(lang, nivel, page, 1);

        if (pergunta.Count <= 0)
        {
            var quiz = await _quizRepository.GetQuizsAsync(quizId);
            return View("QuizConcluido", quiz);
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
        ViewData["Respondida"] = respondida;
        ViewData["Correta"] = correta;
        
        return View(viewModel);
    }

    public async Task<IActionResult> Responder([Bind("AlternativaSeleciona", "QuizId")] QuizViewModel model, int page, string lang, string nivel,string quizId)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("Index", new { lang, nivel, page = page, quizId });

        var perguntaJson = TempData["Pergunta"] as string;
        var pergunta = JsonSerializer.Deserialize<Pergunta>(perguntaJson!);
        var quiz = await _quizRepository.GetQuizsAsync(quizId);
        var correta = false;
        
        if (model.AlternativaSeleciona != pergunta!.AlternativaCorreta)
        {
            quiz.PerguntasErradas.Add(pergunta.Id!);
        }
        else
        {
            quiz.PerguntasAcertadas.Add(pergunta.Id!);
            quiz.XpGanho += 500 * int.Parse(pergunta.Nivel!);

            var user = await _userManager.FindByIdAsync(quiz.UserId!);

            user!.Dinheiro += 1;
            user.XpGanho += 200;

            if(user.XpGanho >= user.XpNescessario)
            {
                user.XpGanho = 0;
                user.NivelXp += 1;
                user.XpNescessario *= user.NivelXp;
            }

            await _userManager.UpdateAsync(user);
            correta = true;
        }
        
        await _quizRepository.UpdateAsync(quizId, quiz);
        var respondida = true;
        return RedirectToAction("Index", new { lang, nivel, page, quizId, respondida, correta });
    }

}