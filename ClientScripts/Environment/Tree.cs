using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour, IDamagable
{
    public Health health;
    public string id;
    private void Start()
    {
        id = Guid.NewGuid().ToString();
        health = new Health(100);

        health.damagable = this;

        GameManager.trees.Add(id,health);
    }
    public void Damage(int _damage)
    {
        health.Damage(_damage);
        if (health.GetHealth() < 0)
        {
            Destroy(gameObject, 1);
            //effect olaiblir
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
public interface IDamagable
{
    void Damage(int _damage);
    void Destroy();
}