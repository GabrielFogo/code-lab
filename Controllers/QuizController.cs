using System.Text.Json;
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
    
    public async Task<IActionResult> Index([FromQuery] string nivel = "1", string lang = "html", int page = 1)
    {
        var pergunta = await _perguntaRepository.GetPaginatedAsync(lang, nivel, page, 1);
        
        if (pergunta.Count <= 0)
        {
            return View("QuizConcluido");
        }
        
        TempData["Pergunta"] =  JsonSerializer.Serialize(pergunta[0]);
        
        var viewModel = new QuizViewModel()
        {
            PerguntaDescricao = pergunta[0].Description,
            Alternativas = pergunta[0].Alternativas
        };
        
        ViewData["Page"] = page;
        ViewData["Lang"] = lang;
        ViewData["Nivel"] = nivel;
        
        return View(viewModel);
    }

    public async Task<IActionResult> Responder([Bind("AlternativaSeleciona", "QuizId")] QuizViewModel model, int page, string lang, string nivel)
    {
        if (!ModelState.IsValid)
            return View("Index", model);
    
        var perguntaJson = TempData["Pergunta"] as string;
        var pergunta = JsonSerializer.Deserialize<Pergunta>(perguntaJson!);
    
        if (model.AlternativaSeleciona != pergunta!.AlternativaCorreta)
        {
            pergunta.NumeroDeErros += 1;
            await _perguntaRepository.UpdateAsync(pergunta.Id!, pergunta);
        }
        
        return RedirectToAction("Index", new { lang, nivel, page = page + 1 });
    }

}