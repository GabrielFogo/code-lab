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

        public async Task<IActionResult> Index([FromQuery] string lang = "html", string nivel = "1")
        {
            var perguntas = await _perguntaRepository.GetFiltredAsync(lang, nivel);
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
            return RedirectToAction("Index","Perguntas");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Perguntas/Delete")]
        public async Task<IActionResult> Delete(string id)
        { 
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("ID não pode ser nulo ou vazio.");
            }

            await _perguntaRepository.DeleteAsync(id);
            return RedirectToAction("Index", "Perguntas");
        }

    }
}
