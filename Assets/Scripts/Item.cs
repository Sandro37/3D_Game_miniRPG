using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item: ScriptableObject
{
    [SerializeField] private Sprite icon;

    public Sprite Icon
    {
        get => icon;
        set => icon = value;
    }
    [SerializeField] private string nome;

    public string Nome
    {
        get => nome;
        set => nome = value;
    }

    [SerializeField] private float value;

    public float Value
    {
        get => this.value;
        set => this.value = value;
    }

    [System.Serializable]
    public enum Type
    {
        POTION,
        ELIXIR,
        CRYSTAL
    }

    public Type itemType;
    public enum SlotType
    {
        HELMET,
        SHIELD,
        ARMOR
    }

    public SlotType slotType;


    public Player player;
    public void GetAction()
    {

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        switch (itemType)
        {
            case Type.POTION:
                player.Increasestats(value, 0f);
                break;
            case Type.ELIXIR:
                Debug.Log("ELIXIR");
                break;
            case Type.CRYSTAL:
                player.Increasestats(0f, value);
                break;
        }
    }

    public void RemoveStats()
    {

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        switch (itemType)
        {
            case Type.POTION:
                player.DecreaseStats(value, 0f);
                break;
            case Type.ELIXIR:
                Debug.Log("ELIXIR");
                break;
            case Type.CRYSTAL:
                player.DecreaseStats(0f, value);
                break;
        }
    }
}
