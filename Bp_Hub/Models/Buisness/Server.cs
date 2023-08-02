namespace Bp_Hub.Models.Buisness
{
    public class Server
    {
        public int Id { get; set; }
        public int Port { get; set; }
        public int HttpPort { get; set; }
        public int PlayersCount { get; set; }
        public bool Active { get; set; }
    }
}
