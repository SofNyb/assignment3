using System.Linq;

namespace Assignment3.Utils
{
    public class UrlParser
    {
        public bool HasId { get; set; }
        public string Id { get; set; }
        public string Path { get; set; }

        public bool ParseUrl(string url)
        {
            if (string.IsNullOrEmpty(url)) return false;

            // Fjern indledende /-tegn fra path
            url = url.TrimStart('/');

            // Split path op
            var parts = url.Split('/');

            if (parts.Length == 0) return false;

            // Tjek om sidste del kan passes til id
            if (int.TryParse(parts.Last(), out int id))
            {
                HasId = true;
                Id = id.ToString();
                Path = "/" + string.Join("/", parts.Take(parts.Length - 1).ToArray());

            }
            else
            {
                HasId = false;
                Id = "";
                Path = "/" +string.Join("/", parts);
            }

            return true;
        }
    }
}