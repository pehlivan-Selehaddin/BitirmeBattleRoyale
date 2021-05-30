using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeControl : MonoBehaviour
{
    private CinemachineImpulseSource impact;

    private ItemBase itembase;
    [SerializeField] public Transform playerTransform;

    public bool isPickaxeAttack = false;
    private void Awake()
    {
        impact = GetComponent<CinemachineImpulseSource>();
    }

    void Start()
    {
        itembase = GetComponent<ItemBase>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isPickaxeAttack) return;

        if (other.TryGetComponent(out Stone stone))
        {
            ClientSend.HitStone(itembase.ItemAttributes.attack, stone.id);

            Vector3 spawnPos = Util.GetRandomPosition(Util.VectorUtil(stone.transform.position, playerTransform.position.y + .3f), 5);

            int randomAmount = Random.Range(1, 5);//Servera Tasınabilir

            ClientSend.SpawnResources(ItemType.Stone, randomAmount, spawnPos);

            impact.GenerateImpulse();
            isPickaxeAttack = false;
        }
        if (other.TryGetComponent(out Iron iron))
        {
            ClientSend.HitIron(itembase.ItemAttributes.attack, iron.id);

            Vector3 spawnPos = Util.GetRandomPosition(Util.VectorUtil(iron.transform.position, playerTransform.position.y + .3f), 5);

            int randomAmount = Random.Range(1, 5);//Servera Tasınabilir

            ClientSend.SpawnResources(ItemType.Iron, randomAmount, spawnPos);

            impact.GenerateImpulse();
            isPickaxeAttack = false;
        }
        if (other.TryGetComponent(out Tree tree))
        {
            ClientSend.HitTree(itembase.ItemAttributes.attack, tree.id);

            Vector3 spawnPos = Util.GetRandomPosition(Util.VectorUtil(tree.transform.position, playerTransform.position.y + .3f), 5);

            int randomAmount = Random.Range(1, 5);//Servera Tasınabilir

            ClientSend.SpawnResources(ItemType.Wood, randomAmount, spawnPos);

            impact.GenerateImpulse();
            isPickaxeAttack = false;
        }
        if (other.TryGetComponent(out Sheep sheep))
        {
            ClientSend.HitSheep(itembase.ItemAttributes.attack, sheep.sheepId);


            impact.GenerateImpulse();
            isPickaxeAttack = false;
        }
    }

}

