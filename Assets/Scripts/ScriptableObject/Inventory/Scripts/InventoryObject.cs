using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;
using System;

[CreateAssetMenu(fileName ="new Inventory Object", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject //, ISerializationCallbackReceiver
{
    public string savePath;
    public ItemDatabaseObject database;
    public int size;
    public Inventory container;
    

    public bool AddItem(Item _item, int _amount)
    {
        // 즉시 사용 아이템일 경우 -- 받을 수 없음(즉시 적용)
        if (_item.isConsumable)
        {
            _item.UseItem();
            return true;
        }

        // 빈공간이 있는지 확인
        if (EmptySlotCount <= 0)
        {
            if (!database.GetItem[_item.id].isStack) // 빈공간이 없는데 스택이 불가능한 아이템이 들어오는 경우 --  받을 수 없음
            {
                return false;
            }
            else
            {
                InventorySlot slot = FindItemOnInventory(_item);
                if (slot != null) // 빈공간이 없지만 스택이 가능하고 이미 존재하는 아이템인경우  -- 받을 수 있음
                {
                    slot.AddAmount(_amount);
                    return true;
                }
                else   // 빈공간이 없지만 스택이 가능하고 이미 존재하지 않는 아이템인경우  -- 받을 수 없음
                {
                    return false;
                }
            }
        }
        else // 빈공간이 있는경우
        {
            if (!database.GetItem[_item.id].isStack) // 빈공간이 있고 스택이 불가능한 아이템이 들어오는 경우 --  받을 수 있음
            {
                SetEmptySlot(_item, _amount);
                return true;
            }
            else // 빈공간이 있고 스택이 가능한 아이템이 들어오는 경우
            {
                InventorySlot slot = FindItemOnInventory(_item);
                Debug.Log(slot);
                if (slot != null) // 빈공간이 있고 스택이 가능하고 이미 존재하는 아이템인경우  -- 받을 수 있음
                {
                    slot.AddAmount(_amount);
                    return true;
                }
                else   // 빈공간이 없지만 스택이 가능하고 이미 존재하지 않는 아이템인경우  -- 받을 수 있음
                {
                    SetEmptySlot(_item, _amount);
                    return true;
                }
            }
        }
        //// 스택이 불가능한 아이템인 경우
        //if (!_item.isStack)
        //{
        //    SetEmptySlot( _item,  _amount);
        //    return;
        //}

        //// 그외 스택이 가능한 아이템이면서 && 이미 존재하는경우
        //for (int i = 0; i < container.items.Length; i++)
        //{
        //    if (container.items[i].item.id == _item.id)
        //    {
        //        container.items[i].AddAmount(_amount);
        //        return;
        //    }
        //}

        //// 스택이 가능하지만 존재하지 않는경우
        //SetEmptySlot(_item, _amount);
    }

    public int EmptySlotCount
    {
        get
        {
            int counter = 0;
            for (int i = 0; i < container.items.Length; i++)
            {
                if (container.items[i].item.id < 0)
                    counter++;
            }
            return counter;
        }
    }

    public InventorySlot FindItemOnInventory(Item _item)
    {
        for (int i = 0; i < container.items.Length; i++)
        {
            if(container.items[i].item.id == _item.id)
            {
                return container.items[i];
            }
        }
        return null;
    }

    public InventorySlot SetEmptySlot(Item _item, int _amount) // 빈공간을 찾아서 아이템 설정
    {
        for (int i = 0; i < container.items.Length; i++)
        {
            if(container.items[i].item.id <= -1)
            {
                container.items[i].UpdateSlots(_item, _amount);
                return container.items[i];
            }
        }

        // setup func when inventory full
        return null;
    }

    public void SwapItem(InventorySlot item1, InventorySlot item2)
    {
        if (item2.CanPlaceInSlot(item1.ItemObject) && item1.CanPlaceInSlot(item2.ItemObject))
        {
            InventorySlot temp = new InventorySlot(item2.item, item2.amount);
            item2.UpdateSlots(item1.item, item1.amount);
            item1.UpdateSlots(temp.item, temp.amount);
        }
    }

    public void RemoveItem(Item _item)
    {
        for (int i = 0; i < container.items.Length; i++)
        {
            if (container.items[i].item == _item)
            {
                container.items[i].UpdateSlots(null, 0);
            }
        }
    }

    //    private void OnEnable()
    //    {
    //#if UNITY_EDITOR
    //        database = (ItemDatabaseObject)AssetDatabase.LoadAssetAtPath("Assets/Resources/Database.asset", typeof(ItemDatabaseObject));
    //#else
    //        //database = Resources.Load<ItemDatabaseObject>("Database");
    //#endif
    //    }

    [ContextMenu("Save")]
    public void Save()
    {
        // Json을 이용한 저장
        //string saveData = JsonUtility.ToJson(this, true);
        //BinaryFormatter bf = new BinaryFormatter();
        //FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        //bf.Serialize(file, saveData);
        //file.Close();

        // IFormatter를 이용한 내부저장
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, container);
        stream.Close();
    }

    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            //BinaryFormatter bf = new BinaryFormatter();
            //FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            //JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            //file.Close();

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            Inventory newContainer = (Inventory)formatter.Deserialize(stream);
            for (int i = 0; i < container.items.Length; i++)
            {
                container.items[i].UpdateSlots(newContainer.items[i].item, newContainer.items[i].amount);
            }
            stream.Close();
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        container.Clear();
    }

    // JSON을 사용할경우 사용
    //public void OnAfterDeserialize()
    //{
    //    for (int i = 0; i < container.items.Count; i++)
    //    {
    //        if(database.GetItem.ContainsKey(i))
    //            container.items[i].itemObject = database.GetItem[container.items[i].ID];
    //    }
    //}

    //public void OnBeforeSerialize()
    //{
    //}
}

[System.Serializable]
public class Inventory
{
    public InventorySlot[] items;
    
    public Inventory(int size)
    {
        items = new InventorySlot[size];
    }

    public void Clear()
    {
        for (int i = 0; i < items.Length; i++)
        {
            items[i].RemoveItem();
        }
    }
}


[System.Serializable]
public class InventorySlot
{
    public ItemType[] allowedTypes = new ItemType[0];
    [NonSerialized] public UserInterface parent;
    public Item item;
    public int amount;

    public ItemObject ItemObject
    {
        get
        {
            if(item.id >= 0)
            {
                return parent.inventory.database.GetItem[item.id];
            }
            return null;
        }  
    }

    public InventorySlot()
    {
        UpdateSlots(null, 0);
    }

    public InventorySlot(Item _item, int _amount)
    {
        UpdateSlots(_item, _amount);
    }

    public void UpdateSlots(Item _item, int _amount)
    {
        this.item = _item;
        this.amount = _amount;
    }

    public void RemoveItem()
    {
        item = new Item();
        amount = 0;
    }
    public void AddAmount(int value)
    {
        this.amount += value;
    }

    public bool CanPlaceInSlot(ItemObject _itemObject)
    {
        if (allowedTypes.Length <= 0 || _itemObject == null || _itemObject.data.id < 0)
        {
            return true;
        }
        for (int i = 0; i < allowedTypes.Length; i++)
        {
            if (_itemObject.type == allowedTypes[i])
            {
                return true;
            }
        }
        return false;
    }
}
