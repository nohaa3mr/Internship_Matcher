namespace InternshipMatcher.API.MinimalAPIs
{
    public class Paged<T> where T : class
    {
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; } 
        public int TotalPages { get; set; }
        public T Items { get; set; }


    }
}