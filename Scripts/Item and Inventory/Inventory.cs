using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour,ISaveManager
{
    public static Inventory instance;

    public List<ItemData> startingItems;

    public List<InventoryItem> equipment;//inventoryItems类型的列表
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;//以ItemData为Key寻找InventoryItem的字典


    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictianory;

    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    [Header("Inventory UI")]

    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;

    private UI_ItemSlot[] inventoryItemSlot;//UI Slot的数组
    private UI_ItemSlot[] stashItemSlot;
    private UI_EquipmentSlot[] equipmentSlot;
    private UI_StatSlot[] statSlot;

    [Header("Items cooldown")]
    private float lastTimeUsedFlask;
    private float lastTimeUsedArmor;

    public float flaskCooldown { get; private set; }
    private float armorCooldown;

    [Header("Data base")]
    public List<InventoryItem> loadedItems;
    public List<ItemData_Equipment> loadedEquipment;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        statSlot = statSlotParent.GetComponentsInChildren<UI_StatSlot>();
    }

    private void Start()
    {
        inventory = new List<InventoryItem>();
        inventoryDictianory = new Dictionary<ItemData, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();

        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlot = stashlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
        

        AddStartingItems();
    }

    private void AddStartingItems()
    {
        foreach (ItemData_Equipment item in loadedEquipment)
        {
            EquipItem(item);
        }


        if (loadedItems.Count > 0)
        {
            foreach(InventoryItem item in loadedItems)
            {
                for (int i = 0; i < item.stackSize; i++)
                {
                    Additem(item.data);
                }
            }
            return;
        }

        for (int i = 0; i < startingItems.Count; i++)
        {
            if(startingItems[i] != null)
            Additem(startingItems[i]);
        }
    }

    public void EquipItem(ItemData _item)
    {
        ItemData_Equipment newEquipment = _item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemData_Equipment oldIEquipment = null;

        foreach (KeyValuePair<ItemData_Equipment,InventoryItem> item in equipmentDictionary)//这种方法可以同时拿到key和value保存到item里面
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)//将拿到的key与转换成itemdata_equipment类型的_item的type对比拿到存在的key
            {
                oldIEquipment = item.Key;//此key需保存在外部的data类型里
                //equipment.Remove(item.Value);
                //equipmentDictionary.Remove(item.Key);
            }
        }//好像用foreach里的value和key无法对外部的list和字典进行操作

        if (oldIEquipment != null)
        {
            Unequipment(oldIEquipment);
            Additem(oldIEquipment);
        }


        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifiers();
        RemoveItem(_item);

    }

    public void Unequipment(ItemData_Equipment itemToRemove)//装备其他同类型的装备时。去除已装备的装备
    {
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(itemToRemove);
            itemToRemove.RemoveModifiers();

        }
    }



    private void UpdateUISlot()
    {

        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            //此步骤用于将对应类型的武器插入对应的槽内
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)//这种方法可以同时拿到key和value保存到item里面
            {
                if (item.Key.equipmentType == equipmentSlot[i].slotType)
                {
                    equipmentSlot[i].UpdateSlot(item.Value);
                }
            }

        }

        //解决出现UI没有跟着Inventory变化的bug
        for (int i = 0; i < inventoryItemSlot.Length; i++)
        {
            inventoryItemSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < stashItemSlot.Length; i++)
        {
            stashItemSlot[i].CleanUpSlot();
        }


        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }

        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }

        UpdateStatsUI();
    }

    public void UpdateStatsUI()
    {
        for (int i = 0; i < statSlot.Length; i++)
        {
            statSlot[i].UpdateStatValueUI();
        }
    }

    public void Additem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment && CanAddItem())
        {
            AddToInventory(_item);

        }
        else if (_item.itemType == ItemType.Material)
        {
            AddToStash(_item);
        }


        UpdateUISlot();
    }

    private void AddToStash(ItemData _item)//向stash加物体的函数
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))//只有这种方法才能在查找到是否存在key对应value是否存在的同时，能够同时拿到value，其他方法的拿不到value
        {
            value.AddStack();
        }//字典的使用，通过ItemData类型的数据找到InventoryItem里的与之对应的同样类型的数据
        else//初始时由于没有相同类型的物体，故调用else是为了初始化库存，使其中含有一个基本的值
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);//填进列表里只有一次
            stashDictionary.Add(_item, newItem);//同上
        }
    }

    private void AddToInventory(ItemData _item)
    {
        if (inventoryDictianory.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictianory.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item)
    {
        if (inventoryDictianory.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventory.Remove(value);
                inventoryDictianory.Remove(_item);
            }
            else
            {
                value.RemoveStack();
            }
        }

        if(stashDictionary.TryGetValue(_item,out InventoryItem stashValue))
        {
            if(stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(_item);
            }
            else
            {
                stashValue.RemoveStack();
            }
        }
        UpdateUISlot();
    }

    public bool CanAddItem()//通过Inventory数量和Slot能存放的数量进行对比，确定是否可以添加新的装备到装备槽
    {
        if (inventory.Count >= inventoryItemSlot.Length)
        {
            Debug.Log("No more space");
            return false;
        }
        return true;
    }

    public bool CanCraft(ItemData_Equipment _itemToCraft,List<InventoryItem> _requiredMaterials)
    {
        List<InventoryItem> materialsToRemove = new List<InventoryItem>();

        for (int i = 0; i < _requiredMaterials.Count; i++)
        {
            if (stashDictionary.TryGetValue(_requiredMaterials[i].data, out InventoryItem stashValue))
            {
                if(stashValue.stackSize < _requiredMaterials[i].stackSize)
                {
                    Debug.Log("not enough materials");
                    return false;
                }
                else
                {
                    materialsToRemove.Add(stashValue);
                }
            }
            else
            {
                Debug.Log("not enough materials");
                return false;
            }
        }

        for (int i = 0;i < materialsToRemove.Count; i++)
        {
            RemoveItem(materialsToRemove[i].data);
        }
        Additem(_itemToCraft);
        Debug.Log("Here is your item" + _itemToCraft);
        return true;
    }

    public List<InventoryItem> GetEquipmentList() => equipment;

    public List<InventoryItem> GetStashList() => stash;

    public ItemData_Equipment GetEquipment(EquipmentType _Type)//通过Type找到对应的已装备装备的函数
    {
        ItemData_Equipment equipedItem = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
            if (item.Key.equipmentType == _Type)
            {
                equipedItem = item.Key;
            }

        return equipedItem;
    }


    public void UseFlask()
    {
        ItemData_Equipment currentFlask = GetEquipment(EquipmentType.Flask);

        if (currentFlask == null)
            return;
        //使用药瓶设置冷却时间
        bool canUseFlask = Time.time > lastTimeUsedFlask + flaskCooldown;

        if (canUseFlask)
        {
            flaskCooldown = currentFlask.itemCooldown;
            currentFlask.Effect(null);
            lastTimeUsedFlask = Time.time;
        }
        else
        {
            Debug.Log("Flask is Cooldown");
        }
    }//使用药瓶函数

    public bool CanUseArmor()
    {
        ItemData_Equipment currentArmor = GetEquipment(EquipmentType.Armor);

        if (Time.time > lastTimeUsedArmor + armorCooldown)
        {
            armorCooldown = currentArmor.itemCooldown;
            lastTimeUsedArmor = Time.time;
            return true;
        }

        Debug.Log("Armor on cooldown");
        return false;
    }

    public void LoadData(GameData _data)
    {
        foreach(KeyValuePair<string,int> pair in _data.inventory)
        {
            foreach(var item in GetItemDataBase())
            {
                if(item != null && item.itemId == pair.Key)
                {
                    InventoryItem itemToLoad = new InventoryItem(item);
                    itemToLoad.stackSize = pair.Value;

                    loadedItems.Add(itemToLoad);
                }
            }
        }

        foreach (string loadedItemId in _data.equipmentId)
        {
            foreach (var item in GetItemDataBase())
            {
                if (item != null && item.itemId == loadedItemId)
                {
                    loadedEquipment.Add(item as ItemData_Equipment);
                }
            }
        }

    }

    public void SaveData(ref GameData _data)
    {
        _data.inventory.Clear();
        _data.equipmentId.Clear();

        foreach(KeyValuePair<ItemData,InventoryItem> pair in inventoryDictianory)
        {
            _data.inventory.Add(pair.Key.itemId,pair.Value.stackSize);
        }

        foreach (KeyValuePair<ItemData, InventoryItem> pair in stashDictionary)
        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> pair in equipmentDictionary)
        {
            _data.equipmentId.Add(pair.Key.itemId);
        }


    }

    private List<ItemData> GetItemDataBase()//获得所有的equipmentData的IdName和data的函数

    {
        List<ItemData> itemDataBase = new List<ItemData>();
        string[] assetsNames = AssetDatabase.FindAssets("", new[] { "Assets/Data/Items" });//这是根据unity的文件夹走的，也就是拿到了所有的Equipment的文件名IdName

        foreach (string SOName in assetsNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);//这是通过找到的文件名拿到对应的位置
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);//这是实打实的通过位置转换拿到相应的数据
            itemDataBase.Add(itemData);//将数据填到itemDataBase里
        }
        return itemDataBase;
    }

}
