using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using SportsNewsAPI.Models;

namespace SportsNewsAPI.Services
{
    public class SportsNewsService
    {
        private readonly IMongoCollection<SportsNews> _newsCollection;
        private const int MAX_LIMIT = 1000;

        public SportsNewsService(IOptions<SportsNewsDatabaseSettings> sportsNewsDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                sportsNewsDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                sportsNewsDatabaseSettings.Value.DatabaseName);

            _newsCollection = mongoDatabase.GetCollection<SportsNews>(
                sportsNewsDatabaseSettings.Value.NewsCollectionName);
        }

        // Получение
        public async Task<SportsNews?> GetAsync(string id) =>
            await _newsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<List<SportsNews>> GetAllAsync()
        {
            var totalCount = await _newsCollection.CountDocumentsAsync(_ => true);

            if (totalCount <= MAX_LIMIT) {
                return await _newsCollection.Find(_ => true).ToListAsync();
            }

            // Иначе случайная выборка
            var randomSample = await _newsCollection.Aggregate().Sample(MAX_LIMIT).ToListAsync();

            return randomSample;
        }

        public async Task<List<SportsNews>> FilterAsync(FilterConfig filterConfig)
        {
            var filter = filterConfig.returnFilter();

            return await _newsCollection
                .Find(filter)
                .SortByDescending(x => x.Date)
                .ToListAsync();
        }

        // Создание
        public async Task CreateAsync(SportsNews newNews) =>
            await _newsCollection.InsertOneAsync(newNews);

        public async Task CreateManyAsync(List<SportsNews> newsList) =>
            await _newsCollection.InsertManyAsync(newsList);

        // Обновление
        public async Task UpdateAsync(string id, SportsNews updatedNews) =>
            await _newsCollection.ReplaceOneAsync(x => x.Id == id, updatedNews);

        // Удаление
        public async Task RemoveAsync(string id) =>
            await _newsCollection.DeleteOneAsync(x => x.Id == id);

        public async Task RemoveAllAsync() =>
            await _newsCollection.DeleteManyAsync(_ => true);

        // Получение количества элементов
        public async Task<long> CountAsync() =>
            await _newsCollection.CountDocumentsAsync(_ => true);
    }
}
