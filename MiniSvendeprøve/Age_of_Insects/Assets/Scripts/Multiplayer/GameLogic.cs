using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    private static GameLogic _singleton;

    public static GameLogic Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{ nameof(GameLogic)} instance already exists, destroying duplicate");
                Destroy(value);
            }
        }
    }

    private void Awake()
    {
        Singleton = this;
    }


    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();

    public static void AddToLobby(ushort id, string username, int lobbyId)
    {
        if (!list.ContainsKey(id))
        {
            Player player = new Player();
            player.IsReady = false;
            player.Id = id;
            player.Username = username;
            player.LobbyId = lobbyId;

            list.Add(id, player);
        }
        
        UIManager.Singleton.UpdateLobbyPlayers(list);
    }

    public void RemoveFromLobby(ushort id)
    {
        list.Remove(id);
        UIManager.Singleton.UpdateLobbyPlayers(list);
    }

    [MessageHandler((ushort)ServerToClientId.playerConnected)]
    private static void ConnectPlayer(Message message)
    {
        AddToLobby(message.GetUShort(), message.GetString(), message.GetInt());
    }

    [MessageHandler((ushort)ServerToClientId.playerReady)]
    private static void ReadyPlayer(Message message)
    {
        list[message.GetUShort()].IsReady = message.GetBool();
        UIManager.Singleton.UpdateLobbyPlayers(list);
    }

    [MessageHandler((ushort)ServerToClientId.startGame)]
    private static void StartGame(Message message)
    {
        if (message.GetBool())
            GameObject.Find("GameHandler").GetComponent<GameHandlerScript>().StartGame();
        UIManager.Singleton.UpdateLobbyPlayers(list);
    }

    [MessageHandler((ushort)ServerToClientId.playerRemoved)]
    private static void PlayerRemoved(Message message)
    {
        list.Clear();
        UIManager.Singleton.UpdateLobbyPlayers(list);
    }
}
