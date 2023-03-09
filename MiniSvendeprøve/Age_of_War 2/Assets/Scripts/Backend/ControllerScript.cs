using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerScript : MonoBehaviour
{
    private DataHandlerScript DataHandler;
    private GameObject Shop;
    // Start is called before the first frame update
    void Start()
    {
        DataHandler = GameObject.Find("DataHandler").GetComponent<DataHandlerScript>();
        Shop = GameObject.Find("Shop");
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlayerPurchaseUnit(GameObject unit)
    {
        unit = Instantiate(unit);
        DataHandler.playerUnits.Add(unit);
        UpdateUnitTargets();
    }

    public void EnemyPurchaseUnit(GameObject unit)
    {
        unit = Instantiate(unit);
        DataHandler.enemyUnits.Add(unit);
        UpdateUnitTargets();
    }

    public void RemoveUnit(GameObject removedUnit)
    {
        if (removedUnit.GetComponent<UnitMain>().summoner.name == "Player")
        {
            DataHandler.enemy.GetComponent<SummonerMain>().experience += removedUnit.GetComponent<UnitMain>().experienceGiven;
            DataHandler.enemy.GetComponent<SummonerMain>().gold += removedUnit.GetComponent<UnitMain>().goldGiven;
            DataHandler.playerUnits.Remove(removedUnit);
        } 
        else
        {
            DataHandler.player.GetComponent<SummonerMain>().experience += removedUnit.GetComponent<UnitMain>().experienceGiven;
            DataHandler.player.GetComponent<SummonerMain>().gold += removedUnit.GetComponent<UnitMain>().goldGiven;
            DataHandler.enemyUnits.Remove(removedUnit);
        }
           
        UpdateUnitTargets();
    }

    public void UpdateUnitTargets()
    {
        //Set targets for player units
        for (int i = 0; i < DataHandler.playerUnits.Count; i++)
        {   
            if (DataHandler.playerUnits.Count > 1 && i != 0) //First unit has no ally in front
                DataHandler.playerUnits[i].GetComponent<UnitMain>().closestAllyUnit = DataHandler.playerUnits[i - 1];

            if (DataHandler.enemyUnits.Count > 0)
            {
                DataHandler.playerUnits[i].GetComponent<UnitMain>().closestEnemyUnit = DataHandler.enemyUnits[0];
            }
            else
            {
                DataHandler.playerUnits[i].GetComponent<UnitMain>().closestEnemyUnit = DataHandler.enemy;
            }
        }
        //Set targets for enemy units
        for (int i = 0; i < DataHandler.enemyUnits.Count; i++)
        {
            if (DataHandler.enemyUnits.Count > 1 && i != 0) //First unit has no ally in front
                DataHandler.enemyUnits[i].GetComponent<UnitMain>().closestAllyUnit = DataHandler.enemyUnits[i - 1];

            if (DataHandler.playerUnits.Count > 0)
            {
                DataHandler.enemyUnits[i].GetComponent<UnitMain>().closestEnemyUnit = DataHandler.playerUnits[0];
            }
            else
            {
                DataHandler.enemyUnits[i].GetComponent<UnitMain>().closestEnemyUnit = DataHandler.player;
            }
        }
    }

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
}
