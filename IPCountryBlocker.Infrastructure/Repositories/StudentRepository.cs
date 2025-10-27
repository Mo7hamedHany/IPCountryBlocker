using IPCountryBlocker.Domain.Interfaces;
using IPCountryBlocker.Domain.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace IPCountryBlocker.Infrastructure.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly IDatabase _database;
        private readonly IServer _server;
        private const string StudentKeyPrefix = "student:";

        public StudentRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
            _server = connection.GetServer(connection.GetEndPoints().First());
        }

        public async Task<bool> AddStudentAsync(Student student)
        {
            var studentJson = System.Text.Json.JsonSerializer.Serialize(student);

            var result = await _database.StringSetAsync($"{StudentKeyPrefix}{student.Id}", studentJson);

            return result;
        }

        public async Task<Student?> GetStudentById(int studentId)
        {
            var studentJson = await _database.StringGetAsync($"{StudentKeyPrefix}{studentId}");

            if (studentJson.IsNullOrEmpty)
                return null;

            var student = JsonSerializer.Deserialize<Student>(studentJson);

            return student;
        }

        public async Task<PagedResult<Student>> GetStudentsAsync(string? searchItem = null, int pageNumber = 1, int pageSize = 10)
        {
            var students = new List<Student>();

            var keys = _server.Keys(pattern: $"{StudentKeyPrefix}*").ToList();

            foreach (var key in keys)
            {
                var studentJson = await _database.StringGetAsync(key);

                if (studentJson.IsNullOrEmpty) continue;

                var student = JsonSerializer.Deserialize<Student>(studentJson);
                if (student != null)
                {
                    students.Add(student);
                }
            }


            if (!string.IsNullOrWhiteSpace(searchItem))
            {
                students = students.Where(s =>
                    (s.FirstName != null && s.FirstName.Contains(searchItem, StringComparison.OrdinalIgnoreCase)) ||
                    (s.LastName != null && s.LastName.Contains(searchItem, StringComparison.OrdinalIgnoreCase)) ||
                    s.Id.ToString().Contains(searchItem)
                ).ToList();
            }


            var totalCount = students.Count;


            var pagedStudents = students
                .OrderBy(s => s.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<Student>
            {
                Items = pagedStudents,
                TotalCount = totalCount
            };
        }

        public async Task<bool> UpdateStudentAsync(Student student)
        {
            var existingStudentJson = await _database.StringGetAsync($"{StudentKeyPrefix}{student.Id}");
            if (existingStudentJson.IsNullOrEmpty)
                return false;
            var studentJson = JsonSerializer.Serialize(student);

            var result = await _database.StringSetAsync($"{StudentKeyPrefix}{student.Id}", studentJson);

            return result;
        }

        public async Task<bool> DeleteStudentAsync(int studentId)
        {
            var deleted = await _database.KeyDeleteAsync($"{StudentKeyPrefix}{studentId}");

            return deleted;
        }
    }
}
