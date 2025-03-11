using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="new Item Database", menuName = "Inventory System/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] items;

    //public Dictionary<ItemObject, int> GetId = new Dictionary<ItemObject, int>();
    public Dictionary<int, ItemObject> GetItem = new Dictionary<int, ItemObject>();

    public void OnAfterDeserialize()
    {
        //GetId = new Dictionary<ItemObject, int>();
        GetItem = new Dictionary<int, ItemObject>();
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null /*&& !GetId.ContainsKey(items[i])*/) // GetId.ContainsKey(items[i])가 반드시 필요
            {
                //GetId.Add(items[i], i);
                items[i].data.id = i;
                GetItem.Add(i, items[i]);
            }
        }
    }

    public void OnBeforeSerialize() 
    { 

    }
}
