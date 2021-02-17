namespace Login.Models
{
    public class Report
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        //ThreadId
        public Thread Thread { get; set; }
    }
}