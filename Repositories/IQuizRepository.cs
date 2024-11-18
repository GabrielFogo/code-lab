using CodeLab.Models;

namespace CodeLab.Repositories
{
    public interface IQuizRepository
    {
        Task<Quiz> CreateAsync(Quiz quiz);
        Task<Quiz> GetQuizsAsync(string id);
        Task UpdateAsync(string id, Quiz quiz);
    }
}
