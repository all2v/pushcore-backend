namespace Astrum.Debts.Application.Models.ViewModels
{
    public class UserDebt
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string NameWithSurname
        {
            get
            {
                return $"{Name} {Surname}";
            }
        }
        public string? RequisiteBank { get; set; }
        public string? RequisiteNumberPhone { get; set; }
    }
}
