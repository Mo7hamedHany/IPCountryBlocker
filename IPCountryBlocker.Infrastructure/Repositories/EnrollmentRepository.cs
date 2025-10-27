using IPCountryBlocker.Domain.Interfaces;
using IPCountryBlocker.Domain.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace IPCountryBlocker.Infrastructure.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly IDatabase _database;
        private readonly IServer _server;
        private const string EnrollmentKeyPrefix = "enrollment:";

        public EnrollmentRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
            _server = connection.GetServer(connection.GetEndPoints().First());
        }

        public async Task<bool> EnrollStudentInClassAsync(Enrollment enrollment)
        {
            enrollment.EnrollmentDate = DateTime.UtcNow;

            var enrollmentJson = System.Text.Json.JsonSerializer.Serialize(enrollment);

            var result = await _database.StringSetAsync($"{EnrollmentKeyPrefix}{enrollment.Id}", enrollmentJson);

            return result;
        }

        public async Task<bool> IsAlreadyEnrolled(int studentId, int classId)
        {
            var keys = _server.Keys(pattern: $"{EnrollmentKeyPrefix}*").ToList();

            foreach (var key in keys)
            {
                var enrollmentJson = await _database.StringGetAsync(key);

                if (enrollmentJson.IsNullOrEmpty) continue;

                var enrollment = JsonSerializer.Deserialize<Enrollment>(enrollmentJson);

                if (enrollment != null && enrollment.StudentId == studentId && enrollment.ClassId == classId)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<List<int>> GetClassesByStudentIdAsync(int studentId)
        {
            var classIds = new List<int>();
            var keys = _server.Keys(pattern: $"{EnrollmentKeyPrefix}*").ToList();

            foreach (var key in keys)
            {
                var enrollmentJson = await _database.StringGetAsync(key);

                if (enrollmentJson.IsNullOrEmpty) continue;

                var enrollment = JsonSerializer.Deserialize<Enrollment>(enrollmentJson);

                if (enrollment != null && enrollment.StudentId == studentId)
                {
                    classIds.Add(enrollment.ClassId);
                }
            }

            return classIds;
        }


        public async Task<Enrollment> GetEnrollmentByIdAsync(int id)
        {
            var enrollmentJson = await _database.StringGetAsync($"{EnrollmentKeyPrefix}{id}");

            if (enrollmentJson.IsNullOrEmpty)
                return null;

            var enrollment = JsonSerializer.Deserialize<Enrollment>(enrollmentJson);

            return enrollment;
        }
    }
}
