namespace MellemOs.Models
{
    public class Role
    {
        public required RoleEnum RoleTitle { get; set; }
        public int AbilityCooldown { get; set; }
    }
}
