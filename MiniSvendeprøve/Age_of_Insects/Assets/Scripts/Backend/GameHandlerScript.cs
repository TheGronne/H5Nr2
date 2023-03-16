using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameHandlerScript : MonoBehaviour
{
    private static GameHandlerScript _singleton;

    public static GameHandlerScript Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{ nameof(GameHandlerScript)} instance already exists, destroying duplicate");
                Destroy(value);
            }
        }
    }

    private void Awake()
    {
        Singleton = this;
    }

    public void StartGame(Dictionary<ushort, Player> list)
    {
        List<ushort> playerIds = list.Select(kvp => kvp.Key).ToList();
        List<Player> players = list.Select(kvp => kvp.Value).ToList();

        UIManager.Singleton.StartGame(players);

        ControllerScript.Singleton.PostPlayerObject(playerIds[0], GameObject.Find("Player1"));
        ControllerScript.Singleton.PostPlayerObject(playerIds[1], GameObject.Find("Player2"));

        ControllerScript.Singleton.PatchPlayerName(players[0].Id, players[0].Username);
        ControllerScript.Singleton.PatchPlayerName(players[1].Id, players[1].Username);
    }

    public void SpawnUnit(ushort playerId, int unitId)
    {
        Debug.Log(playerId);
        var player = ControllerScript.Singleton.GetPlayerObject(playerId);
        var unit = ControllerScript.Singleton.GetUnit(unitId);

        unit.GetComponent<UnitMain>().summoner = player;
        unit.transform.position = player.transform.Find("SummonPosition").position;
        unit = Instantiate(unit);
        ControllerScript.Singleton.PostPlayerUnit(playerId, unit);
        UpdateUnitTargets();
    }

    public void RemoveUnit(GameObject removedUnit)
    {
        foreach (var item in ControllerScript.Singleton.GetPlayerObjectList())
        {
            if (removedUnit.GetComponent<UnitMain>().summonerId != item.Key)
            {
                ControllerScript.Singleton.GetPlayerSummonerMain(item.Key).experience += removedUnit.GetComponent<UnitMain>().experienceGiven;
                ControllerScript.Singleton.GetPlayerSummonerMain(item.Key).gold += removedUnit.GetComponent<UnitMain>().goldGiven;
            } else
            {
                ControllerScript.Singleton.DeletePlayerUnit(item.Key, removedUnit);
            }
        }

        UpdateUnitTargets();
        UIManager.Singleton.UpdateUI();
    }

    public void UpdateUnitTargets()
    {
        foreach (var item in ControllerScript.Singleton.GetPlayerObjectList())
        {
            ushort enemyPlayerId = new ushort();
            foreach (var item1 in ControllerScript.Singleton.GetPlayerObjectList())
            {
                if (item.Key != item1.Key)
                    enemyPlayerId = item1.Key;
            }

            for (int i = 0; i < ControllerScript.Singleton.GetPlayerUnitList(item.Key).Count; i++)
            {
                if (ControllerScript.Singleton.GetPlayerUnitList(item.Key).Count > 1 && i != 0) //First unit has no ally in front
                    ControllerScript.Singleton.GetPlayerUnitUnitMain(item.Key, i).closestAllyUnit = ControllerScript.Singleton.GetPlayerUnit(item.Key, i - 1);

                if (ControllerScript.Singleton.GetPlayerUnitList(enemyPlayerId).Count > 0)
                {
                    ControllerScript.Singleton.GetPlayerUnitUnitMain(item.Key, i).closestEnemyUnit = ControllerScript.Singleton.GetPlayerUnit(enemyPlayerId, 0);
                }
                else
                {
                    ControllerScript.Singleton.GetPlayerUnitUnitMain(item.Key, i).closestEnemyUnit = ControllerScript.Singleton.GetPlayerObject(enemyPlayerId);
                }
            }
        }
    }

    public void RunPlayerCivilizationAction(ushort playerId, int civId)
    {
        ControllerScript.Singleton.RunPlayerCivilizationAction(playerId, civId);
    }
}
