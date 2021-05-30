using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iron : MonoBehaviour, IDamagable
{
    public Health health;
    public string id;
    private void Start()
    {
        id = Guid.NewGuid().ToString();
        health = new Health(30);
        health.damagable = this;

        GameManager.irons.Add(id, health);
    }
    //public event EventHandler OnHealthChanged;


    public void Damage(int _damage)
    {
        health.Damage(_damage);
        Debug.Log(health.GetHealth());
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}