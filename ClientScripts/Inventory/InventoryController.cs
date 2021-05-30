using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private UIInventory uiInventory;
    [SerializeField] private LocalCanvasManager canvasManager;
    [HideInInspector] public Inventory inventory;

    private Animator animator;

    [Header("Weapons")]
    [SerializeField] private GameObject sword;
    [SerializeField] private GameObject Axe;
    [SerializeField] private GameObject Bow;
    [SerializeField] private GameObject Pickaxe;
    [SerializeField] private GameObject Shield;

    private const int AXE_LAYER = 1;
    private const int SWORD_LAYER = 2;
    private const int PICKAXE_LAYER = 3;
    private const int BOW_LAYER = 4;
    private void Awake()
    {
        animator = GetComponent<Animator>();

        inventory = new Inventory();
        uiInventory.SetInventory(inventory);
        canvasManager.SetPlayerInfo(inventory,OnWeaponChange);
    }
    public void OnWeaponChange(int currentItem)
    {
        Debug.Log("Onweaponchage");
        ItemType itemType = (ItemType)currentItem;
        animator.SetTrigger("changeWeapon");

        switch (itemType)
        {
            case ItemType.Sword:
                //setactive sword gameobject
                SetAnimationLayer(SWORD_LAYER);
                break;
            case ItemType.Axe:
                //setactive axe gameobject
                SetAnimationLayer(AXE_LAYER);
                break;
            case ItemType.Bow:
                //setactive bow gameobject
                SetAnimationLayer(BOW_LAYER);
                break;
            case ItemType.Pickaxe:
                //setactive pickaxe gameobject
                SetAnimationLayer(PICKAXE_LAYER);
                break;
            case ItemType.Shield:
                //setactive shield gameobject
                SetAnimationLayer(SWORD_LAYER);
                break;
        }
    }
    private void SetWeapon(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Sword:
                sword.SetActive(true);
                Axe.SetActive(false);
                Pickaxe.SetActive(false);
                Bow.SetActive(false);
                //Shield.SetActive(true);
                break;
            case ItemType.Axe:
                sword.SetActive(false);
                Axe.SetActive(true);
                Pickaxe.SetActive(false);
                Bow.SetActive(false);
                break;
            case ItemType.Bow:
                sword.SetActive(false);
                Axe.SetActive(false);
                Pickaxe.SetActive(false);
                Bow.SetActive(true);
                break;
            case ItemType.Pickaxe:
                sword.SetActive(false);
                Axe.SetActive(false);
                Pickaxe.SetActive(true);
                Bow.SetActive(false);
                break;
        }
    }
    
    public void SetAnimationLayer(int index)
    {
        for (int i = 0; i < animator.layerCount-2; i++)
        {
            int weight = i == index ? 1 : 0;
            animator.SetLayerWeight(i, weight);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ItemBase item = GetComponent<ItemBase>();
            inventory.AddItem(item);//Inventory item ekleme
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log(inventory.GetItemList().Count);
            foreach (var item in inventory.GetItemList())
            {
                Debug.Log(item.Amount);
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            uiInventory.WearItem(ItemType.Armor);
        }
    }
}
