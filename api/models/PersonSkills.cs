using System.Text.Json.Serialization;

namespace BaseSpace.Models
{
    public class Person
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public ICollection<Skill> Skills { get; set; } = new List<Skill>();
    }
    public class Skill
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Name { get; set; }

        public byte Level { get; set; }

        [JsonIgnore]
        public long PersonId { get; set; }

        [JsonIgnore]
        public Person Person { get; set; }

    }
}
