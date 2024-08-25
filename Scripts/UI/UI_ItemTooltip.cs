using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ItemTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescription;

    [SerializeField] private int defaultFontSize = 32;

    public void ShowToolTip(ItemData_Equipment item)//将item里面的数据读取到对应的text上并显示此组件
    {
        if (item == null)
            return;

        itemNameText.text = item.itemName;
        itemTypeText.text = item.equipmentType.ToString();
        itemDescription.text = item.GetDescription();

        if (itemNameText.text.Length > 12)//使标题超过最大尺寸时字体缩小
        {
            itemNameText.fontSize = itemNameText.fontSize * .7f;
        }
        else
        {
            itemNameText.fontSize = defaultFontSize;
        }

        gameObject.SetActive(true);
    }
    public void HideToolTip()//关闭此组件
    {
        itemNameText.fontSize = defaultFontSize;//防止字体越来越小
        gameObject.SetActive(false);
    }

}
