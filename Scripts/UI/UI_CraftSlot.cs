using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{
    protected override void Start()
    {
        base.Start();
    }

    public void SetUpCraftSlot(ItemData_Equipment _data)//设置CraftSlot的公开函数
    {
        if (_data == null)
            return;
        item.data = _data;
        itemImage.sprite = _data.icon;
        itemText.text = _data.name;

        if (itemText.text.Length > 12)
        {
            itemText.fontSize = 24 * .7f;
        }
        else
        {
            itemText.fontSize = 24;
        }

    }

    private void OnEnable()
    {
        UpdateSlot(item);
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        ui.craftWindow.SetUpCraftWIndow(item.data as ItemData_Equipment);
    }
}
