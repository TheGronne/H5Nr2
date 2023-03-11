using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopScript : MonoBehaviour
{
    public List<GameObject> Units;
    public List<GameObject> Turrets;
    public List<GameObject> Civilizations;

    public GameObject UnitShop;
    public GameObject TurretShop;
    public GameObject CivilizationShop;
    public GameObject OuterMenu;

    private void Start()
    {
        SetupNewShop(UnitShop, TurretShop, CivilizationShop);
    }

    public void SetupNewShop(GameObject unitShop, GameObject turretShop, GameObject civilizationShop)
    {
        //Hide previous menues
        Back();
        var uShop = Instantiate(unitShop);
        uShop.transform.SetParent(GameObject.Find("UnitShop").transform);
        uShop.GetComponent<RectTransform>().localPosition = GameObject.Find("UnitShop").GetComponent<RectTransform>().localPosition;
        uShop.GetComponent<RectTransform>().localScale = GameObject.Find("UnitShop").GetComponent<RectTransform>().localScale;
        UnitShop = uShop;
        var tShop = Instantiate(turretShop);
        tShop.transform.SetParent(GameObject.Find("TurretShop").transform);
        tShop.GetComponent<RectTransform>().localPosition = GameObject.Find("TurretShop").GetComponent<RectTransform>().localPosition;
        tShop.GetComponent<RectTransform>().localScale = GameObject.Find("TurretShop").GetComponent<RectTransform>().localScale;
        TurretShop = tShop;
        var cShop = Instantiate(civilizationShop);
        cShop.transform.SetParent(GameObject.Find("CivilizationShop").transform);
        cShop.GetComponent<RectTransform>().localPosition = GameObject.Find("CivilizationShop").GetComponent<RectTransform>().localPosition;
        cShop.GetComponent<RectTransform>().localScale = GameObject.Find("CivilizationShop").GetComponent<RectTransform>().localScale;
        CivilizationShop = cShop;
        //Hide new menues
        Back();
    }

    public void OpenUnitShop()
    {
        UnitShop.SetActive(true);
        OuterMenu.SetActive(false);
    }

    public void OpenTurretShop()
    {
        TurretShop.SetActive(true);
        OuterMenu.SetActive(false);
    }

    public void OpenCivilizationShop()
    {
        CivilizationShop.SetActive(true);
        OuterMenu.SetActive(false);
    }

    public void Back()
    {
        OuterMenu.SetActive(true);
        UnitShop.SetActive(false);
        TurretShop.SetActive(false);
        CivilizationShop.SetActive(false);
    }

    public void PlayerPurchaseUnit(int id)
    {
        //i wish I could add controller as a private object, but since shop is a prefab I would have to do tons of rework, including turning controller into prefab
        var player = GameObject.Find("Controller").GetComponent<ControllerScript>().GetPlayer();
        if (PurchaseUnit(player, id))
        {
            var unit = GameObject.Find("Controller").GetComponent<ControllerScript>().GetUnit(id);
            GameObject.Find("GameHandler").GetComponent<GameHandlerScript>().PlayerPurchaseUnit(unit);
        }
    }

    public void EnemyPurchaseUnit(int id)
    {
        var enemy = GameObject.Find("Controller").GetComponent<ControllerScript>().GetEnemy();
        if (PurchaseUnit(enemy, id))
        {
            var unit = GameObject.Find("Controller").GetComponent<ControllerScript>().GetUnit(id);
            GameObject.Find("GameHandler").GetComponent<GameHandlerScript>().EnemyPurchaseUnit(unit);
        }
    }

    public void PlayerPurchaseCivilization(int id)
    {
        var player = GameObject.Find("Controller").GetComponent<ControllerScript>().GetPlayer();
        if (PurchaseCivilization(player, id))
        {
            GameObject.Find("GameHandler").GetComponent<GameHandlerScript>().PlayerPurchaseCivilization(id);
        }
    }

    public void EnemyPurchaseCivilization(int id)
    {
        var enemy = GameObject.Find("Controller").GetComponent<ControllerScript>().GetEnemy();
        if (PurchaseCivilization(enemy, id))
        {
            GameObject.Find("GameHandler").GetComponent<GameHandlerScript>().EnemyPurchaseCivilization(id);
        }
    }

    public bool PurchaseUnit(GameObject purchaser, int unitId)
    {
        if (purchaser.GetComponent<SummonerMain>().gold >= GameObject.Find("Controller").GetComponent<ControllerScript>().GetUnit(unitId).GetComponent<PurchaseableMain>().goldCost)
        {
            purchaser.GetComponent<SummonerMain>().gold -= GameObject.Find("Controller").GetComponent<ControllerScript>().GetUnit(unitId).GetComponent<PurchaseableMain>().goldCost;
            return true;
        }
        return false; 
    }

    public bool PurchaseTurret(GameObject purchaser, int turretId)
    {
        if (purchaser.GetComponent<SummonerMain>().gold >= GameObject.Find("Controller").GetComponent<ControllerScript>().GetTurret(turretId).GetComponent<PurchaseableMain>().goldCost)
        {
            purchaser.GetComponent<SummonerMain>().gold -= GameObject.Find("Controller").GetComponent<ControllerScript>().GetTurret(turretId).GetComponent<PurchaseableMain>().goldCost;
            return true;
        }
        return false;
    }

    public bool PurchaseCivilization(GameObject purchaser, int civId)
    {
        if (purchaser.GetComponent<SummonerMain>().gold >= GameObject.Find("Controller").GetComponent<ControllerScript>().GetCivilization(civId).GetComponent<PurchaseableMain>().goldCost)
        {
            purchaser.GetComponent<SummonerMain>().gold -= GameObject.Find("Controller").GetComponent<ControllerScript>().GetCivilization(civId).GetComponent<PurchaseableMain>().goldCost;
            return true;
        }
        return false;
    }

}
