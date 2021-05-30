using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health
{
    private int health;
    private int maxHealth;

    public Health(int _maxHealth)
    {
        maxHealth = _maxHealth;
        health = _maxHealth;
    }

    public int GetHealth() => health;
    public void SetHealth(int _health) => health = _health;

    public float GetPercentage() => health / maxHealth;

    public int Damage(int _damage) => health -= _damage;
}
