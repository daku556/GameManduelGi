using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Helmet,
    Primary_Weapon,
    Secondary_Weapon,
    Chest,
    Boots,
    PickUp,
    Default
}

public enum Attributes
{
    Strength,
    Agility
}

public abstract class ItemObject : ScriptableObject
{
    public Sprite UIDisplay;
    public ItemType type;
    public GameObject prefab;
    [TextArea(15,20)]
    public string description; // 아이템 설명

    public bool isConsumable; // 즉시 사용 여부 확인
    public bool isStack;
    public Item data = new Item();

    //public ItemBuff[] buffs;
    //public ItemInstantBuff[] instants;

    public virtual bool UseItem(PlayerManager player)
    {
        return false;
    }

    public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }
}

[System.Serializable]
public class Item
{
    public string name;
    public int id = -1;
    public GameObject prefab;

    public bool isConsumable;
    public bool isStack;
    public ItemBuff[] buffs;
    public ItemInstantBuff[] instantBuff;
    public Item()
    {
        name = "";
        id = -1;
    }
    public Item(ItemObject item)
    {
        name = item.name;
        id = item.data.id;
        isConsumable = item.isConsumable;
        isStack = item.isStack;
        prefab = item.prefab;

        buffs = new ItemBuff[item.data.buffs.Length];
        for (int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new ItemBuff(item.data.buffs[i].min, item.data.buffs[i].max);
            buffs[i].attribute = item.data.buffs[i].attribute;
        }

        instantBuff = new ItemInstantBuff[item.data.instantBuff.Length];
        for (int i = 0; i < instantBuff.Length; i++)
        {
            instantBuff[i] = new ItemInstantBuff(item.data.instantBuff[i].value);
            instantBuff[i].stats = item.data.instantBuff[i].stats;
        }
    }

    public void UseItem()
    {
        PlayerManager p = PlayerManager.Instance; 
        for (int i = 0; i < instantBuff.Length; i++)
        {
            p.setStats(instantBuff[i].stats, instantBuff[i].value);
        }
    }
}

[System.Serializable]
public class ItemBuff
{ 
    public Attributes attribute;
    public int value;
    public int max;
    public int min;
    public ItemBuff(int _min, int _max)
    {
        min = _min;
        max = _max;
        GenerateValue();
    }

    public void GenerateValue()
    {
        value = UnityEngine.Random.Range(min, max);
    }
}

[System.Serializable]
public class ItemInstantBuff
{ 
    public CharacterStats stats;
    public int value;

    public ItemInstantBuff(int _value)
    {
        value = _value;
    }
}
