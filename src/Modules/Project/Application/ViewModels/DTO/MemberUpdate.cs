namespace Astrum.Project.ViewModels.DTO
{
    public class MemberUpdate
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string AvatarUrl { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsManager { get; set; }
        public string Role { get; set; }
    }
}
