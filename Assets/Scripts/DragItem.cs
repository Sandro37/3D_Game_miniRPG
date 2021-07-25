using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{


    public static GameObject ItemBeginDragged;
    private Vector3 startPos;
    private Transform startParent;
    [SerializeField] private Item item;

    public Item Item
    {
        get => item;
        set => item = value;
    }

    private void Start()
    {
        GetComponent<Image>().sprite = item.Icon;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        ItemBeginDragged = gameObject;
        startPos = transform.position;
        startParent = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ItemBeginDragged = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        if (transform.parent  == startParent)
        {
            transform.position = startPos;
        }
    }

  
    public void SetParent(Transform slotTransform, Slots slot)
    {
        
        if (item.slotType.ToString() == slot.slotType.ToString())
        {
            transform.SetParent(slotTransform);
            item.GetAction();
        }
        else if (slot.slotType.ToString() == "INVENTORY")
        {
            transform.SetParent(slotTransform);
            item.RemoveStats();
        }
    }
}
