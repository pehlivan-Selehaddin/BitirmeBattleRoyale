using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public static Inventory instance { get; private set; }
    private List<ItemBase> inventoryItemList;
    private List<ItemBase> craftItemList;
    public Inventory()
    {
        if (instance == null)
        {
            instance = this;
            inventoryItemList = new List<ItemBase>();
            craftItemList = new List<ItemBase>();
        }
    }

    public ItemBase GetItem(ItemType itemType)
    {
        for (int i = 0; i < inventoryItemList.Count; i++)
        {
            if (inventoryItemList[i].itemType == itemType)
            {
                return inventoryItemList[i];
            }
        }
        return null;
    }

    public event EventHandler OnItemListChanged;

    public void AddItem(ItemBase item)
    {
        ItemEventArgs itemEvent = new ItemEventArgs();

        if (inventoryItemList.Count > 0)
        {
            bool isItemAdded = false;
            for (int i = 0; i < inventoryItemList.Count; i++)
            {
                if (inventoryItemList[i].itemType == item.itemType)
                {
                    switch (inventoryItemList[i].ItemActionType)
                    {
                        case ItemActionType.Craft:
                            inventoryItemList[i].Amount += item.Amount;
                            break;
                        case ItemActionType.Fight:
                            inventoryItemList[i].ItemAttributes.durability += item.ItemAttributes.durability;
                            if (inventoryItemList[i].ItemAttributes.durability > 100) inventoryItemList[i].ItemAttributes.durability = 100;
                            break;
                        default:
                            break;
                    }
                    itemEvent.item = inventoryItemList[i];
                    isItemAdded = true;
                }
            }
            if (!isItemAdded)
            {
                inventoryItemList.Add(item);
                itemEvent.item = item;
            }
        }
        else
        {
            inventoryItemList.Add(item);
            itemEvent.item = item;
        }
        OnItemListChanged?.Invoke(this, itemEvent);
    }

    public List<ItemBase> GetItemList()
    {
        return inventoryItemList;
    }
    public void RemoveItem(ItemBase item)
    {
        inventoryItemList.Remove(item);
    }

    public ItemAttribute GetItemAttribute(ItemType itemType)
    {
        List<ItemBase> itemBases = GetItemList();
        for (int i = 0; i < itemBases.Count; i++)
        {
            if (itemBases[i].itemType == itemType)
            {
                return itemBases[i].ItemAttributes;
            }
        }
        return null;
    }
    public float GetItemAmount(ItemType itemType)
    {
        List<ItemBase> itemBases = GetItemList();
        for (int i = 0; i < itemBases.Count; i++)
        {
            if (itemBases[i].itemType == itemType)
            {
                return itemBases[i].Amount;
            }
        }
        return 0;
    }
    /*CRAFT*/
    public void AddItemToCraft(ItemBase item)
    {
        bool isResource = IsResource(item.itemType);

        if (!isResource) return;

        craftItemList.Add(item);
    }
    public void RemoveItemFromCraft(ItemBase item)
    {
        if (!craftItemList.Contains(item)) return;

        bool isResource = IsResource(item.itemType);

        if (!isResource) return;

        craftItemList.Remove(item);
    }
    private void OnCraftListAdded()
    {
        if (craftItemList.Count != 2) return;

        ItemType firstItemType = craftItemList[0].itemType;
        float firstAmount = craftItemList[0].Amount;

        ItemType secondItemType = craftItemList[1].itemType;
        float secondAmount = craftItemList[1].Amount;

        if (firstItemType == ItemType.Wood && secondItemType == ItemType.Stone)//ODUN TAŞ
        {
            if (firstAmount == 2 && secondAmount == 3)
            {
                ItemBase itemBase = new ItemBase() { itemType = ItemType.Sword, ItemAttributes = new ItemAttribute() { attack = 20, durability = 50 }, Amount = 1 };//odun ve taş kılıç
                AddItem(itemBase);
            }
            else
            if (firstAmount == 4 && secondAmount == 2)
            {
                ItemBase itemBase = new ItemBase() { itemType = ItemType.Axe, ItemAttributes = new ItemAttribute() { attack = 15, durability = 50 }, Amount = 1 };//odun ve taş Axe
                AddItem(itemBase);
            }
            else
            if (firstAmount == 3 && secondAmount == 4)
            {
                ItemBase itemBase = new ItemBase() { itemType = ItemType.Pickaxe, ItemAttributes = new ItemAttribute() { attack = 15, durability = 50 }, Amount = 1 };//odun ve taş Pickaxe
                AddItem(itemBase);
            }
            else
            if (firstAmount == 2 && secondAmount == 1)
            {
                ItemBase itemBase = new ItemBase() { itemType = ItemType.Arrow, ItemAttributes = new ItemAttribute() { attack = 15, durability = 50 }, Amount = 3 };//odun ve taş arrow
                AddItem(itemBase);
            }
            else
            if (firstAmount == 1 && secondAmount == 1)
            {
                ItemBase itemBase = new ItemBase() { itemType = ItemType.Helmet, ItemAttributes = new ItemAttribute() { defence = 30, healthBonus = 40, durability = 50 }, Amount = 1 };
                AddItem(itemBase);
            }
            else
            if (firstAmount == 3 && secondAmount == 3)
            {
                ItemBase itemBase = new ItemBase() { itemType = ItemType.Armor, ItemAttributes = new ItemAttribute() { defence = 50, healthBonus = 100, durability = 50 }, Amount = 1 };
                AddItem(itemBase);
            }
            else
            if (firstAmount == 2 && secondAmount == 2)
            {
                ItemBase itemBase = new ItemBase() { itemType = ItemType.Boots, ItemAttributes = new ItemAttribute() { defence = 25, healthBonus = 35, durability = 50 }, Amount = 1 };
                AddItem(itemBase);
            }
            else
            if (firstAmount == 4 && secondAmount == 3)
            {
                ItemBase itemBase = new ItemBase() { itemType = ItemType.Shield, ItemAttributes = new ItemAttribute() { defence = 120, healthBonus = 35, durability = 50 }, Amount = 1 };
                AddItem(itemBase);
            }
        }
        else if (firstItemType == ItemType.Wood && secondItemType == ItemType.Iron)//ODUN DEMIR
        {
            if (firstAmount == 2 && secondAmount == 3)
            {
                ItemBase itemBase = new ItemBase() { itemType = ItemType.Sword, ItemAttributes = new ItemAttribute() { attack = 30, durability = 50 }, Amount = 1 };
                AddItem(itemBase);
            }
            else
            if (firstAmount == 4 && secondAmount == 2)
            {
                ItemBase itemBase = new ItemBase() { itemType = ItemType.Axe, ItemAttributes = new ItemAttribute() { attack = 25, durability = 50 }, Amount = 1 };
                AddItem(itemBase);
            }
            else if (firstAmount == 3 && secondAmount == 4)
            {
                ItemBase itemBase = new ItemBase() { itemType = ItemType.Pickaxe, ItemAttributes = new ItemAttribute() { attack = 20, durability = 50 }, Amount = 1 };
                AddItem(itemBase);
            }
            else if (firstAmount == 4 && secondAmount == 3)
            {
                ItemBase itemBase = new ItemBase() { itemType = ItemType.Shield, ItemAttributes = new ItemAttribute() { defence = 150, healthBonus = 15, durability = 50 }, Amount = 1 };
                AddItem(itemBase);
            }
        }
        else if (firstItemType == ItemType.Wood && secondItemType == ItemType.Fabric)// ODUN YÜN
        {
            if (firstAmount == 4 && secondAmount == 3)
            {
                ItemBase itemBase = new ItemBase() { itemType = ItemType.Bow, ItemAttributes = new ItemAttribute() { attack = 30, durability = 50 }, Amount = 1 };
                AddItem(itemBase);
            }
        }
        else if (firstItemType == ItemType.Iron && secondItemType == ItemType.Fabric)// DEMİR YÜN
        {
            if (firstAmount == 4 && secondAmount == 3)
            {
                ItemBase itemBase = new ItemBase() { itemType = ItemType.Bow, ItemAttributes = new ItemAttribute() { attack = 50, durability = 50 }, Amount = 1 };
                AddItem(itemBase);
            }
            else if (firstAmount == 1 && secondAmount == 2)
            {
                ItemBase itemBase = new ItemBase() { itemType = ItemType.Helmet, ItemAttributes = new ItemAttribute() { defence = 40, healthBonus = 50, durability = 50 }, Amount = 1 };
                AddItem(itemBase);
            }
            else if (firstAmount == 3 && secondAmount == 6)
            {
                ItemBase itemBase = new ItemBase() { itemType = ItemType.Armor, ItemAttributes = new ItemAttribute() { defence = 60, healthBonus = 130, durability = 50 }, Amount = 1 };
                AddItem(itemBase);
            }
            else if (firstAmount == 2 && secondAmount == 4)
            {
                ItemBase itemBase = new ItemBase() { itemType = ItemType.Boots, ItemAttributes = new ItemAttribute() { defence = 35, healthBonus = 50, durability = 50 }, Amount = 1 };
                AddItem(itemBase);
            }
        }

    }

    private bool IsResource(ItemType itemType)
    {
        switch (itemType)
        {

            case ItemType.Wood:
            case ItemType.Stone:
            case ItemType.Fabric:
            case ItemType.Iron:
                return true;
            default: return false;
        }
    }
}
public class ItemEventArgs : EventArgs
{
    public ItemBase item;
}