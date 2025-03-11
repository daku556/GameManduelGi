using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicInterface : UserInterface
{
    //protected List<Transform> inventoryCells = new List<Transform>();
    public GameObject inventoryPrefab;
    //private void setInventoryCellReference()
    //{
    //    Transform grid = transform; // 이 스크립트가 부착된 객체는 Vertical Layout Group
    //    foreach (Transform cell in grid)
    //    {
    //        inventoryCells.Add(cell);
    //    }
    //}

    public override void CreateInventorySlots()
    {
        slotsOnInterface = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < inventory.container.items.Length; i++)
        {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragBegin(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

            slotsOnInterface.Add(obj, inventory.container.items[i]);
        }
    }
    //private Vector3 getPosition(int i)
    //{
    //    return inventoryCells[i].position;
    //}
}
