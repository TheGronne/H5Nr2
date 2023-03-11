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
        SendReady();
    }

    public void BackToMain()
    {
        usernameField.interactable = true;
        connectUI.SetActive(true);
    }

    public void SendName()
    {
        //Reliable means making sure package is received.
        Message message = Message.Create(MessageSendMode.reliable, (ushort)ClientToServerId.name);
        message.AddString(usernameField.text);
        Debug.Log(usernameField.text);
        NetworkManager.Singleton.Client.Send(message);
    }

    public void SendReady()
    {
        Message message = Message.Create(MessageSendMode.reliable, (ushort)ClientToServerId.ready);
        NetworkManager.Singleton.Client.Send(message);
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
            Debug.Log(item.Username);
            if (item.LobbyId == 0)
            {
                usernameText1.text = players[0].Username;
                if (players[0].IsReady)
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
                usernameText2.text = players[1].Username;
                if (players[1].IsReady)
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
        Message message = Message.Create(MessageSendMode.reliable, (ushort)ClientToServerId.startGame);
        NetworkManager.Singleton.Client.Send(message);
    }

    public void StartGame()
    {
        lobby.SetActive(false);
        inGame.SetActive(true);
    }
}
