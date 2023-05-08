namespace TestTaskVK.Models
{
    public enum CodeState
    {
        Active = 100,
        Blocked = 200
    }
    public class UserState
    {
        public UserState()
        {
            Code = CodeState.Active;
        }

        public int Id { get; set; }
        public CodeState Code { get; set; }
        public string? Description { get; set; }
    }
}
