using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slots : MonoBehaviour, IDropHandler
{
    public enum SlotType
    {
        INVENTORY,
        HELMET,
        SHIELD,
        ARMOR
    }

    public SlotType slotType;

    public GameObject item
    {
        get
        {
            if(transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }
            return null;
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (!item)
        {
            DragItem.ItemBeginDragged.GetComponent<DragItem>().SetParent(transform, this);
        }
    }

}
