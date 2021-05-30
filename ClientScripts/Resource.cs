using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : ItemBase
{
    private WaitForSeconds delay;
    public string resourceId;
    public void InitializedValues(ItemType _itemType, int _amount)
    {
        resourceId = Guid.NewGuid().ToString();
        itemType = _itemType;
        Amount = _amount;
        ItemActionType = ItemActionType.Craft;

        delay = new WaitForSeconds(.1f);
        StartCoroutine(Turn());
    }
    IEnumerator Turn()
    {
        while (true)
        {
            yield return delay;
            transform.Rotate(Vector3.right * 5);
        }
    }
    private bool isSend = false;
    private void OnTriggerEnter(Collider other)
    {
        if (isSend) return;
        if (other.TryGetComponent(out InventoryController inventoryController))
        {
            isSend = true;
            inventoryController.inventory.AddItem(this);

            ClientSend.PickUpResource(resourceId);
        }
    }
}
