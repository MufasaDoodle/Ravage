using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 50f;

    Animator animator;
    public bool isDead = false;

    public void TakeDamage(float amount)
    {
        animator = GetComponent<Animator>();

        if (animator != null)
        {
            animator.SetBool("Take Damage", true);
        }

        health -= amount;
        if (health <= 0)
        {
            Player.Instance.EconomyController.GiveMoney(100);
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        if (animator == null)
        {
            Destroy(gameObject);
        }
        else
        {
            Destroy(GetComponent<BoxCollider>());
            animator.SetBool("Die", true);

            //if we in the future want to clean up dead bodies, maybe on a set timer or just after a certain amount of seconds
            //animator.GetCurrentAnimatorStateInfo(0).length to get clip length
            Destroy(gameObject, 10f);
            EnemySpawner.enemyCount--; //for spawner debugging purposes
        }


    }
}
