using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Button craftButton;

    [SerializeField] private Image[] materialsImage;

    public void SetUpCraftWIndow(ItemData_Equipment _data)
    {
        craftButton.onClick.RemoveAllListeners();//��ֹ���ֵ��Button���������ĺ���

        for (int i = 0; i < materialsImage.Length; i++)//�����е�UI����Ϊclear��ɫ
        {
            materialsImage[i].color = Color.clear;
            materialsImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }

        for (int i = 0; i < _data.craftingMaterials.Count; i++)
        {
            if (_data.craftingMaterials.Count > materialsImage.Length)
            {
                Debug.LogWarning("���ϱȸ�����������");
            }

            materialsImage[i].sprite = _data.craftingMaterials[i].data.icon;
            materialsImage[i].color = Color.white;
            TextMeshProUGUI materialsSlotText = materialsImage[i].GetComponentInChildren<TextMeshProUGUI>();

            materialsSlotText.text = _data.craftingMaterials[i].stackSize.ToString();
            materialsSlotText.color = Color.white;
        }

        itemIcon.sprite = _data.icon;
        itemName.text = _data.itemName;
        itemDescription.text = _data.GetDescription();


        craftButton.onClick.AddListener(() => Inventory.instance.CanCraft(_data, _data.craftingMaterials));
    }


}
