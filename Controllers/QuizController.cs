using System.Text.Json;
using CodeLab.Models;
using CodeLab.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodeLab.Controllers;

public class QuizController : Controller
{
    private readonly IPerguntaRepository _perguntaRepository;
    private const string QuestionQueueKey = "QuestionQueue";

    public QuizController(IPerguntaRepository perguntaRepository)
    {
        _perguntaRepository = perguntaRepository;
    }

    public async Task<IActionResult> Index([FromQuery] string lang = "html")
    {
        var perguntas = await _perguntaRepository.GetFiltredByLangAsync(lang);
        var questionQueue = new Queue<Pergunta>(perguntas);
        HttpContext.Session.SetString(QuestionQueueKey, JsonSerializer.Serialize(questionQueue));

        return RedirectToAction("NextQuestion");
    }
    
    public IActionResult NextQuestion()
    {
        var questionQueueJson = HttpContext.Session.GetString(QuestionQueueKey);
        
        if (string.IsNullOrEmpty(questionQueueJson))
        {
            return View("QuizCompleted");
        }

        var questionQueue = JsonSerializer.Deserialize<Queue<Pergunta>>(questionQueueJson);

        if (questionQueue == null || questionQueue.Count == 0)
        {
            return View("QuizCompleted");
        }
        
        var perguntaAtual = questionQueue.Dequeue();
        
        HttpContext.Session.SetString(QuestionQueueKey, JsonSerializer.Serialize(questionQueue));
      
        return View("Index", perguntaAtual);
    }
}