namespace Assignment3.Models
{
    public class Request
    {
        public string Path { get; set; } //"/categories" eller "/categories/{id}"
        public string Date { get; set; } //Unix timestamp
        public string? Method { get; set; } //"GET", "POST", "PUT", "DELETE"
        public string? Body { get; set; } //JSON body som string
    }
}