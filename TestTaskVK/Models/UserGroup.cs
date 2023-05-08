using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestTaskVK.Models
{
    public enum CodeGroup
    {
        Admin = 100,
        User = 200
    }
    public class UserGroup
    {
        public int Id { get; set; }
        public CodeGroup Code { get; set; }
        
        public string? Description { get; set; }
    }
}
