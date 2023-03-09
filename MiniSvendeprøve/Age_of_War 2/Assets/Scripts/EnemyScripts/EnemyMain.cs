using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMain : SummonerMain
{
    public int civilizationId = 0;
    public bool isAlive = true;

    public int passiveGoldGenerationAmount = 1;
    public float passiveGoldGenerationRegularity = 5;
    public float passiveGoldGenerationTimer;
    public bool runPassiveGoldGenerationTimer = true;

    // Start is called before the first frame update
    void Start()
    {
        health = 1000;
        gold = 200;
        experience = 0;
        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        if (passiveGoldGenerationTimer > 0 && runPassiveGoldGenerationTimer == true)
        {
            passiveGoldGenerationTimer -= Time.deltaTime;
        }
        else
        {
            gold += passiveGoldGenerationAmount;
            passiveGoldGenerationTimer = passiveGoldGenerationRegularity;
        }
    }

    public void Setup()
    {
        passiveGoldGenerationTimer = passiveGoldGenerationRegularity;
    }
}
