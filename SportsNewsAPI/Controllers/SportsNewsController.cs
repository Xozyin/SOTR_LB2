using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SportsNewsAPI.Models;
using SportsNewsAPI.Services;

namespace SportsNewsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SportsNewsController : ControllerBase
    {
        private readonly SportsNewsService _sportsNewsService;

        public SportsNewsController(SportsNewsService newsService) =>
            _sportsNewsService = newsService;

        // Получение всех объектов

        [HttpGet]
        public async Task<ActionResult<List<SportsNewsDto>>> Get()
        {
            var news = await _sportsNewsService.GetAllAsync();

            var newsDtos = news.Select(n => new SportsNewsDto
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content,
                Date = DateOnly.FromDateTime(n.Date),
                Source = n.Source
            }).ToList();

            return Ok(newsDtos);
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<SportsNewsDto>> Get(string id)
        {
            var news = await _sportsNewsService.GetAsync(id);

            if (news is null)
            {
                return NotFound($"Новость с ID {id} не найдена!");
            }

            var newsDto = new SportsNewsDto
            {
                Id = news.Id,
                Title = news.Title,
                Content = news.Content,
                Date = DateOnly.FromDateTime(news.Date),
                Source = news.Source
            };

            return newsDto;
        }

        // Фильтрация
        [HttpGet("filter")]
        public async Task<ActionResult<List<SportsNewsDto>>> Filter(
            [FromQuery] string? titleContains,
            [FromQuery] string? contentContains,
            [FromQuery] string? source,
            [FromQuery] DateOnly? fromDate,
            [FromQuery] DateOnly? toDate
            )
        {
            var filter = new FilterConfig(titleContains, contentContains, source, fromDate, toDate);

            var news = await _sportsNewsService.FilterAsync(filter);

            var newsDtos = news.Select(n => new SportsNewsDto
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content,
                Date = DateOnly.FromDateTime(n.Date),
                Source = n.Source
            }).ToList();

            return Ok(newsDtos);
        }

        [HttpPost]
        public async Task<IActionResult> Post(SportsNewsDtoWithoutID newNews)
        {
            var dateTime = newNews.Date.ToDateTime(TimeOnly.MinValue);

            var news = new SportsNews
            {
                Title = newNews.Title,
                Content = newNews.Content,
                Date = dateTime,
                Source = newNews.Source
            };

            await _sportsNewsService.CreateAsync(news);

            var newsDto = new SportsNewsDto
            {
                Id = news.Id,
                Title = news.Title,
                Content = news.Content,
                Date = DateOnly.FromDateTime(news.Date),
                Source = news.Source
            };

            return CreatedAtAction(nameof(Get), new { id = news.Id }, newsDto);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<ActionResult> Update(string id, SportsNewsDtoWithoutID updateDto)
        {
            var news = await _sportsNewsService.GetAsync(id);

            if (news is null)
            {
                return NotFound();
            }

            var dateTime = updateDto.Date.ToDateTime(TimeOnly.MinValue);

            var updatedNews = new SportsNews
            {
                Id = id,
                Title = updateDto.Title,
                Content = updateDto.Content,
                Date = dateTime,
                Source = updateDto.Source
            };

            await _sportsNewsService.UpdateAsync(id, updatedNews);

            return Ok();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<ActionResult> Delete(string id)
        {
            var news = await _sportsNewsService.GetAsync(id);

            if (news is null)
            {
                return NotFound($"Новость с ID {id} не найдена!");
            }

            await _sportsNewsService.RemoveAsync(id);

            return Ok();
        }

        // Получение кол-ва элементов
        [HttpGet("count")]
        public async Task<ActionResult<long>> Count()
        {
            var count = await _sportsNewsService.CountAsync();
            return Ok(count);
        }

        // Удаление всех элементов
        [HttpDelete]
        public async Task<ActionResult> DeleteAll()
        {
            await _sportsNewsService.RemoveAllAsync();
            return Ok();
        }
    }
}
