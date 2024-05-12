namespace Astrum.Inventory.Application.Models
{
    public class UserInventory
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
    }
}
