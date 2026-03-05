using MongoDB.Bson;
using MongoDB.Driver;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SportsNewsAPI.Models
{
    public class SportsNewsDto
    {
        public string? Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public string Source { get; set; } = string.Empty;
    }

    public class SportsNewsDtoWithoutID
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public string Source { get; set; } = string.Empty;
    }

    public class FilterConfig
    {
        private FilterDefinition<SportsNews> _filter;
        public FilterConfig(
            string? titleContains = null,
            string? contentContains = null,
            string? source = null,
            DateOnly? fromDate = null,
            DateOnly? toDate = null)
        {
            var filterBuilder = Builders<SportsNews>.Filter;
            _filter = Builders<SportsNews>.Filter.Empty;

            if (!string.IsNullOrEmpty(titleContains))
            {
                _filter &= filterBuilder.Regex(x => x.Title, new BsonRegularExpression(titleContains, "i"));
            }

            if (!string.IsNullOrEmpty(contentContains))
            {
                _filter &= filterBuilder.Regex(x => x.Content, new BsonRegularExpression(contentContains, "i"));
            }

            if (!string.IsNullOrEmpty(source))
            {
                _filter &= filterBuilder.Eq(x => x.Source, source);
            }

            if (fromDate.HasValue)
            {
                _filter &= filterBuilder.Gte(x => x.Date, fromDate.Value.ToDateTime(TimeOnly.MinValue));
            }

            if (toDate.HasValue)
            {
                _filter &= filterBuilder.Lte(x => x.Date, toDate.Value.ToDateTime(TimeOnly.MaxValue));
            }
        }

        public FilterDefinition<SportsNews> returnFilter() => _filter;
    }
}
