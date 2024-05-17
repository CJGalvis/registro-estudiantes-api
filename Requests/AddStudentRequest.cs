namespace StudentsApi.Requests
{
    public class AddStudentRequest
    {
        public string Dni { get; set; }
        public string Name { get; set; }
        public IEnumerable<int> SubjectIds { get; set; }
    }
}
