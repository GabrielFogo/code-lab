using CodeLab.Models;
using CodeLab.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;

namespace CodeLab.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class PerguntasController : Controller
    {
        private readonly IPerguntaRepository _perguntaRepository;

        public PerguntasController(IPerguntaRepository perguntaRepository)
        {
            _perguntaRepository = perguntaRepository;
        }

        public async Task<IActionResult> Index([FromQuery] string lang = "html")
        {
            var perguntas = await _perguntaRepository.GetFiltredAsync(lang);
            return View(perguntas);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Linguagem,Nivel,Descricao,Alternativa1,Alternativa2,Alternativa3,Alternativa4,AlternativaCorreta")] CriaPerguntaViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var pergunta = new Pergunta
            {
                Linguagem = model.Linguagem,
                Description = model.Descricao,
                Nivel = model.Nivel,
                NumeroDeErros = 0,
                Alternativas = new List<Alternativa> {
                    new Alternativa()
                    {
                        Description = model.Descricao,
                        LetrasAlternativa = "A"
                    },
                    new Alternativa()
                    {
                        Description = model.Descricao,
                        LetrasAlternativa = "B"
                    },
                    new Alternativa()
                    {
                        Description = model.Descricao,
                        LetrasAlternativa = "C"
                    },
                    new Alternativa()
                    {
                        Description = model.Descricao,
                        LetrasAlternativa = "D"
                    },

                },
                AlternativaCorreta = model.AlternativaCorreta,
            };

            await _perguntaRepository.CreateAsync(pergunta);
            return RedirectToAction("Create","Perguntas");
        }
    }
}
