using Assets.Scripts.Items;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public string displayName;
    public string description;
    public int cost;
    public ItemQuality itemQuality;
    public ItemType itemType;

}
