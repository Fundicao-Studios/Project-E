using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int healthLevel = 10;
    public int maxHealth;
    public int currentHealth;

    Animator animator;
    Navigation_CustomPathfinding nav;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        nav = GetComponent<Navigation_CustomPathfinding>();
    }

    void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth = currentHealth - damage;

        animator.Play("Damage_01");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            animator.Play("Dead_01");
            nav.enabled = false;
        }
    }

    public void Death()
    {
        //Destroy(gameObject);
    }
}
