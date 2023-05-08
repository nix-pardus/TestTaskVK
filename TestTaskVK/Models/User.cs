using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Serialization;

namespace TestTaskVK.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public DateTime CreatedDate { get; set; }
        public UserGroup? UserGroup { get; set; }
        public UserState? UserState { get; set; }
    }
}
