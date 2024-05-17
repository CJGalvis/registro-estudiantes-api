namespace StudentsApi.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string? Dni {  get; set; }
        public string? Name {  get; set; }

        public ICollection<Subject> Subjects {  get; } = new List<Subject>();
    }
}
