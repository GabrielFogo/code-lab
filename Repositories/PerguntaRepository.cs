using CodeLab.Data;
using CodeLab.Models;
using MongoDB.Driver;

namespace CodeLab.Services;

public class PerguntasRepository : IPerguntaRepository
{
    private readonly IMongoCollection<Pergunta> _perguntasCollection;

    public PerguntasRepository(ContextMongoDb mongoDb)
    {
        _perguntasCollection = mongoDb.Perguntas;
    }

    // Método para obter perguntas por linguagem
    public async Task<List<Pergunta>> GetFiltredAsync(string linguagem, string nivel)
    {
        var langFilter = Builders<Pergunta>.Filter.Eq(p => p.Linguagem, linguagem);
        var nivelFilter = Builders<Pergunta>.Filter.Eq(p => p.Nivel, nivel);
        var filter = Builders<Pergunta>.Filter.And(langFilter, nivelFilter);
        
        return await _perguntasCollection.Find(filter).ToListAsync();
    }
    
    public async Task<List<Pergunta>> GetFiltredByLangAsync(string linguagem)
    {
        var filter = Builders<Pergunta>.Filter.Eq(p => p.Linguagem, linguagem);
        return await _perguntasCollection.Find(filter).ToListAsync();
    }

    // Método para obter uma pergunta pelo ID
    public async Task<Pergunta> GetPerguntaByIdAsync(string id)
    {
        var filter = Builders<Pergunta>.Filter.Eq(p => p.Id, id);
        return await _perguntasCollection.Find(filter).FirstOrDefaultAsync();
    }
    
    // Método para criar uma nova pergunta
    public async Task CreateAsync(Pergunta pergunta)
        => await _perguntasCollection.InsertOneAsync(pergunta);
    
    // Método para atualizar uma pergunta existente
    public async Task UpdateAsync(string id, Pergunta perguntaAtualizada)
    {
        var filter = Builders<Pergunta>.Filter.Eq(p => p.Id, id);

        var update = Builders<Pergunta>.Update
            .Set(p => p.Linguagem, perguntaAtualizada.Linguagem)
            .Set(p => p.Description, perguntaAtualizada.Description)
            .Set(p => p.NumeroDeErros, perguntaAtualizada.NumeroDeErros)
            .Set(p => p.Nivel, perguntaAtualizada.Nivel)
            .Set(p => p.AlternativaCorreta, perguntaAtualizada.AlternativaCorreta);
        // Atualizando as alternativas
        for (int i = 0; i < perguntaAtualizada.Alternativas.Count; i++)
        {
            update = update.Set(p => p.Alternativas[i], perguntaAtualizada.Alternativas[i]);
        }

        await _perguntasCollection.UpdateOneAsync(filter, update);
    }

    // Método para deletar uma pergunta
    public async Task DeleteAsync(string id)
    {
        var filter = Builders<Pergunta>.Filter.Eq(p => p.Id, id);
        await _perguntasCollection.DeleteOneAsync(filter);
    }
}
