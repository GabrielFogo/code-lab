using CodeLab.Models;

namespace CodeLab.Services;

public interface IPerguntaRepository
{
    Task<List<Pergunta>> GetFiltredAsync(string linguagem);
    Task<Pergunta> GetPerguntaByIdAsync(string id);
    Task CreateAsync(Pergunta pergunta);
    Task UpdateAsync(string id, Pergunta perguntaAtualizada);
    Task DeleteAsync(string id);
}