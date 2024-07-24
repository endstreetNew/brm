namespace Sassa.BRM.Services
{
    public class ActiveUser
    {
        public string Name { get; set; } = Guid.NewGuid().ToString();
    }
    public class ActiveUserList
    {
        public List<ActiveUser> Users { get; set; } = new();
    }
}
