using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;

    public Vector3 startPos;
    public Quaternion startRot;

    [SerializeField]
    private GameObject hitArea;

    private float speed = 10;
    public void Initialize(int _id)
    {
        id = _id;
    }
   
    public void Move(Vector3 _moveDirection)
    {
        transform.LookAt(_moveDirection);
        transform.position = Vector3.MoveTowards(transform.position, _moveDirection, Time.deltaTime * speed);

        ServerSend.PlayerPosition(Server.clients[id].match, id, transform.position);
        ServerSend.PlayerRotation(Server.clients[id].match, id, transform.rotation);
    }

    public void PlayerInAim(Vector3 lookDirection, AimType aimType, bool isMoving, Vector3 moveDirection)
    {
        switch (aimType)
        {
            case AimType.BowAim:
                speed = 2;
                break;
            case AimType.PistolAim:
                speed = 3;
                break;
            default:
                speed = 10;
                break;
        }

        transform.LookAt(lookDirection);
        Match match = Server.clients[id].match;

        ServerSend.PlayerRotation(match, id, transform.rotation);


        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveDirection, Time.fixedDeltaTime * speed);

            ServerSend.PlayerPosition(match, id, transform.position);
        }
    }

    public void HitTree(string treeId,int damage)
    {
        //Vector3 colliderSize = Vector3.up * .3f;
        Collider[] colliderArray = Physics.OverlapSphere(hitArea.transform.position, .7f);
        for (int i = 0; i < colliderArray.Length; i++)
        {
            if (colliderArray[i].TryGetComponent(out Tree tree))
            {
                tree.Damage(Server.clients[id].match.id,damage);
                ServerSend.TreeHealth(id, treeId, tree.healths[Server.clients[id].match.id].GetHealth());
            }
        }
    }

    public void HitStone(string stoneId, int damage)
    {
       // Vector3 colliderSize = Vector3.up * .7f;
        Collider[] colliderArray = Physics.OverlapSphere(hitArea.transform.position, .7f);
        for (int i = 0; i < colliderArray.Length; i++)
        {
            if (colliderArray[i].TryGetComponent(out Stone stone))
            {
                stone.Damage(Server.clients[id].match.id, damage);
                ServerSend.StoneHealth(id, stoneId, stone.healths[Server.clients[id].match.id].GetHealth());
            }
        }
    }

    public void HitIron(string ironId, int damage)
    {

        // Vector3 colliderSize = Vector3.up * .7f;
        Collider[] colliderArray = Physics.OverlapSphere(hitArea.transform.position, .7f);
        for (int i = 0; i < colliderArray.Length; i++)
        {
            if (colliderArray[i].TryGetComponent(out Iron iron))
            {
                iron.Damage(Server.clients[id].match.id, damage);
                ServerSend.IronHealth(id, ironId, iron.healths[Server.clients[id].match.id].GetHealth());
            }
        }
    }
}

/*PERFORMANS DENENECEK*/
//Vector3 movedirection;
//private void FixedUpdate()
//{
//    transform.LookAt(_moveDirection);
//    transform.position = Vector3.MoveTowards(transform.position, _moveDirection, Time.fixedDeltaTime * speed);

//    ServerSend.PlayerPosition(Server.clients[id].match, id, transform.position);
//    ServerSend.PlayerRotation(Server.clients[id].match, id, transform.rotation);
//}