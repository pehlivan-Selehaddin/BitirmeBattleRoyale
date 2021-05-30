using Cinemachine;
using UnityEngine;


public class AxeControl : MonoBehaviour
{
    private CinemachineImpulseSource impact;

    private ItemBase itembase;

    [SerializeField] public Transform playerTransform;

    public bool isAxeAttack = false;
    private void Awake()
    {
        impact = GetComponent<CinemachineImpulseSource>();
    }
    private void Start()
    {
        itembase = GetComponent<ItemBase>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (!isAxeAttack) return;

        if (other.TryGetComponent(out Tree tree))
        {
            ClientSend.HitTree(itembase.ItemAttributes.attack, tree.id);

            Vector3 spawnPos = Util.GetRandomPosition(Util.VectorUtil(tree.transform.position, playerTransform.position.y+.3f), 5);

            int randomAmount = Random.Range(1, 5);//Servera Tasınabilir

            ClientSend.SpawnResources(ItemType.Wood, randomAmount, spawnPos);

            impact.GenerateImpulse();
            isAxeAttack = false;
        }
        if (other.TryGetComponent(out Sheep sheep))
        {
            ClientSend.HitSheep(itembase.ItemAttributes.attack, sheep.sheepId);


            impact.GenerateImpulse();
            isAxeAttack = false;
        }
    }
}
