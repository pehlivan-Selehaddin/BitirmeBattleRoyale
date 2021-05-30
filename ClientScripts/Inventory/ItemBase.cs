using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    public ItemType itemType;

    public ItemActionType ItemActionType;

    public ItemAttribute ItemAttributes;

    public int Amount;
}
public enum ItemType
{
    Sword,
    Axe,
    Bow,
    Pickaxe,
    Shield,
    Arrow,
    Wood,
    Stone,
    Fabric,
    Iron,
    Helmet,
    Armor,
    Boots
}
public enum ItemActionType
{
    Craft,
    Fight
}
[System.Serializable]
public class ItemAttribute
{ 
    public int durability;
    public int attack;
    public int defence;
    public int healthBonus;

}