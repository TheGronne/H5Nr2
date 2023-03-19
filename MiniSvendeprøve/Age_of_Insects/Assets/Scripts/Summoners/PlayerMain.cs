using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : SummonerMain
{
    public bool isAlive = true;

    public int passiveGoldGenerationAmount = 1;
    public float passiveGoldGenerationRegularity = 1;
    public float passiveGoldGenerationTimer;
    public bool runPassiveGoldGenerationTimer = true;

    // Start is called before the first frame update
    void Start()
    {
        health = 1000;
        gold = 125;
        experience = 0;
        defense = 0;
        civilizationId = 0;
        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameHandlerScript.Singleton.GameIsStarted)
        {
            if (passiveGoldGenerationTimer > 0 && runPassiveGoldGenerationTimer == true)
                passiveGoldGenerationTimer -= Time.deltaTime;
            else
            {
                gold += passiveGoldGenerationAmount;
                passiveGoldGenerationTimer = passiveGoldGenerationRegularity;
                UIManager.Singleton.UpdateUI();
            }
        }
    }

    public void Setup()
    {
        passiveGoldGenerationTimer = passiveGoldGenerationRegularity;
    }
}
