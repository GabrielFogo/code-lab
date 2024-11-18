using CodeLab.Data;
using CodeLab.Models;
using MongoDB.Driver;

namespace CodeLab.Repositories
{
    public class QuizRepository : IQuizRepository
    {

        private readonly IMongoCollection<Quiz> _quizCollection;

        public QuizRepository(ContextMongoDb mongoDb)
        {
            _quizCollection = mongoDb.Quizzes;
        }

        public async Task<Quiz> CreateAsync(Quiz quiz)
        {
            await _quizCollection.InsertOneAsync(quiz);
            return quiz;
        }

        public async Task<Quiz> GetQuizsAsync(string id)
        {
            var filter = Builders<Quiz>.Filter.Eq(p => p.Id, id);
            return await _quizCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(string id, Quiz quiz)
        {
            var filter = Builders<Quiz>.Filter.Eq(p => p.Id, id);
            var update = Builders<Quiz>.Update
                .Set(p => p.PerguntasAcertadas, quiz.PerguntasAcertadas)
                .Set(p => p.PerguntasErradas, quiz.PerguntasErradas);
           
            await _quizCollection.UpdateOneAsync(filter, update);
        }
    }
}
