using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftList : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject craftSlotPrefab;//����craftSlot��Ԥ����
    [SerializeField] private List<ItemData_Equipment> craftEquipment;//һ����Ҫ����craftList��data������
    [SerializeField] private Transform craftSlotParent;//����Ѱ�ҽ�Ҫɾ����craftSlot�ĸ����
 

    void Start()
    {
        transform.parent.GetChild(0).GetComponent<UI_CraftList>().SetupCraftList();
        SetupDefaultCraftWindow();
    }


    public void SetupCraftList()//�����б��������е�װ������ʵ����craftslot������CraftList�ĺ���
    {
        

        for (int i = 0; i < craftSlotParent.childCount; i++)//ɾ������ԭ����������list���slot
        {
            Destroy(craftSlotParent.GetChild(i).gameObject);
            
        }

        

        for (int i = 0; i < craftEquipment.Count; i++)
        {
            GameObject newSlot = Instantiate(craftSlotPrefab, craftSlotParent);//������ʵ����craftPartent����
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
