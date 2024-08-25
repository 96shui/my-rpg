using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftList : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject craftSlotPrefab;//创建craftSlot的预制体
    [SerializeField] private List<ItemData_Equipment> craftEquipment;//一个将要导进craftList的data类型组
    [SerializeField] private Transform craftSlotParent;//便于寻找将要删除的craftSlot的父组件
 

    void Start()
    {
        transform.parent.GetChild(0).GetComponent<UI_CraftList>().SetupCraftList();
        SetupDefaultCraftWindow();
    }


    public void SetupCraftList()//将所有保存在其中的装备类型实例化craftslot并存入CraftList的函数
    {
        

        for (int i = 0; i < craftSlotParent.childCount; i++)//删除所有原来存在于其list里的slot
        {
            Destroy(craftSlotParent.GetChild(i).gameObject);
            
        }

        

        for (int i = 0; i < craftEquipment.Count; i++)
        {
            GameObject newSlot = Instantiate(craftSlotPrefab, craftSlotParent);//创建的实例往craftPartent里塞
            newSlot.GetComponent<UI_CraftSlot>().SetUpCraftSlot(craftEquipment[i]);
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetupCraftList();
    }


    public void SetupDefaultCraftWindow()
    {
        if (craftEquipment[0] != null)
        {
            GetComponentInParent<UI>().craftWindow.SetUpCraftWIndow(craftEquipment[0]);
        }
    }

   
}
