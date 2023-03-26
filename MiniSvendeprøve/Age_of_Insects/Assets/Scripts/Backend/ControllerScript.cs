using RiptideNetworking;
using RiptideNetworking.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerScript : MonoBehaviour
{
    private static ControllerScript _singleton;

    public static ControllerScript Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{ nameof(ControllerScript)} instance already exists, destroying duplicate");
                Destroy(value);
            }
        }
    }

    private void Awake()
    {
        Singleton = this;
    }

    private GameObject Shop;
    private ShopScript ShopScript;

    // Start is called before the first frame update
    void Start()
    {
        Shop = GameObject.Find("Shop");
        ShopScript = Shop.GetComponent<ShopScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RunPlayerCivilizationAction(ushort playerId, int civId)
    {
        DataHandlerScript.Singleton.RunCivilizationAction(civId, GetPlayerObject(playerId));
        GetPlayerSummonerMain(playerId).civilizationId = civId;
        if (playerId == GetLocalPlayerId())
            ShopScript.SetupNewShop(DataHandlerScript.Singleton.UnitShops[civId], DataHandlerScript.Singleton.CivilizationShops[civId]);
    }

    // GET methods
    public GameObject GetPlayerObject(ushort id)
    {
        return DataHandlerScript.Singleton.playerGameObjectList[id];
    }
    public Player GetPlayerClass(ushort id)
    {
        return DataHandlerScript.Singleton.playerClassList[id];
    }
    public GameObject GetUnit(int id)
    {
        return DataHandlerScript.Singleton.units[id];
    }
    public GameObject GetCivilization(int id)
    {
        return DataHandlerScript.Singleton.civilizations[id];
    }
    public GameObject GetPlayerUnit(ushort playerId, int index)
    {
        return DataHandlerScript.Singleton.playerUnits[playerId][index];
    }
    public List<GameObject> GetPlayerUnitList(ushort playerId)
    {
        return DataHandlerScript.Singleton.playerUnits[playerId];
    }
    public SummonerMain GetPlayerSummonerMain(ushort id)
    {
        return DataHandlerScript.Singleton.playerGameObjectList[id].GetComponent<SummonerMain>();
    }
    public UnitMain GetPlayerUnitUnitMain(ushort playerId, int id)
    {
        return GetPlayerUnit(playerId, id).GetComponent<UnitMain>();
    }
    public Dictionary<ushort, GameObject> GetPlayerObjectList()
    {
        return DataHandlerScript.Singleton.playerGameObjectList;
    }
    public Dictionary<ushort, Player> GetPlayerClassList()
    {
        return DataHandlerScript.Singleton.playerClassList;
    }
    public ushort GetLocalPlayerId()
    {
        ushort localId = new ushort();
        foreach (var player in DataHandlerScript.Singleton.playerClassList)
        {
            if (player.Value.IsLocal == true)
                localId = player.Value.Id;
        }
        return localId;
    }

    // DELETE methods
    public void DeletePlayerUnit(ushort playerId, int index)
    {
        DataHandlerScript.Singleton.playerUnits[playerId].RemoveAt(index);
    }
    public void DeletePlayer(ushort id)
    {
        DataHandlerScript.Singleton.playerClassList.Remove(id);
    }
    public void DeletePlayerList()
    {
        DataHandlerScript.Singleton.playerClassList.Clear();
    }

    // POST methods
    public void PostPlayerUnit(ushort playerId, GameObject unit)
    {
        DataHandlerScript.Singleton.playerUnits[playerId].Add(unit);
    }
    public void PostPlayerClass(ushort id, Player player)
    {
        DataHandlerScript.Singleton.playerClassList.Add(id, player);
    }
    public void PostPlayerObject(ushort id, GameObject player)
    {
        DataHandlerScript.Singleton.playerGameObjectList.Add(id, player);
        DataHandlerScript.Singleton.playerUnits.Add(id, new List<GameObject>());
    }

    //PATH methods
    public void PatchPlayerName(ushort id, string name)
    {
        DataHandlerScript.Singleton.playerGameObjectList[id].GetComponent<SummonerMain>().username = name;
    }
    public void PatchPlayer(Player player)
    {
        DataHandlerScript.Singleton.playerClassList[player.Id] = player;
    }


    // Multiplayer messages
    public void SendReady()
    {
        Message message = Message.Create(MessageSendMode.reliable, (ushort)ClientToServerId.ready);
        NetworkManager.Singleton.Client.Send(message);
    }

    public void SendConnect(string username)
    {
        //Reliable means making sure package is received.
        Message message = Message.Create(MessageSendMode.reliable, (ushort)ClientToServerId.connected);
        message.AddString(username);
        NetworkManager.Singleton.Client.Send(message);
    }

    public void SendStartGameRequest()
    {
        Message message = Message.Create(MessageSendMode.reliable, (ushort)ClientToServerId.startGame);
        NetworkManager.Singleton.Client.Send(message);
    }

    public void SendUnitPurchase(int unitId)
    {
        float spawnTimer = Singleton.GetUnit(unitId).GetComponent<UnitMain>().summonTime;

        Message message = Message.Create(MessageSendMode.reliable, (ushort)ClientToServerId.unitPurchase);
        message.AddInt(unitId);
        message.AddFloat(spawnTimer);
        NetworkManager.Singleton.Client.Send(message);
    }

    public void SendCivilizationPurchase(int civId)
    {
        Message message = Message.Create(MessageSendMode.reliable, (ushort)ClientToServerId.civilizationPurchase);
        message.AddInt(civId);
        NetworkManager.Singleton.Client.Send(message);
    }
}
