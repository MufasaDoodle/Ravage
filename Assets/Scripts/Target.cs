using System;
using System.Collections;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 50f;

    public bool isDead = false;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
