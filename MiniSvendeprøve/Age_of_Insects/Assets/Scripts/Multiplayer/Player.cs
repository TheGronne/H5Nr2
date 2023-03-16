using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public ushort Id { get; set; }
    public bool IsLocal { get; set; }
    public bool IsReady { get; set; }
    public string Username { get; set; }
    public int LobbyId { get; set; }
}
