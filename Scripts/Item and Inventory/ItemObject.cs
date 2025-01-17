using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;
    [SerializeField] private Vector2 velocity;


    private void SetupVisuals()
    {
        if (itemData == null)
        {
            return;
        }

        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item object-" + itemData.itemName;
    }

    
    public void SetupItem(ItemData _itemData, Vector2 _velocity)//设置实例函数
    {
        itemData = _itemData;
        rb.velocity = _velocity;//设置速度

        SetupVisuals();
    }



    public void PickupItem()
    {
        if (!Inventory.instance.CanAddItem() && itemData.itemType == ItemType.Equipment)
        {
            rb.velocity = new Vector2(0, 7);
            PlayerManager.instance.player.fx.CreatePopUpText("空间不足");
            return;

        }

        Inventory.instance.Additem(itemData);
        Destroy(gameObject);
    }
}
