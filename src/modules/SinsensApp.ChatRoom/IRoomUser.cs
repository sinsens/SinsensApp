namespace SinsensApp.ChatRoom
{
    public interface IRoomUser
    {
        string Id { get; set; }
        string Name { get; set; }
        bool Online { get; set; }
    }
}