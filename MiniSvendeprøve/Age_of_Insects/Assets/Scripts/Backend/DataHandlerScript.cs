using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHandlerScript : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;

    public List<GameObject> playerUnits = new List<GameObject>();
    public List<GameObject> enemyUnits = new List<GameObject>();

    public List<GameObject> playerUnitQueue = new List<GameObject>();
    public List<GameObject> enemyUnitQueue = new List<GameObject>();

    public List<GameObject> units = new List<GameObject>();
    public List<GameObject> turrets = new List<GameObject>();
    public List<GameObject> civilizations = new List<GameObject>();

    // Must always be in same order as civilizations list
    public List<GameObject> UnitShops = new List<GameObject>();
    public List<GameObject> TurretShops = new List<GameObject>();
    public List<GameObject> CivilizationShops = new List<GameObject>();

    public delegate void civAction(SummonerMain purchaser);
    public List<civAction> civilizationActions = new List<civAction>();

    // Start is called before the first frame update
    void Start()
    {
        civilizationActions.Add(Ant);
        civilizationActions.Add(Beetle);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RunCivilizationAction(int civId, SummonerMain purchaser)
    {
        civilizationActions[civId - 1](purchaser);
    }

    private void Ant(SummonerMain purchaser)
    {
        purchaser.defense = 0;
    }

    private void Beetle(SummonerMain purchaser)
    {
        purchaser.defense = 1;
    }
}
