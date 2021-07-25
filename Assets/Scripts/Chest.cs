using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{

    private Animator anim;
    [SerializeField] private float colliderRadius;
    [SerializeField] private bool isOpened;


    [SerializeField] private List<Item> items = new List<Item>();

    // Start is called before the first frame update
    void Start()
    {
        isOpened = false;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetPlayer();
    }



    void GetPlayer()
    {
        if (!isOpened)
            foreach (Collider col in Physics.OverlapSphere((transform.position + transform.forward * colliderRadius), colliderRadius))
            {
                if (col.gameObject.CompareTag("Player"))
                {
                    if (Input.GetKey(KeyCode.E))
                        OpenChest();
                }
            }
    }

    private void OpenChest()
    {

        anim.SetTrigger("open");
        foreach (Item i in items)
        {
            Inventory.instanceInvetory.CreateItem(i);
        }
        isOpened = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward, colliderRadius);
    }
}
