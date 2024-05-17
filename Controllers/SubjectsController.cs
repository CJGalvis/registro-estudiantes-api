using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentsApi.Context;
using StudentsApi.Models;
using StudentsApi.Responses;

namespace StudentsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SubjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<SubjectResponse>> GetSubject()
        {
            var response = await _context.Subject.Include(t => t.Teacher).ToListAsync();
            return response.Select(s => new SubjectResponse
            {
                Id = s.Id,
                Name = s.Name,
                Teacher = s.Teacher.Id
            });
        }
    }
}