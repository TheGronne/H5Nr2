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
        var player = ControllerScript.Singleton.GetPlayerObject(ControllerScript.Singleton.GetLocalPlayerId());
        if (PurchaseUnit(player, id))
        {
            UIManager.Singleton.UpdateUI();
            ControllerScript.Singleton.SendUnitPurchase(id);
        }
    }

    public void PlayerPurchaseCivilization(int id)
    {
        var player = ControllerScript.Singleton.GetPlayerObject(ControllerScript.Singleton.GetLocalPlayerId());
        if (PurchaseCivilization(player, id))
        {
            ControllerScript.Singleton.SendCivilizationPurchase(id);
        }
    }

    public bool PurchaseUnit(GameObject purchaser, int unitId)
    {
        if (purchaser.GetComponent<SummonerMain>().gold >= ControllerScript.Singleton.GetUnit(unitId).GetComponent<PurchaseableMain>().goldCost)
        {
            purchaser.GetComponent<SummonerMain>().gold -= ControllerScript.Singleton.GetUnit(unitId).GetComponent<PurchaseableMain>().goldCost;
            return true;
        }
        return false; 
    }

    public bool PurchaseTurret(GameObject purchaser, int turretId)
    {
        if (purchaser.GetComponent<SummonerMain>().gold >= ControllerScript.Singleton.GetTurret(turretId).GetComponent<PurchaseableMain>().goldCost)
        {
            purchaser.GetComponent<SummonerMain>().gold -= ControllerScript.Singleton.GetTurret(turretId).GetComponent<PurchaseableMain>().goldCost;
            return true;
        }
        return false;
    }

    public bool PurchaseCivilization(GameObject purchaser, int civId)
    {
        if (purchaser.GetComponent<SummonerMain>().gold >= ControllerScript.Singleton.GetCivilization(civId).GetComponent<PurchaseableMain>().goldCost)
        {
            purchaser.GetComponent<SummonerMain>().gold -= ControllerScript.Singleton.GetCivilization(civId).GetComponent<PurchaseableMain>().goldCost;
            return true;
        }
        return false;
    }

}
