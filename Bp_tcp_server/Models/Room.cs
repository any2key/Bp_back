using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Bp_tcp_server.Server.BpServer;

public class Room
{
    internal Guid Id { get; set; }
    public int MaxPlayers { get; set; }
    public RoomData Data { get; set; }
    public List<Player> Players { get; set; }
    public int State { get; set; }

    public void AddPlayer(Player player) { }
    public void RemovePlayer(string connectionId) { }
}

public class RoomData
{
    public int Type { get; set; }
    public int PlaneType { get; set; }

}

public class Player
{
    public Connection Connection { get; set; }
    public PlayerData Data { get; set; }
}
public class PlayerData
{
    public string Plane { get; set; }
}

