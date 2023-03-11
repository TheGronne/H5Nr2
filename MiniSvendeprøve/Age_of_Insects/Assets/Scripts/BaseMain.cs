using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMain : MonoBehaviour
{
    public int health;

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log("Enemy died");
    }
}
