using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandlerScript : MonoBehaviour
{
    private ControllerScript Controller;
    private float playerUnitSummonTimer = 0;
    private float enemyUnitSummonTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        Controller = GameObject.Find("Controller").GetComponent<ControllerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Controller.GetPlayerUnitQueue().Count > 0)
        {
            if (playerUnitSummonTimer <= 0)
            {
                var summonedUnit = SummonUnit(Controller.GetPlayer(), Controller.GetPlayerUnitQueueUnit(0));
                Controller.DeletePlayerUnitQueueUnit(0);
                Controller.PostPlayerUnit(summonedUnit);
                UpdateUnitTargets();
                if (Controller.GetPlayerUnitQueue().Count > 0)
                    playerUnitSummonTimer = Controller.GetPlayerUnitQueueUnit(0).GetComponent<UnitMain>().summonTime;
            }
            else
            {
                playerUnitSummonTimer -= Time.deltaTime;
            }
        }
        if (Controller.GetEnemyUnitQueue().Count > 0)
        {
            if (enemyUnitSummonTimer <= 0)
            {
                var summonedUnit = SummonUnit(Controller.GetEnemy(), Controller.GetEnemyUnitQueueUnit(0));
                Controller.DeleteEnemyUnitQueueUnit(0);
                Controller.PostEnemyUnit(summonedUnit);
                UpdateUnitTargets();
                if (Controller.GetEnemyUnitQueue().Count > 0)
                    enemyUnitSummonTimer = Controller.GetEnemyUnitQueueUnit(0).GetComponent<UnitMain>().summonTime;
            }
            else
            {
                enemyUnitSummonTimer -= Time.deltaTime;
            }
        }
    }

    public void StartGame()
    {
        UIManager.Singleton.StartGame();
    }

    public void PlayerPurchaseUnit(GameObject unit)
    {
        if (Controller.GetPlayerUnitQueue().Count == 0)
            playerUnitSummonTimer = unit.GetComponent<UnitMain>().summonTime;
        Controller.PostUnitToPlayerQueue(unit);
    }

    public void EnemyPurchaseUnit(GameObject unit)
    {
        if (Controller.GetEnemyUnitQueue().Count == 0)
            enemyUnitSummonTimer = unit.GetComponent<UnitMain>().summonTime;
        Controller.PostUnitToEnemyQueue(unit);
    }

    public GameObject SummonUnit(GameObject purchaser, GameObject unit)
    {
        unit.GetComponent<UnitMain>().summoner = purchaser;
        unit.tag = purchaser.name;
        unit.transform.position = purchaser.transform.Find("SummonPosition").position;
        unit = Instantiate(unit);
        return unit;
    }

    public void RemoveUnit(GameObject removedUnit)
    {
        if (removedUnit.GetComponent<UnitMain>().summoner.name == "Player")
        {
            Controller.GetEnemySummonerMain().experience += removedUnit.GetComponent<UnitMain>().experienceGiven;
            Controller.GetEnemySummonerMain().gold += removedUnit.GetComponent<UnitMain>().goldGiven;
            Controller.DeletePlayerUnit(removedUnit);
        }
        else
        {
            Controller.GetPlayerSummonerMain().experience += removedUnit.GetComponent<UnitMain>().experienceGiven;
            Controller.GetPlayerSummonerMain().gold += removedUnit.GetComponent<UnitMain>().goldGiven;
            Controller.DeleteEnemyUnit(removedUnit);
        }

        UpdateUnitTargets();
    }

    public void UpdateUnitTargets()
    {
        //Set targets for player units
        for (int i = 0; i < Controller.GetPlayerUnitList().Count; i++)
        {
            if (Controller.GetPlayerUnitList().Count > 1 && i != 0) //First unit has no ally in front
                Controller.GetPlayerUnitUnitMain(i).closestAllyUnit = Controller.GetPlayerUnit(i - 1);

            if (Controller.GetEnemyUnitList().Count > 0)
            {
                Controller.GetPlayerUnitUnitMain(i).closestEnemyUnit = Controller.GetEnemyUnit(0);
            }
            else
            {
                Controller.GetPlayerUnitUnitMain(i).closestEnemyUnit = Controller.GetEnemy();
            }
        }
        //Set targets for enemy units
        for (int i = 0; i < Controller.GetEnemyUnitList().Count; i++)
        {
            if (Controller.GetEnemyUnitList().Count > 1 && i != 0) //First unit has no ally in front
                Controller.GetEnemyUnitUnitMain(i).closestAllyUnit = Controller.GetEnemyUnit(i - 1);

            if (Controller.GetPlayerUnitList().Count > 0)
            {
                Controller.GetEnemyUnitUnitMain(i).closestEnemyUnit = Controller.GetPlayerUnit(0);
            }
            else
            {
                Controller.GetEnemyUnitUnitMain(i).closestEnemyUnit = Controller.GetPlayer();
            }
        }
    }

    public void PlayerPurchaseCivilization(int id)
    {
        Controller.RunEnemyCivilizationAction(id);
    }

    public void EnemyPurchaseCivilization(int id)
    {
        Controller.RunPlayerCivilizationAction(id);
    }
}
