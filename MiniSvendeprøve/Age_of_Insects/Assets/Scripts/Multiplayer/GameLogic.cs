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

    public static void AddToLobby(ushort id, string username, int lobbyId, bool isReady)
    {
        if (!ControllerScript.Singleton.GetPlayerClassList().ContainsKey(id))
        {
            Player player = new Player();
            player.IsReady = isReady;
            player.Id = id;
            player.Username = username;
            player.LobbyId = lobbyId;
            if (id == NetworkManager.Singleton.Client.Id)
                player.IsLocal = true;
            else
                player.IsLocal = false;

            ControllerScript.Singleton.PostPlayerClass(id, player);
        }
        
        UIManager.Singleton.UpdateLobbyPlayers(ControllerScript.Singleton.GetPlayerClassList());
    }

    public void ReturnToLobby()
    {
        UIManager.Singleton.BackToLobby();
    }

    public void RemoveFromLobby(ushort id)
    {
        ControllerScript.Singleton.DeletePlayer(id);
        UIManager.Singleton.UpdateLobbyPlayers(ControllerScript.Singleton.GetPlayerClassList());
        if (GameHandlerScript.Singleton.GameIsStarted)
            ReturnToLobby();
    }

    [MessageHandler((ushort)ServerToClientId.playerConnected)]
    private static void ConnectPlayer(Message message)
    {
        AddToLobby(message.GetUShort(), message.GetString(), message.GetInt(), message.GetBool());
    }

    [MessageHandler((ushort)ServerToClientId.playerReady)]
    private static void ReadyPlayer(Message message)
    {
        Player player = ControllerScript.Singleton.GetPlayerClass(message.GetUShort());
        player.IsReady = message.GetBool();
        ControllerScript.Singleton.PatchPlayer(player);
        UIManager.Singleton.UpdateLobbyPlayers(ControllerScript.Singleton.GetPlayerClassList());
    }

    [MessageHandler((ushort)ServerToClientId.startGame)]
    private static void StartGame(Message message)
    {
        if (message.GetBool())
            GameHandlerScript.Singleton.StartGame(ControllerScript.Singleton.GetPlayerClassList());
        UIManager.Singleton.UpdateLobbyPlayers(ControllerScript.Singleton.GetPlayerClassList());
    }

    [MessageHandler((ushort)ServerToClientId.playerRemoved)]
    private static void PlayerRemoved(Message message)
    {
        ControllerScript.Singleton.DeletePlayerList();
        UIManager.Singleton.UpdateLobbyPlayers(ControllerScript.Singleton.GetPlayerClassList());
    }

    [MessageHandler((ushort)ServerToClientId.spawnUnit)]
    private static void SpawnUnit(Message message)
    {
        GameHandlerScript.Singleton.SpawnUnit(message.GetUShort(), message.GetInt());
    }

    [MessageHandler((ushort)ServerToClientId.runCivilizationAction)]
    private static void RunCivilizationAction(Message message)
    {
        GameHandlerScript.Singleton.RunPlayerCivilizationAction(message.GetUShort(), message.GetInt());
    }
}
