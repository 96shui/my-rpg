using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour,IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler
{

    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;

    protected UI ui;
    public InventoryItem item;

    protected virtual void Start()
    {
        ui = GetComponentInParent<UI>();
    }

    public void UpdateSlot(InventoryItem _newItem)
    {
        item = _newItem;

        itemImage.color = Color.white;

        if (item != null)
        {
            itemImage.sprite = item.data.icon;

            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
        }
        else
        {
            itemText.text = "";
        }
    }

    public void CleanUpSlot()//解决出现UI没有跟着Inventory变化的bug
    {
        item = null;

        itemImage.sprite = null;
        itemImage.color = Color.clear;

        itemText.text = "";
    }


    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null)
            return;

        if (Input.GetKey(KeyCode.LeftControl)) 
        {
            Inventory.instance.RemoveItem(item.data);
            return;

        }

        if (item.data.itemType == ItemType.Equipment)
            Inventory.instance.EquipItem(item.data);

        ui.itemTooltip.HideToolTip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null) return;


        Vector2 mousePosition = Input.mousePosition;

        float xOffset = 0;

        if (mousePosition.x > 450)
        {
            xOffset = -230;
        }
        else
        {
            xOffset = 230;
        }

        ui.itemTooltip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + 150);

        ui.itemTooltip.ShowToolTip(item.data as ItemData_Equipment);
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        if (item == null) return;

        ui.itemTooltip.HideToolTip();
    }
}
