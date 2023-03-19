using RiptideNetworking;
using RiptideNetworking.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _singleton;

    public static UIManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{ nameof(UIManager)} instance already exists, destroying duplicate");
                Destroy(value);
            }
        }
    }

    [Header("Connect")]
    [SerializeField] private GameObject connectUI;
    [SerializeField] private InputField usernameField;
    [SerializeField] private GameObject lobby;
    [SerializeField] private Text usernameText1;
    [SerializeField] private Button readyButton1;
    [SerializeField] private Text usernameText2;
    [SerializeField] private Button readyButton2;
    [SerializeField] private GameObject inGame;

    [SerializeField] private GameObject goldLabel;
    [SerializeField] private GameObject experienceLabel;
    [SerializeField] private GameObject localPlayer;

    private void Awake()
    {
        Singleton = this;
    }

    void Start()
    {
        lobby.SetActive(false);
        inGame.SetActive(false);
    }
    public void ConnectClicked()
    {
        usernameField.interactable = false;
        connectUI.SetActive(false);
        lobby.SetActive(true);

        NetworkManager.Singleton.Connect();
    }

    public void ReadyClicked()
    {   
        ControllerScript.Singleton.SendReady();
    }

    public void BackToMain()
    {
        usernameField.interactable = true;
        connectUI.SetActive(true);
        lobby.SetActive(false);
        inGame.SetActive(false);
    }

    public void BackToLobby()
    {
        connectUI.SetActive(false);
        lobby.SetActive(true);
        inGame.SetActive(false);
    }

    public void SendConnect()
    {
        //Reliable means making sure package is received.
        ControllerScript.Singleton.SendConnect(usernameField.text);
    }

    public void UpdateLobbyPlayers(Dictionary<ushort, Player> list)
    {
        List<Player> players = list.Select(kvp => kvp.Value).ToList();

        usernameText1.text = "New Text";
        usernameText2.text = "New Text";
        readyButton1.GetComponent<Image>().color = Color.red;
        readyButton1.GetComponentInChildren<Text>().text = "Not ready";
        readyButton2.GetComponent<Image>().color = Color.red;
        readyButton2.GetComponentInChildren<Text>().text = "Not ready";
        foreach (var item in players)
        {
            if (item.LobbyId == 0)
            {
                usernameText1.text = item.Username;
                if (item.IsReady)
                {
                    readyButton1.GetComponent<Image>().color = Color.green;
                    readyButton1.GetComponentInChildren<Text>().text = "Ready";
                }
                else
                {
                    readyButton1.GetComponent<Image>().color = Color.red;
                    readyButton1.GetComponentInChildren<Text>().text = "Not ready";
                }
            }

            if (item.LobbyId == 1)
            {
                usernameText2.text = item.Username;
                if (item.IsReady)
                {
                    readyButton2.GetComponent<Image>().color = Color.green;
                    readyButton2.GetComponentInChildren<Text>().text = "Ready";
                }
                else
                {
                    readyButton2.GetComponent<Image>().color = Color.red;
                    readyButton2.GetComponentInChildren<Text>().text = "Not ready";
                }
            }
        }
    }

    public void SendStartGameRequest()
    {
        ControllerScript.Singleton.SendStartGameRequest();
    }

    public void StartGame(List<Player> players)
    {
        lobby.SetActive(false);
        inGame.SetActive(true);

        foreach (var item in players)
            if (item.IsLocal)
                localPlayer = GameObject.Find("Player" + (item.LobbyId + 1));

        UpdateUI();
        GameObject.Find("Main Camera").GetComponent<CameraMain>().hasStarted = true;
    }

    public void UpdateUI()
    {
        goldLabel.GetComponent<Text>().text = "Gold: " + localPlayer.GetComponent<SummonerMain>().gold.ToString();
        experienceLabel.GetComponent<Text>().text = "EXP: " + localPlayer.GetComponent<SummonerMain>().experience.ToString();
    }
}
