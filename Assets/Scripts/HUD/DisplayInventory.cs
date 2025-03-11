//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;
//using UnityEngine.InputSystem;
//using UnityEngine.UI;
//using UnityEngine.EventSystems;
//using UnityEngine.Events;

//public class DisplayInventory : MonoBehaviour
//{
//    public MouseItem mouseItem = new MouseItem();
//    public GameObject inventoryPrefab;
//    public InventoryObject inventory;
//    private List<Transform> inventoryCells = new List<Transform>();
//    private Dictionary<GameObject, InventorySlot> itemsDisplayed = new Dictionary<GameObject, InventorySlot>();
//    public Vector3 textOffset = new Vector3(0, -30f, 0);
//    // Start is called before the first frame update
//    void Awake()
//    {
//        setInventoryCellReference();
//        createInventorySlots();
//        //CreateDisplay();
//    }

//    public void LateUpdate()
//    {
//        updateInventorySlots();
//    }

//    private void setInventoryCellReference()
//    {
//        Transform grid = transform; // 이 스크립트가 부착된 객체는 Vertical Layout Group
//        foreach (Transform cell in grid)
//        {
//            inventoryCells.Add(cell);
//        }
//    }

//    public void updateInventorySlots()
//    {
//        foreach (KeyValuePair<GameObject, InventorySlot> _slot in itemsDisplayed)
//        {
//            if(_slot.Value.ID >= 0)
//            {
//                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[_slot.Value.ID].UIDisplay;
//                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
//                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.amount == 1 ? "" : _slot.Value.amount.ToString("n0");
//            }
//            else
//            {
//                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
//                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
//                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
//            }
//        }
//    }

//    private void createInventorySlots()
//    {
//        itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

//        for (int i = 0;i < inventory.container.items.Length; i++)
//        {
//            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);

//            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
//            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
//            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragBegin(obj); });
//            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
//            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

//            itemsDisplayed.Add(obj, inventory.container.items[i]);
//        }
//    }
//    public void OnEnter(GameObject obj)
//    {
//        mouseItem.hoverObj = obj;
//        if (itemsDisplayed.ContainsKey(obj))
//        {
//            mouseItem.hoveritem = itemsDisplayed[obj];
//        }
//    }
//    private void OnExit(GameObject obj)
//    {
//        mouseItem.hoverObj = null;
//        mouseItem.hoveritem = null;
//    }
    
//    private void OnDragBegin(GameObject obj)
//    {
//        var mouseObject = new GameObject();
//        var rectTransform = mouseObject.AddComponent<RectTransform>();
//        rectTransform.sizeDelta = new Vector2(50, 50);
//        mouseObject.transform.SetParent(transform.parent);
//        if (itemsDisplayed[obj].ID >= 0)
//        {
//            var image = mouseObject.AddComponent<Image>();
//            image.sprite = inventory.database.GetItem[itemsDisplayed[obj].ID].UIDisplay;
//            image.raycastTarget = false;
//        }

//        mouseItem.obj = mouseObject;
//        mouseItem.item = itemsDisplayed[obj];
//    }
//    private void OnDragEnd(GameObject obj)
//    {
//        if (mouseItem.hoverObj)
//        {
//            inventory.MoveItem(itemsDisplayed[obj], itemsDisplayed[mouseItem.hoverObj]);
//        }
//        else
//        {
//            inventory.RemoveItem(itemsDisplayed[obj].item);
//        }
//        Destroy(mouseItem.obj);
//        mouseItem.item = null;
//    }
//    private void OnDrag(GameObject obj)
//    {
//        if(mouseItem.obj != null)
//        {
//            mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;
//        }
//    }


//    private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
//    {
//        EventTrigger trigger = obj.GetComponent<EventTrigger>();
//        var eventTrigger = new EventTrigger.Entry();
//        eventTrigger.eventID = type;
//        eventTrigger.callback.AddListener(action);
//        trigger.triggers.Add(eventTrigger);
//    }

//    private Vector3 getPosition(int i)
//    {
//        return inventoryCells[i].position;
//    }


    
//}

//public class MouseItem
//{
//    public GameObject obj;
//    public InventorySlot item;
//    public InventorySlot hoveritem;
//    public GameObject hoverObj;
//}



//private void UpdateDisplay()
//{
//    for (int i = 0; i < inventory.container.items.Count; i++)
//    {
//        InventorySlot slot = inventory.container.items[i];
//        if (itemsDisplayed.ContainsKey(slot))
//        {
//            itemsDisplayed[slot].GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString();
//        }
//        else
//        {
//            Transform objCell = findEmptyCell();
//            var obj = Instantiate(inventoryPrefab);
//            obj.GetComponent<Image>().sprite = inventory.database.GetItem[slot.item.id].UIDisplay;
//            RectTransform objRect = obj.GetComponent<RectTransform>();
//            obj.transform.SetParent(objCell, false);
//            objRect.localPosition = Vector3.zero;
//            objRect.localRotation = Quaternion.identity;
//            itemsDisplayed.Add(slot, obj);

//            TextMeshProUGUI textMeshPro = obj.GetComponentInChildren<TextMeshProUGUI>();
//            textMeshPro.text = slot.amount.ToString();
//        }
//    }
//}



//public void CreateDisplay()
//{
//    for (int i = 0; i < inventory.container.items.Count; i++)
//    {
//        InventorySlot slot = inventory.container.items[i];
//        Transform objCell = findEmptyCell();
//        var obj = Instantiate(inventoryPrefab);
//        obj.GetComponent<Image>().sprite = inventory.database.GetItem[slot.item.id].UIDisplay;
//        RectTransform objRect = obj.GetComponent<RectTransform>();
//        obj.transform.SetParent(objCell, false);
//        objRect.localPosition = Vector3.zero;
//        objRect.localRotation = Quaternion.identity;
//        itemsDisplayed.Add(slot, obj);

//        TextMeshProUGUI textMeshPro = obj.GetComponentInChildren<TextMeshProUGUI>();
//        textMeshPro.text = slot.amount.ToString();
//    }
//}

//private Transform findEmptyCell()
//{
//    for (int i = 0; i < inventoryCells.Count; i++)
//    {
//        for (int j = 0; j < inventoryCells[i].Count; j++)
//        {
//            if (inventoryCells[i][j].childCount == 0)
//            {
//                return inventoryCells[i][j];
//            }
//        }
//    }
//    return null;
//}