using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new Default Item", menuName = "Item/Default")]

public class DefaultObject : ItemObject
{
    // Start is called before the first frame update
    void Awake()
    {
        type = ItemType.Default;
    }
}
