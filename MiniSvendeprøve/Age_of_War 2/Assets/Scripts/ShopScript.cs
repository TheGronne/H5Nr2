using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopScript : MonoBehaviour
{
    private ControllerScript Controller;

    private void Start()
    {
        Controller = GameObject.Find("Controller").GetComponent<ControllerScript>();
    }

    public void OpenUnitShop()
    {

    }

    public void OpenTurretShop()
    {

    }

    public void OpenCivilizationShop()
    {

    }

    public void Back()
    {

    }

    public void PlayerPurchaseUnit(int id)
    {
        var player = Controller.GetPlayer();
        if (PurchaseUnit(player, id))
        {
            var unit = SetupUnit(player, id);
            Controller.PlayerPurchaseUnit(unit);
        }
    }

    public void EnemyPurchaseUnit(int id)
    {
        var enemy = Controller.GetEnemy();
        if (PurchaseUnit(enemy, id))
        {
            var unit = SetupUnit(enemy, id);
            Controller.EnemyPurchaseUnit(unit);
        }
    }
    public bool PurchaseUnit(GameObject purchaser, int unitId)
    {
        if (purchaser.GetComponent<SummonerMain>().gold >= Controller.GetUnit(unitId).GetComponent<UnitMain>().goldCost)
        {
            purchaser.GetComponent<SummonerMain>().gold -= Controller.GetUnit(unitId).GetComponent<UnitMain>().goldCost;
            return true;
        }
        return false; 
    }

    public GameObject SetupUnit(GameObject purchaser, int unitId)
    {
        GameObject summonedUnit = Controller.GetUnit(unitId);
        summonedUnit.GetComponent<UnitMain>().summoner = purchaser;
        summonedUnit.tag = purchaser.name;
        summonedUnit.transform.position = purchaser.transform.Find("SummonPosition").position;
        return summonedUnit;
    }
}
