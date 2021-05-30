using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    private Inventory inventory;
    [Header("Char Equipment")]
    [SerializeField] private UIItem rightHandImage;
    [SerializeField] private UIItem leftHandImage;
    [SerializeField] private UIItem helmetImage;
    [SerializeField] private UIItem armorImage;
    [SerializeField] private UIItem bootsImage;



    [Header("Inventory")]
    [SerializeField] private UIItem axeImage;
    [SerializeField] private UIItem pickaxeImage;
    [SerializeField] private UIItem shieldImage;
    [SerializeField] private UIItem swordImage;
    [SerializeField] private UIItem bowImage;
    [SerializeField] private UIItem arrowImage;
    [SerializeField] private UIItem stoneImage;
    [SerializeField] private UIItem fabricImage;
    [SerializeField] private UIItem woodImage;
    [SerializeField] private UIItem ironImage;

    public void SetInventory(Inventory _inventory)
    {
        inventory = _inventory;
        inventory.OnItemListChanged += UIInventory_OnItemListChanged;
    }

    private void UIInventory_OnItemListChanged(object sender, EventArgs e)
    {
        ItemEventArgs itemEvent = (ItemEventArgs)e;
        ItemBase item = itemEvent.item;

        SetItemText(item);
    }

    public void SetItemText(ItemBase item)
    {
        UIItem uIItem = GetUIItem(item.itemType);
        switch (item.ItemActionType)
        {
            case ItemActionType.Craft:
                uIItem.text.SetText(item.Amount.ToString());
                break;
            case ItemActionType.Fight:
                uIItem.text.SetText("%" + item.ItemAttributes.durability);
                break;
            default:
                break;
        }
    }
    /*CHAR EQUIPMENT*/
    public void WearItem(ItemType itemType)
    {
        Sprite itemSprite = ItemAssets.instance.GetSprite(itemType);
        bool isRight = IsRightHandWeapon(itemType);
        bool isBow = itemType == ItemType.Bow;
        bool isWeapon = IsWeapon(itemType);

        if (isWeapon)
        {
            if (isRight)
            {
                if (isBow)
                {
                    leftHandImage.itemImage.sprite = ItemAssets.instance.GetSprite(ItemType.Arrow);//oto arrow sayısı girildi sol ele
                    leftHandImage.text.SetText(Inventory.instance.GetItemAmount(itemType).ToString());
                    leftHandImage.itemImage.color = Color.white;
                }

                rightHandImage.itemImage.sprite = itemSprite;
                rightHandImage.itemImage.color = Color.white;

                if (Inventory.instance.GetItemAttribute(itemType) != null)
                    rightHandImage.text.SetText(Inventory.instance.GetItemAttribute(itemType).durability.ToString());
            }
            else
            {
                leftHandImage.itemImage.sprite = itemSprite;
                leftHandImage.itemImage.color = Color.white;
                if (Inventory.instance.GetItemAttribute(itemType) != null)
                    leftHandImage.text.SetText(Inventory.instance.GetItemAttribute(itemType).durability.ToString());
            }
        }
        else
        {
            switch (itemType)
            {
                case ItemType.Helmet:
                    helmetImage.itemImage.sprite = itemSprite;
                    if (Inventory.instance.GetItemAttribute(itemType) != null)
                        helmetImage.text.SetText(Inventory.instance.GetItemAttribute(itemType).durability.ToString());
                    break;
                case ItemType.Armor:
                    armorImage.itemImage.sprite = itemSprite;
                    if (Inventory.instance.GetItemAttribute(itemType) != null)
                        armorImage.text.SetText(Inventory.instance.GetItemAttribute(itemType).durability.ToString());
                    break;
                case ItemType.Boots:
                    bootsImage.itemImage.sprite = itemSprite;
                    if (Inventory.instance.GetItemAttribute(itemType) != null)
                        bootsImage.text.SetText(Inventory.instance.GetItemAttribute(itemType).durability.ToString());
                    break;
                default:
                    break;
            }
        }

    }
    private bool IsRightHandWeapon(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Sword:
            case ItemType.Axe:
            case ItemType.Bow:
            case ItemType.Pickaxe:
                return true;
        }
        return false;
    }
    private bool IsWeapon(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Sword:
            case ItemType.Axe:
            case ItemType.Bow:
            case ItemType.Pickaxe:
            case ItemType.Shield:
                return true;
        }
        return false;
    }
    private UIItem GetUIItem(ItemType itemType)
    {
        switch (itemType)
        {
            default:
            case ItemType.Sword:
                return swordImage;
            case ItemType.Axe:
                return axeImage;
            case ItemType.Bow:
                return bowImage;
            case ItemType.Pickaxe:
                return pickaxeImage;
            case ItemType.Shield:
                return shieldImage;
            case ItemType.Arrow:
                return arrowImage;
            case ItemType.Wood:
                return woodImage;
            case ItemType.Stone:
                return stoneImage;
            case ItemType.Fabric:
                return fabricImage;
            case ItemType.Iron:
                return ironImage;
        }
    }
}
[System.Serializable]
public class UIItem
{
    public Image itemImage;
    public TextMeshProUGUI text;
}
