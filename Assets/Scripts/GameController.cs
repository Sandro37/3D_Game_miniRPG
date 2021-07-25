using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject inventoryButton;
    [SerializeField] private GameObject itemPrefab;

    private bool isInventoryEnable = false;

    public static GameController instanceGameControler;

    private void Awake()
    {
        instanceGameControler = this;
    }

    public GameObject ItemPrefab
    {
        get => itemPrefab;
    }
    public bool IsInventoryEnable
    {
        get => isInventoryEnable;
    }
    public void ActiveGameObject(GameObject Gameobj)
    {
        Gameobj.SetActive(true);
        inventoryButton.SetActive(false);
        isInventoryEnable = true;
    }

    public void DisableGameObject(GameObject Gameobj)
    {
        Gameobj.SetActive(false);
        inventoryButton.SetActive(true);
        isInventoryEnable = false;
    }
}
