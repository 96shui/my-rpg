using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour,ISaveManager
{
    public static Inventory instance;

    public List<ItemData> startingItems;

    public List<InventoryItem> equipment;//inventoryItems���͵��б�
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;//��ItemDataΪKeyѰ��InventoryItem���ֵ�


    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictianory;

    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    [Header("Inventory UI")]

    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;

    private UI_ItemSlot[] inventoryItemSlot;//UI Slot������
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

        foreach (KeyValuePair<ItemData_Equipment,InventoryItem> item in equipmentDictionary)//���ַ�������ͬʱ�õ�key��value���浽item����
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)//���õ���key��ת����itemdata_equipment���͵�_item��type�Ա��õ����ڵ�key
            {
                oldIEquipment = item.Key;//��key�豣�����ⲿ��data������
                //equipment.Remove(item.Value);
                //equipmentDictionary.Remove(item.Key);
            }
        }//������foreach���value��key�޷����ⲿ��list���ֵ���в���

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

    public void Unequipment(ItemData_Equipment itemToRemove)//װ������ͬ���͵�װ��ʱ��ȥ����װ����װ��
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
            //�˲������ڽ���Ӧ���͵����������Ӧ�Ĳ���
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)//���ַ�������ͬʱ�õ�key��value���浽item����
            {
                if (item.Key.equipmentType == equipmentSlot[i].slotType)
                {
                    equipmentSlot[i].UpdateSlot(item.Value);
                }
            }

        }

        //�������UIû�и���Inventory�仯��bug
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

    private void AddToStash(ItemData _item)//��stash������ĺ���
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))//ֻ�����ַ��������ڲ��ҵ��Ƿ����key��Ӧvalue�Ƿ���ڵ�ͬʱ���ܹ�ͬʱ�õ�value�������������ò���value
        {
            value.AddStack();
        }//�ֵ��ʹ�ã�ͨ��ItemData���͵������ҵ�InventoryItem�����֮��Ӧ��ͬ�����͵�����
        else//��ʼʱ����û����ͬ���͵����壬�ʵ���else��Ϊ�˳�ʼ����棬ʹ���к���һ��������ֵ
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);//����б���ֻ��һ��
            stashDictionary.Add(_item, newItem);//ͬ��
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

    public bool CanAddItem()//ͨ��Inventory������Slot�ܴ�ŵ��������жԱȣ�ȷ���Ƿ��������µ�װ����װ����
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

    public ItemData_Equipment GetEquipment(EquipmentType _Type)//ͨ��Type�ҵ���Ӧ����װ��װ���ĺ���
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
        //ʹ��ҩƿ������ȴʱ��
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
    }//ʹ��ҩƿ����

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

    private List<ItemData> GetItemDataBase()//������е�equipmentData��IdName��data�ĺ���

    {
        List<ItemData> itemDataBase = new List<ItemData>();
        string[] assetsNames = AssetDatabase.FindAssets("", new[] { "Assets/Data/Items" });//���Ǹ���unity���ļ����ߵģ�Ҳ�����õ������е�Equipment���ļ���IdName

        foreach (string SOName in assetsNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);//����ͨ���ҵ����ļ����õ���Ӧ��λ��
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);//����ʵ��ʵ��ͨ��λ��ת���õ���Ӧ������
            itemDataBase.Add(itemData);//�������itemDataBase��
        }
        return itemDataBase;
    }

}
