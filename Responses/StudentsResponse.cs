namespace StudentsApi.Responses
{
    public class StudentsResponse
    {
        public int Id { get; set; }
        public string? Dni { get; set; }
        public string? Name { get; set; }
        public IEnumerable<SubjectResponse> Subjects { get; set; }
    }
}
