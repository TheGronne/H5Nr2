using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHandlerScript : MonoBehaviour
{
    private static DataHandlerScript _singleton;

    public static DataHandlerScript Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{ nameof(DataHandlerScript)} instance already exists, destroying duplicate");
                Destroy(value);
            }
        }
    }

    private void Awake()
    {
        Singleton = this;
    }

    public Dictionary<ushort, Player> playerClassList = new Dictionary<ushort, Player>();
    public Dictionary<ushort, GameObject> playerGameObjectList = new Dictionary<ushort, GameObject>();

    public Dictionary<ushort, List<GameObject>> playerUnits = new Dictionary<ushort, List<GameObject>>();

    public List<GameObject> units = new List<GameObject>();
    public List<GameObject> turrets = new List<GameObject>();
    public List<GameObject> civilizations = new List<GameObject>();

    // Must always be in same order as civilizations list
    public List<GameObject> UnitShops = new List<GameObject>();
    public List<GameObject> TurretShops = new List<GameObject>();
    public List<GameObject> CivilizationShops = new List<GameObject>();

    public delegate void civAction(GameObject purchaser);
    public List<civAction> civilizationActions = new List<civAction>();

    // Start is called before the first frame update
    void Start()
    {
        civilizationActions.Add(Ladybug);
        civilizationActions.Add(Earthworm);
        civilizationActions.Add(Flea);
        civilizationActions.Add(Cockroach);
        civilizationActions.Add(Millipede);
        civilizationActions.Add(Caterpillar);
        civilizationActions.Add(Cricket);
        civilizationActions.Add(Termite);
        civilizationActions.Add(Beetle);
        civilizationActions.Add(Scorpion);
        civilizationActions.Add(Centipede);
        civilizationActions.Add(PrayingMantis);
        civilizationActions.Add(Spider);
        civilizationActions.Add(Wasp);
        civilizationActions.Add(Ant);
    }

    public void RunCivilizationAction(int civId, GameObject purchaser)
    {
        civilizationActions[civId - 1](purchaser);
    }

    // Generation 2
    private void Ladybug(GameObject purchaser)
    {
        purchaser.GetComponent<SummonerMain>().defense = 2;
        purchaser.GetComponent<SpriteRenderer>().color = new Color(0.75f, 0.75f, 1f, 1f);
    }

    private void Earthworm(GameObject purchaser)
    {
        purchaser.GetComponent<SummonerMain>().defense = 1;
        purchaser.GetComponent<SpriteRenderer>().color = new Color(0.75f, 1f, 0.75f, 1f);
    }

    private void Flea(GameObject purchaser)
    {
        purchaser.GetComponent<SummonerMain>().defense = 0;
        purchaser.GetComponent<SpriteRenderer>().color = new Color(1f, 0.75f, 0.75f, 1f);
    }

    // Generation 3
    private void Cockroach(GameObject purchaser)
    {
        purchaser.GetComponent<SummonerMain>().defense = 10;
        purchaser.GetComponent<SpriteRenderer>().color = new Color(0.75f, 0.75f, 1f, 1f);
    }

    private void Millipede(GameObject purchaser)
    {
        purchaser.GetComponent<SummonerMain>().defense = 8;
        purchaser.GetComponent<SpriteRenderer>().color = new Color(0.75f, 1f, 0.75f, 1f);
    }

    private void Caterpillar(GameObject purchaser)
    {
        purchaser.GetComponent<SummonerMain>().defense = 6;
        purchaser.GetComponent<SpriteRenderer>().color = new Color(1f, 0.75f, 0.75f, 1f);
    }

    private void Cricket(GameObject purchaser)
    {
        purchaser.GetComponent<SummonerMain>().defense = 4;
        purchaser.GetComponent<SpriteRenderer>().color = new Color(0.75f, 0.75f, 1f, 1f);
    }

    private void Termite(GameObject purchaser)
    {
        purchaser.GetComponent<SummonerMain>().defense = 2;
        purchaser.GetComponent<SpriteRenderer>().color = new Color(0.75f, 1f, 0.75f, 1f);
    }


    // Generation 4
    private void Beetle(GameObject purchaser)
    {
        purchaser.GetComponent<SummonerMain>().defense = 10000;
        purchaser.GetComponent<SpriteRenderer>().color = new Color(1f, 0.75f, 0.75f, 1f);
    }
    private void Scorpion(GameObject purchaser)
    {
        purchaser.GetComponent<SummonerMain>().defense = 100;
        purchaser.GetComponent<SpriteRenderer>().color = new Color(1f, 0.75f, 0.75f, 1f);
    }
    private void Centipede(GameObject purchaser)
    {
        purchaser.GetComponent<SummonerMain>().defense = 80;
        purchaser.GetComponent<SpriteRenderer>().color = new Color(1f, 0.75f, 0.75f, 1f);
    }
    private void PrayingMantis(GameObject purchaser)
    {
        purchaser.GetComponent<SummonerMain>().defense = 60;
        purchaser.GetComponent<SpriteRenderer>().color = new Color(1f, 0.75f, 0.75f, 1f);
    }
    private void Spider(GameObject purchaser)
    {
        purchaser.GetComponent<SummonerMain>().defense = 40;
        purchaser.GetComponent<SpriteRenderer>().color = new Color(1f, 0.75f, 0.75f, 1f);
    }
    private void Wasp(GameObject purchaser)
    {
        purchaser.GetComponent<SummonerMain>().defense = 25;
        purchaser.GetComponent<SpriteRenderer>().color = new Color(1f, 0.75f, 0.75f, 1f);
    }
    private void Ant(GameObject purchaser)
    {
        purchaser.GetComponent<SummonerMain>().defense = 10;
        purchaser.GetComponent<SpriteRenderer>().color = new Color(1f, 0.75f, 0.75f, 1f);
    }
}
