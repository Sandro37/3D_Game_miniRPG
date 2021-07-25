using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject slotParent;
    [SerializeField] private List<Slots> slots = new List<Slots>();


    public static Inventory instanceInvetory;
    // Start is called before the first frame update
    void Start()
    {
        instanceInvetory = this;
        GetSlots();
    }

    public void GetSlots()
    {
        foreach(Slots slot in slotParent.GetComponentsInChildren<Slots>())
        {
            slots.Add(slot);
        }
       // CreateItem();
    }

    public void CreateItem(Item item)
    {

        foreach (Slots slot in slots)
        {
            if(slot.transform.childCount == 0)
            {
                GameObject currentItem = Instantiate(GameController.instanceGameControler.ItemPrefab, slot.transform);
                currentItem.GetComponent<DragItem>().Item = item;

                return;
            }
        }

        
    }
}
