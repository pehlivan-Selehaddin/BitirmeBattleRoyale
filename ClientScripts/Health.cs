using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health
{
    private int health;
    private int maxHealth;

    public IDamagable damagable;
    public Health(int _maxHeealth)
    {
        maxHealth = _maxHeealth;
        health = maxHealth;
    }


    public int GetHealth() => health;
    public int SetHealth(int _health)
    {
        health = _health;

        health = health <= 0 ? 0 : health;
        if (health<=0)
        {
            damagable.Destroy();
        }
        return health;
    }


    public float GetPercentage() => (float)(health / maxHealth);

    public void Damage(int damage)
    {
        health -= damage;
        health = health <= 0 ? 0 : health;
    }

}
