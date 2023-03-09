using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMain : BaseMain
{
    public int baseDamage = 0;
    public int homeDamage = 0;
    public float attackSpeed = 0;
    private float attackTimer = 0;
    public int attackRange = 0;
    public float walkingSpeed = 0;
    public int sizeRadius = 0;

    public int goldCost = 0;

    public int experienceGiven = 0;
    public int goldGiven = 0;

    public GameObject closestEnemyUnit;
    public GameObject closestAllyUnit;

    //True = walking towards enemy, False = walking towards player
    public GameObject summoner;
    public bool walkingDirection;
    public bool isStopped;
    private bool isAttacking = false;
        
    // Start is called before the first frame update
    void Start()
    {
        attackTimer = 1 / attackSpeed;
        //True if player, false if enemy
        walkingDirection = summoner.name == "Player" ? true : false;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
            Die();

        //Attack enemy unit
        if (closestEnemyUnit != null && Vector2.Distance(closestEnemyUnit.transform.position, transform.position) < attackRange)
            StartAttacking();
        //Stop behind ally unit
        else if (closestAllyUnit != null && Vector2.Distance(closestAllyUnit.transform.position, transform.position) < sizeRadius)
            StopMoving();
        else 
            StartMoving();

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
        if (closestEnemyUnit.name == "Player" || closestEnemyUnit.name == "Enemy")
            closestEnemyUnit.GetComponent<BaseMain>().TakeDamage(homeDamage);
        else
            closestEnemyUnit.GetComponent<BaseMain>().TakeDamage(baseDamage);
    }

    protected override void Die()
    {
        GameObject.Find("Controller").GetComponent<ControllerScript>().RemoveUnit(gameObject);
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
}
