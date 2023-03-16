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

    public delegate void civAction(SummonerMain purchaser);
    public List<civAction> civilizationActions = new List<civAction>();

    // Start is called before the first frame update
    void Start()
    {
        civilizationActions.Add(Ant);
        civilizationActions.Add(Beetle);
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
