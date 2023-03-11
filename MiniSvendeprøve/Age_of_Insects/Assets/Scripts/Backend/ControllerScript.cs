using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerScript : MonoBehaviour
{
    private DataHandlerScript DataHandler;
    private GameHandlerScript GameHandler;
    private GameObject Shop;
    private ShopScript ShopScript;

    
    // Start is called before the first frame update
    void Start()
    {
        DataHandler = GameObject.Find("DataHandler").GetComponent<DataHandlerScript>();
        GameHandler = GameObject.Find("GameHandler").GetComponent<GameHandlerScript>();
        Shop = GameObject.Find("Shop");
        ShopScript = Shop.GetComponent<ShopScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RunPlayerCivilizationAction(int id)
    {
        DataHandler.RunCivilizationAction(id, GetPlayerSummonerMain());
        ShopScript.SetupNewShop(DataHandler.UnitShops[id], DataHandler.TurretShops[id], DataHandler.CivilizationShops[id]);
        GetPlayerSummonerMain().civilizationId = id;
    }
    public void RunEnemyCivilizationAction(int id)
    {
        DataHandler.RunCivilizationAction(id, GetEnemySummonerMain());
        ShopScript.SetupNewShop(DataHandler.UnitShops[id], DataHandler.TurretShops[id], DataHandler.CivilizationShops[id]);
        GetEnemySummonerMain().civilizationId = id;
    }

    // GET methods
    public GameObject GetPlayer()
    {
        return DataHandler.player;
    }
    public GameObject GetEnemy()
    {
        return DataHandler.enemy;
    }
    public GameObject GetUnit(int id)
    {
        return DataHandler.units[id];
    }
    public GameObject GetTurret(int id)
    {
        return DataHandler.turrets[id];
    }
    public GameObject GetCivilization(int id)
    {
        return DataHandler.civilizations[id];
    }
    public GameObject GetPlayerUnit(int id)
    {
        return DataHandler.playerUnits[id];
    }
    public GameObject GetEnemyUnit(int id)
    {
        return DataHandler.enemyUnits[id];
    }
    public List<GameObject> GetPlayerUnitList()
    {
        return DataHandler.playerUnits;
    }
    public List<GameObject> GetEnemyUnitList()
    {
        return DataHandler.enemyUnits;
    }
    public List<GameObject> GetPlayerUnitQueue()
    {
        return DataHandler.playerUnitQueue;
    }
    public List<GameObject> GetEnemyUnitQueue()
    {
        return DataHandler.enemyUnitQueue;
    }
    public GameObject GetPlayerUnitQueueUnit(int id)
    {
        return DataHandler.playerUnitQueue[id];
    }
    public GameObject GetEnemyUnitQueueUnit(int id)
    {
        return DataHandler.enemyUnitQueue[id];
    }
    public SummonerMain GetPlayerSummonerMain()
    {
        return DataHandler.player.GetComponent<SummonerMain>();
    }
    public SummonerMain GetEnemySummonerMain()
    {
        return DataHandler.enemy.GetComponent<SummonerMain>();
    }
    public UnitMain GetPlayerUnitUnitMain(int id)
    {
        return GetPlayerUnit(id).GetComponent<UnitMain>();
    }
    public UnitMain GetEnemyUnitUnitMain(int id)
    {
        return GetEnemyUnit(id).GetComponent<UnitMain>();
    }

    // DELETE methods
    public void DeletePlayerUnit(GameObject unit)
    {
        DataHandler.playerUnits.Remove(unit);
    }
    public void DeleteEnemyUnit(GameObject unit)
    {
        DataHandler.enemyUnits.Remove(unit);
    }
    public void DeletePlayerUnitQueueUnit(int id)
    {
        DataHandler.playerUnitQueue.RemoveAt(id);
    }
    public void DeleteEnemyUnitQueueUnit(int id)
    {
        DataHandler.enemyUnitQueue.RemoveAt(id);
    }


    // POST methods
    public void PostPlayerUnit(GameObject unit)
    {
        DataHandler.playerUnits.Add(unit);
    }
    public void PostEnemyUnit(GameObject unit)
    {
        DataHandler.enemyUnits.Add(unit);
    }
    public void PostUnitToPlayerQueue(GameObject unit)
    {
        DataHandler.playerUnitQueue.Add(unit);
    }
    public void PostUnitToEnemyQueue(GameObject unit)
    {
        DataHandler.enemyUnitQueue.Add(unit);
    }
}
