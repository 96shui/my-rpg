using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipmentType slotType;//����ô�õ���

    private void OnValidate()
    {
        gameObject.name = "Equipment slot -" + slotType.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if(item==null||item.data==null)
            return;

        //���װ���ۺ�ж��װ��
        Inventory.instance.Unequipment(item.data as ItemData_Equipment);
        Inventory.instance.Additem(item.data as ItemData_Equipment);

        ui.itemTooltip.HideToolTip();

        CleanUpSlot();
    }

}
