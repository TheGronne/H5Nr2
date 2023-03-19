﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMain : PurchaseableMain
{
    public int baseDamage = 0;
    public int homeDamage = 0;
    public float attackSpeed = 0;
    private float attackTimer = 0;
    public int attackRange = 0;
    public float walkingSpeed = 0;
    public int sizeRadius = 0;

    public GameObject closestEnemyUnit;
    public GameObject closestAllyUnit;

    //True = walking towards enemy, False = walking towards player
    public GameObject summoner;
    public ushort summonerId;
    private bool walkingDirection;
    private bool isStopped;
    private bool isAttacking = false;

    public float summonTime = 0;
        
    // Start is called before the first frame update
    void Start()
    {
        attackTimer = 1 / attackSpeed;
        //True if player, false if enemy
        walkingDirection = summoner.name == "Player1" ? true : false;
        gameObject.GetComponent<UnitHealthbar>().SetMaxHealth(health);
        gameObject.GetComponent<UnitHealthbar>().SetHealth(health);
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
            Die();

        //Attack enemy unit
        if (closestEnemyUnit.tag == "Player")
        {
            if (closestEnemyUnit != null && Math.Abs(closestEnemyUnit.transform.position.x - transform.position.x) < attackRange + 2)
                StartAttacking();
            //Stop behind ally unit
            else if (closestAllyUnit != null && Math.Abs(closestAllyUnit.transform.position.x - transform.position.x) < sizeRadius)
                StopMoving();
            else
                StartMoving();
        }
        else
        {
            if (closestEnemyUnit != null && Math.Abs(closestEnemyUnit.transform.position.x - transform.position.x) < attackRange)
                StartAttacking();
            //Stop behind ally unit
            else if (closestAllyUnit != null && Math.Abs(closestAllyUnit.transform.position.x - transform.position.x) < sizeRadius)
                StopMoving();
            else
                StartMoving();
        }
       

        if (!isStopped)
        {
            if (walkingDirection)
                transform.position += new Vector3(walkingSpeed * Time.deltaTime, 0, 0);
            else
                transform.position += new Vector3(-walkingSpeed * Time.deltaTime, 0, 0);
        }

        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    public void StartAttacking()
    {
        StopMoving();
        isAttacking = true;
        if (attackTimer <= 0)
        {
            Attack();
            attackTimer = 1 / attackSpeed;
        }
    }
    public void Attack()
    {
        if (closestEnemyUnit.name == "Player1" || closestEnemyUnit.name == "Player2")
        {
            if (homeDamage - closestEnemyUnit.GetComponent<SummonerMain>().defense > 0)
                closestEnemyUnit.GetComponent<BaseMain>().TakeDamage(homeDamage - closestEnemyUnit.GetComponent<SummonerMain>().defense);
            else // Minimum 1 damage dealt
                closestEnemyUnit.GetComponent<BaseMain>().TakeDamage(1);
        }
        else
            closestEnemyUnit.GetComponent<BaseMain>().TakeDamage(baseDamage);
    }

    protected override void Die()
    {
        GameHandlerScript.Singleton.RemoveUnit(gameObject);
        //Keep this line at all costs. Assures units die at the same time if 2 units are equal.
        StartAttacking();
        Destroy(gameObject);
    }

    public void StopMoving()
    {
        isStopped = true;
    }

    public void StartMoving()
    {
        isAttacking = false;
        isStopped = false;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        gameObject.GetComponent<UnitHealthbar>().SetHealth(health);
    }
}
