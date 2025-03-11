using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;


public abstract class UserInterface : MonoBehaviour
{
    public InventoryObject inventory;
    public Dictionary<GameObject, InventorySlot> slotsOnInterface;
    // Start is called before the first frame update
    void Awake()
    {
        setSlotParent();
        CreateInventorySlots();

        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(this); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(this); });
    }

    public void LateUpdate()
    {
        updateInventorySlots();
    }

    protected void setSlotParent()
    {
        for (int i = 0; i < inventory.container.items.Length; i++)
        {
            inventory.container.items[i].parent = this;
        }
    }

    public void updateInventorySlots()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in slotsOnInterface)
        {
            if (_slot.Value.item.id >= 0)    
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = _slot.Value.ItemObject.UIDisplay;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.amount == 1 ? "" : _slot.Value.amount.ToString("n0");
            }
            else
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }

    public abstract void CreateInventorySlots();
    protected void OnEnterInterface(UserInterface obj)
    {
        MouseData.interfaceMouseIsOver = obj;
    }
    protected void OnExitInterface(UserInterface obj)
    {
        MouseData.interfaceMouseIsOver = null;
    }
    protected void OnEnter(GameObject obj)
    {
        MouseData.slotHoveredOver = obj;
    }
    protected void OnExit(GameObject obj)
    {
        MouseData.slotHoveredOver = null;
    }

    protected void OnDragBegin(GameObject obj)
    {
        if (slotsOnInterface[obj].item.id >= 0)
        {
            var mouseObject = new GameObject();
            var rectTransform = mouseObject.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(50, 50);
            mouseObject.transform.SetParent(transform.parent);

        
            var image = mouseObject.AddComponent<Image>();
            image.sprite = slotsOnInterface[obj].ItemObject.UIDisplay;
            image.raycastTarget = false;
        
            MouseData.tempItemBeingDragged = mouseObject;
            //GameManager.Instance.mouseItem.item = slotsOnInterface[obj];
        }
    }
    protected void OnDragEnd(GameObject obj)
    {
        //var itemOnMouse = GameManager.Instance.mouseItem;
        //var mouseHoverItem = itemOnMouse.hoveritem; // 마우스로 잡고있는 아이템이 아닌 마우스가 현재 위에있는 아이템
        //var mouseHoverObj = itemOnMouse.hoverObj;
        //var GetItemObject = inventory.database.GetItem;

        //if(itemOnMouse.ui != null)
        //{
        //    if (mouseHoverObj && itemsDisplayed[obj].ID >= 0)
        //    {
        //        if (mouseHoverItem.CanPlaceInSlot(GetItemObject[itemsDisplayed[obj].ID]) && (mouseHoverItem.item.id <= -1 || mouseHoverItem.item.id >= 0 && itemsDisplayed[obj].CanPlaceInSlot(GetItemObject[mouseHoverItem.item.id]))) // 옮기려는 아이템이 해당 슬롯에 적합 && 목적지의 아이템이 옮기려는 아이템의 슬롯에 적합
        //        {
        //            inventory.MoveItem(itemsDisplayed[obj], mouseHoverItem.parent.itemsDisplayed[mouseHoverObj]);
        //        }
        //    }
        //}
        //else
        //{
        //    inventory.RemoveItem(itemsDisplayed[obj].item);
        //}

        //Destroy(itemOnMouse.obj);
        //itemOnMouse.item = null;

        Destroy(MouseData.tempItemBeingDragged);
        if(MouseData.interfaceMouseIsOver == null)
        {
            Transform playerPosition;
            playerPosition = PlayerManager.Instance.GetComponentInChildren<PlayerController>().transform;
            Debug.Log(playerPosition);
            Instantiate(slotsOnInterface[obj].item.prefab, playerPosition.position + new Vector3(3,0,0), Quaternion.identity);


            slotsOnInterface[obj].RemoveItem();
            
            
            return;
        }
        if(MouseData.slotHoveredOver != null)
        {
            InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver];
            inventory.SwapItem(slotsOnInterface[obj],mouseHoverSlotData);
        }
    }
    protected void OnDrag(GameObject obj)
    {
        if (MouseData.tempItemBeingDragged != null)
        {
            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }

    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }
}

public static class MouseData
{
    public static UserInterface interfaceMouseIsOver; // 현재 마우스가 위에있는 인터페이스
    public static GameObject tempItemBeingDragged; // 마우스가 들고있는(드래그중인) 오브젝트
    public static GameObject slotHoveredOver; // 현재 마우스가 위에있는 슬롯(오브젝트)
}