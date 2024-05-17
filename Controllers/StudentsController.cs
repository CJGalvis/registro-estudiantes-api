using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentsApi.Context;
using StudentsApi.Models;
using StudentsApi.Requests;
using StudentsApi.Responses;

namespace StudentsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{dni:required}")]
        public async Task<ActionResult<StudentsResponse>> GetStudent(string dni)
        {
            var student = await _context.Student
                                        .Include(t => t.Subjects)
                                        .ThenInclude(t => t.Teacher)
                                        .FirstOrDefaultAsync(c => c.Dni == dni);
            if (student == null)
            {
                return NotFound();
            }

            return new StudentsResponse
            {
                Dni = student.Dni,
                Name = student.Name,
                Subjects = student.Subjects.Select(s => new SubjectResponse
                {
                    Id = s.Id,
                    Name = s.Name,
                    Teacher = s.Teacher.Id
                })
            };
        }

        [HttpGet("{dni:required}/classmates")]
        public async Task<ActionResult<IEnumerable<StudentsResponse>>> GetClassmates(string dni)
        {
            var student = await _context.Student
                                        .Include(t => t.Subjects)
                                        .FirstOrDefaultAsync(c => c.Dni == dni);
            if (student == null)
            {
                return NotFound();
            }

            var subjectIds = student.Subjects.Select(t => t.Id);

            var classmates = await _context.Student
                                    .Include(t => t.Subjects)
                                    .Where(t => t.Id != student.Id)
                                    .Where(t => _context.Subject
                                                        .Where(s => s.Students.Any(sId => sId.Id == t.Id))
                                                        .Any(s => subjectIds.Any(sId => sId == s.Id)))
                                    .ToListAsync();

            var result = classmates.Select(classmate => new StudentsResponse
            {
                Dni = classmate.Dni,
                Name = classmate.Name,
                Subjects = classmate.Subjects.Select(s => new SubjectResponse
                {
                    Id = s.Id,
                    Name = s.Name,
                })
            });
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(AddStudentRequest request)
        {
            var student = new Student
            {
                Dni = request.Dni,
                Name = request.Name
            };
            _context.Student.Add(student);

            var subjects = _context.Subject.Where(s => request.SubjectIds.Any(sId => s.Id == sId));
            foreach (var subject in subjects)
            {
                student.Subjects.Add(subject);
                subject.Students.Add(student);
            }

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}