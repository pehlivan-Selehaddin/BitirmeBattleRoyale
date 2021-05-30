using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalCanvasManager : MonoBehaviour
{
    private Inventory playerInventory;
    private Action<int> OnWeaponChange;
    public void SetPlayerInfo(Inventory _inventory, Action<int> weaponChange)
    {
        playerInventory = _inventory;
        OnWeaponChange = weaponChange;
    }


    #region Toggle Inventory
    [SerializeField] private RectTransform charEquipment;
    [SerializeField] private RectTransform inventory;
    [SerializeField] private RectTransform craftWindow;
    private bool isShowingInventory = false;
    private bool isShowingCraft = false;

    private Vector2 charEquipmentHidePos = new Vector2(-235, 0);



    private Vector2 inventoryHidePos = new Vector2(325, 0);
    private Vector2 craftUIHidePos = new Vector2(325, 0);

    private WaitForSeconds deltaTime;
    private void Awake()
    {
        deltaTime = new WaitForSeconds(Time.deltaTime);
    }

    public void ToggleInventory()
    {
        isShowingInventory = !isShowingInventory;
        StopAllCoroutines();
        StartCoroutine(AnimateInventoryUI());
    }
    private float speed = 2000;
    private IEnumerator AnimateInventoryUI()
    {
        if (isShowingInventory)
        {
            while (inventory.anchoredPosition != Vector2.zero)
            {
                yield return deltaTime;

                inventory.anchoredPosition = Vector3.MoveTowards(inventory.anchoredPosition, Vector2.zero, Time.deltaTime * speed);
                charEquipment.anchoredPosition = Vector3.MoveTowards(charEquipment.anchoredPosition, Vector2.zero, Time.deltaTime * speed);
            }
        }
        else
        {
            while (inventory.anchoredPosition != charEquipmentHidePos)
            {
                yield return deltaTime;

                inventory.anchoredPosition = Vector3.MoveTowards(inventory.anchoredPosition, inventoryHidePos, Time.deltaTime * speed);
                charEquipment.anchoredPosition = Vector3.MoveTowards(charEquipment.anchoredPosition, charEquipmentHidePos, Time.deltaTime * speed);
            }
        }
    }

    public void ToggleCraftBtn()
    {
        isShowingCraft = !isShowingCraft;
        StopAllCoroutines();
        StartCoroutine(AnimateCraftingUI());
    }
    private IEnumerator AnimateCraftingUI()
    {
        if (isShowingCraft)
        {
            if (inventory.anchoredPosition == Vector2.zero)
            {
                while (craftWindow.anchoredPosition != Vector2.zero)
                {
                    yield return deltaTime;
                    craftWindow.anchoredPosition = Vector3.MoveTowards(craftWindow.anchoredPosition, Vector2.zero, Time.deltaTime * speed);
                }
            }
            else
            {
                while (inventory.anchoredPosition != Vector2.zero)
                {
                    yield return deltaTime;

                    inventory.anchoredPosition = Vector3.MoveTowards(inventory.anchoredPosition, Vector2.zero, Time.deltaTime * speed);
                    craftWindow.anchoredPosition = Vector3.MoveTowards(craftWindow.anchoredPosition, Vector2.zero, Time.deltaTime * speed);
                }
            }

        }
        else
        {
            while (inventory.anchoredPosition != charEquipmentHidePos)
            {
                yield return deltaTime;

                inventory.anchoredPosition = Vector3.MoveTowards(inventory.anchoredPosition, inventoryHidePos, Time.deltaTime * speed);
                craftWindow.anchoredPosition = Vector3.MoveTowards(craftWindow.anchoredPosition, craftUIHidePos, Time.deltaTime * speed);
            }
        }
    }
    #endregion


    [SerializeField] private GameObject bag;
    public void ToggleBag()
    {
        if (bag.activeSelf)
            bag.SetActive(false);
        else
            bag.SetActive(true);
    }
    public void ChooseWeapon(int weaponType)
    {
        //weapontType 0 Sword , 1 Axe, 2 Bow , 3 Pickaxe
        //playerInventory.
        ItemBase item = playerInventory.GetItem((ItemType)weaponType);

        
        if (item != null && item.ItemAttributes.durability > 0)
        {
            OnWeaponChange?.Invoke(weaponType);
        }
    }
}
