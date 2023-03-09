using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEventHandler : MonoBehaviour
{
    public static GameObject goldLabel;
    public static GameObject experienceLabel;
    public static GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        goldLabel = GameObject.Find("GoldLabel");
        experienceLabel = GameObject.Find("ExperienceLabel");
    }

    // Update is called once per frame
    void Update()
    {
        goldLabel.GetComponent<Text>().text = "Gold: " + player.GetComponent<SummonerMain>().gold.ToString();
        experienceLabel.GetComponent<Text>().text = "EXP: " + player.GetComponent<SummonerMain>().experience.ToString();
    }
}
