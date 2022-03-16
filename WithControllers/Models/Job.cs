namespace MM.Models
{
    public class Job
    {
        public Job()
        {
        }

        public Job(JobDTO jobdto)
        {
            Customer=jobdto.Customer;
            StartDate=jobdto.StartDate;
            Days=jobdto.Days;
            Location=jobdto.Location;
            Comments = jobdto.Comments;
            Models = jobdto.Models;
            Expenses = null;
        }
        public long JobId { get; set; }
        //[MaxLength(64)]
        public string? Customer { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public int Days { get; set; }
        //[MaxLength(128)]
        public string? Location { get; set; }
        //[MaxLength(2000)]
        public string? Comments { get; set; }
        public List<Model>? Models { get; set; }
        public List<Expense>? Expenses { get; set; }
    }

}
