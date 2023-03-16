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
        var player = ControllerScript.Singleton.GetPlayerObject(playerId);
        var unit = ControllerScript.Singleton.GetUnit(unitId);

        unit.GetComponent<UnitMain>().summoner = player;
        unit.GetComponent<UnitMain>().summonerId = playerId;
        unit.transform.position = player.transform.Find("SummonPosition").position;
        unit = Instantiate(unit);
        ControllerScript.Singleton.PostPlayerUnit(playerId, unit);
        UpdateUnitTargets();
    }

    public void RemoveUnit(GameObject removedUnit)
    {
        foreach (var player in ControllerScript.Singleton.GetPlayerClassList())
        {
            if (removedUnit.GetComponent<UnitMain>().summonerId != player.Value.Id)
            {
                ControllerScript.Singleton.GetPlayerSummonerMain(player.Key).experience += removedUnit.GetComponent<UnitMain>().experienceGiven;
                ControllerScript.Singleton.GetPlayerSummonerMain(player.Key).gold += removedUnit.GetComponent<UnitMain>().goldGiven;
            } else
            {
                ControllerScript.Singleton.DeletePlayerUnit(player.Key, 0);
            }
        }

        UpdateUnitTargets();
        UIManager.Singleton.UpdateUI();
    }

    public void UpdateUnitTargets()
    {
        foreach (var player in ControllerScript.Singleton.GetPlayerClassList())
        {
            ushort enemyPlayerId = new ushort();
            foreach (var player2 in ControllerScript.Singleton.GetPlayerClassList())
                if (player.Value.Id != player2.Value.Id)
                    enemyPlayerId = player2.Value.Id;

            for (int i = 0; i < ControllerScript.Singleton.GetPlayerUnitList(player.Value.Id).Count; i++)
            {
                if (i != 0) //First unit has no ally in front
                    ControllerScript.Singleton.GetPlayerUnitUnitMain(player.Value.Id, i).closestAllyUnit = ControllerScript.Singleton.GetPlayerUnit(player.Value.Id, i - 1);

                if (ControllerScript.Singleton.GetPlayerUnitList(enemyPlayerId).Count > 0)
                    ControllerScript.Singleton.GetPlayerUnitUnitMain(player.Value.Id, i).closestEnemyUnit = ControllerScript.Singleton.GetPlayerUnit(enemyPlayerId, 0);
                else
                    ControllerScript.Singleton.GetPlayerUnitUnitMain(player.Value.Id, i).closestEnemyUnit = ControllerScript.Singleton.GetPlayerObject(enemyPlayerId);
            }
        }
    }

    public void RunPlayerCivilizationAction(ushort playerId, int civId)
    {
        ControllerScript.Singleton.RunPlayerCivilizationAction(playerId, civId);
    }
}
