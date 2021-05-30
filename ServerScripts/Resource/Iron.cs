using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iron : MonoBehaviour, IDamagable
{
    public Dictionary<int, Health> healths = new Dictionary<int, Health>();

    private void Start()
    {
        for (int i = 0; i < MatchMaker.instance.matchCount; i++)
        {
            healths.Add(i, new Health(30));
        }
    }
    public void Damage(int matchId, int _damage)
    {
        healths[matchId].Damage(_damage);
    }
}
