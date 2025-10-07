using System.Text.Json.Serialization;

namespace Assignment3.Models
{
    public class Category
    {
        public int Cid { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public int Id //alias for at opfylde tests, som forventer id
        {
            get => Cid;
            set => Cid = value;
        }

        public Category(int cid, string name)
        {
            Cid = cid;
            Name = name;
        }
    }
}