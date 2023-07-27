using Bp_back.Models.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bp_back.Models.Buisness
{
    public class Hub
    {
        [Key]
        public Guid Id { get; set; }
        public string Url { get; set; }
        public int Port { get; set; }
        public string Name { get; set; }
        public DateTime Added { get; set; }
        public User User { get; set; }
        [NotMapped]
        public IEnumerable<Server>? Servers { get; set; }
        [NotMapped]
        public bool Active { get; set; }
    }
}
