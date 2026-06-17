namespace BLL.ViewModels
{
    public class EditUserRolesVM
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> CurrentRoles { get; set; }
        public List<string> AllRoles { get; set; }
    }
}
