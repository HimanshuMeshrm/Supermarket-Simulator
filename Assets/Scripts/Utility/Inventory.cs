using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class Inventory
{
    [SerializeField] private List<ItemSpace> Spaces = new List<ItemSpace>();
    [SerializeField] ItemData Item;
    [SerializeField] private Transform spaceParent;

    public void Initialize(ItemData itemID, Transform spaceParent)
    {
        this.Item = itemID;
        this.spaceParent = spaceParent;

        AddSpaces();
    }

    private void AddSpaces()
    {
        foreach (Transform child in spaceParent)
        {
            Spaces.Add(new ItemSpace(this.Item, child));
        }
    }

    public void AddItem(Transform item)
    {
        foreach (var space in Spaces)
        {
            if (space.IsEmpty)
            {
                space.SetItem(item);
                return;
            }
        }
    }

    public Transform RemoveItem()
    {
        foreach (var space in Spaces)
        {
            if (!space.IsEmpty)
            {
                Transform item = space.ItemTransform;
                space.Clear();
                return item;
            }
        }
        return null;
    }
    public int GetItemCount(ItemData data)
    {
        int count = 0;
        foreach (var space in Spaces)
        {
            if (space.Item == data)
            {
                count++;
            }
        }
        return count;
    }
}