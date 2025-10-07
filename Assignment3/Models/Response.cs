namespace Assignment3.Models
{
    public class Response
    {
        public string Status { get; set; } //"1 ok" eller fejl
        public string Reason { get; set; } //"Missing field x" eller lign.
        public string Body { get; set; } //JSON streng med data 
    }
}