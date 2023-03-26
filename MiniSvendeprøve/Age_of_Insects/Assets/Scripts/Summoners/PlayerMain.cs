using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : SummonerMain
{
    public bool isAlive = true;

    private int passiveGoldGenerationAmount = 5;
    private float passiveGoldGenerationRegularity = 1;
    private float passiveGoldGenerationTimer;
    private bool runPassiveGoldGenerationTimer = true;

    // Start is called before the first frame update
    void Start()
    {
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
        health = 1000;
        gold = 125000;
        experience = 10000;
        defense = 0;
        civilizationId = 0;
        passiveGoldGenerationTimer = passiveGoldGenerationRegularity;
        gameObject.GetComponent<UnitHealthbar>().SetMaxHealth(health);
        gameObject.GetComponent<UnitHealthbar>().SetHealth(health);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        gameObject.GetComponent<UnitHealthbar>().SetHealth(health);
    }

}
