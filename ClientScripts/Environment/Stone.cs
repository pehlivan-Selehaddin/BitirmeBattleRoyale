using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour, IDamagable
{
    public Health health;
    public string id;
    [SerializeField]
    private int _health;
    private void Start()
    {
        id = Guid.NewGuid().ToString();
        health = new Health(_health);
        health.damagable = this;

        GameManager.stones.Add(id, health);
    }
    // public event EventHandler OnHealthChanged;


    public void Damage(int _damage)
    {
        health.Damage(_damage);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}