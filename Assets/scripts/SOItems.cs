using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    None,
    Ketchup,
    Mustard,
    Mayonnaise,
    Cheese,
    Flamethrower
}

[CreateAssetMenu(fileName = "New Item", menuName = "Items")]
public class SOItems : ScriptableObject
{
    public ItemType ItemType;
    public int Damage = 10;
    public float Speed = 1f;
    public float JumpRadius = 0.5f;
    public int Protection = 0;
    public Sprite ItemImage;
    public int Heal = 1;
}
