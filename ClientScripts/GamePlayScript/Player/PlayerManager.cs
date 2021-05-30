using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public int id;
    public int matchId;

    public bool isSpawn = false;

    
    public Animator animator;
    public bool isLocalPlayer
    {
        get
        {
            return id == Client.instance.myId;
        }
    }
    public void Initialize(int _id, int _matchId)
    {
        id = _id;
        matchId = _matchId;
    }
}
