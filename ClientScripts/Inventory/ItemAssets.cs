using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    [SerializeField] private Sprite axeSprite;
    [SerializeField] private Sprite pickaxeSprite;
    [SerializeField] private Sprite shieldSprite;
    [SerializeField] private Sprite swordSprite;
    [SerializeField] private Sprite bowSprite;
    [SerializeField] private Sprite arrowSprite;
    [SerializeField] private Sprite stoneSprite;
    [SerializeField] private Sprite fabricSprite;
    [SerializeField] private Sprite woodSprite;
    [SerializeField] private Sprite ironSprite;
    [SerializeField] private Sprite helmetSprite;
    [SerializeField] private Sprite armorSprite;
    [SerializeField] private Sprite bootsSprite;
    public static ItemAssets instance { get; private set; }
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public Sprite GetSprite(ItemType itemType)
    {
        switch (itemType)
        {
            default:
            case ItemType.Sword:
                return swordSprite;
            case ItemType.Axe:
                return axeSprite;
            case ItemType.Bow:
                return bowSprite;
            case ItemType.Pickaxe:
                return pickaxeSprite;
            case ItemType.Shield:
                return shieldSprite;
            case ItemType.Arrow:
                return arrowSprite;
            case ItemType.Wood:
                return woodSprite;
            case ItemType.Stone:
                return stoneSprite;
            case ItemType.Fabric:
                return fabricSprite;
            case ItemType.Helmet:
                return helmetSprite;
            case ItemType.Armor:
                return armorSprite;
            case ItemType.Boots:
                return bootsSprite;
            case ItemType.Iron:
                return ironSprite;
        }
    }
}
