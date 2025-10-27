using IPCountryBlocker.Domain.Interfaces;
using IPCountryBlocker.Domain.Models;
using StackExchange.Redis;

namespace IPCountryBlocker.Infrastructure.Repositories
{
    public class MarkRepository : IMarkRepository
    {
        private readonly IDatabase _database;
        private readonly IServer _server;
        private const string MarkKeyPrefix = "mark:";

        public MarkRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
            _server = connection.GetServer(connection.GetEndPoints().First());
        }

        public async Task<bool> AddMarkAsync(Mark mark)
        {
            var markJson = System.Text.Json.JsonSerializer.Serialize(mark);

            if (markJson is null)
                return false;

            return await _database.StringSetAsync($"{MarkKeyPrefix}{mark.Id}", markJson);
        }

        public async Task<IEnumerable<Mark>> GetMarksByClassIdAsync(int classId)
        {
            var marks = new List<Mark>();
            var keys = _server.Keys(pattern: $"{MarkKeyPrefix}*").ToArray();
            foreach (var key in keys)
            {
                var markJson = await _database.StringGetAsync(key);
                if (!markJson.IsNullOrEmpty)
                {
                    var mark = System.Text.Json.JsonSerializer.Deserialize<Mark>(markJson);
                    if (mark != null && mark.ClassId == classId)
                    {
                        marks.Add(mark);
                    }
                }
            }
            return marks;

        }

        public async Task<Mark?> GetMarksOfStudentForClassIdAsync(int studentId, int classId)
        {
            var keys = _server.Keys(pattern: $"{MarkKeyPrefix}*").ToArray();
            foreach (var key in keys)
            {
                var markJson = await _database.StringGetAsync(key);
                if (!markJson.IsNullOrEmpty)
                {
                    var mark = System.Text.Json.JsonSerializer.Deserialize<Mark>(markJson);
                    if (mark != null && mark.StudentId == studentId && mark.ClassId == classId)
                    {
                        return mark;
                    }
                }
            }

            return null;
        }
    }
}
