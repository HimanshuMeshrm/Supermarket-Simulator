using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory
{
    [SerializeField] private List<ItemSpace> spaces = new List<ItemSpace>();
    [SerializeField] private ItemData defaultItem;
    [SerializeField] private Transform spaceParent;

    public IReadOnlyList<ItemSpace> Spaces => spaces;

    const string ANY_ITEM = "AnyItem";

    public void Initialize(ItemData defaultItem, Transform spaceParent)
    {
        this.defaultItem = defaultItem;
        this.spaceParent = spaceParent;

        spaces.Clear();

        int index = 0;
        foreach (Transform child in spaceParent)
        {
            index++;
            child.name = $"Space {index.ToString("00")}";
            var newSpace = new ItemSpace(null, child);
            spaces.Add(newSpace);
        }

        Debug.Log($"Inventory initialized with {spaces.Count} spaces.");
    }

    public bool AddItem(Item item)
    {
        if (item.ItemData == defaultItem || item.ItemData.Name == ANY_ITEM)
        {
            foreach (var space in spaces)
            {
                if (space.IsEmpty)
                {
                    space.SetItem(item);
                    return true;
                }
            }
            Debug.LogWarning("No empty space found to add item.");
        }
        return false;
    }

    public bool AddItemToSpace(ItemSpace targetSpace, Item item)
    {
        if (targetSpace == null || item == null)
        {
            Debug.LogError("Invalid space or item.");
            return false;
        }

        if (!spaces.Contains(targetSpace))
        {
            Debug.LogError("The provided space does not belong to this inventory.");
            return false;
        }

        if (item.ItemData == defaultItem || defaultItem.Name == ANY_ITEM)
        {
           
            targetSpace.SetItem(item);
        }
        return true;
    }

    public ItemSpace ReserveFirstEmptySpace()
    {
        foreach (var space in spaces)
        {
            if (space.IsEmpty)
            {
                space.Reserve();
                return space;
            }
        }

        return null;
    }

    public Transform RemoveItem(bool returnToPool = false)
    {
        foreach (var space in spaces)
        {
            if (!space.IsEmpty)
            {
                var removedItem = space.ItemTransform;
                space.Clear(returnToPool);
                return removedItem;
            }
        }
        return null;
    }

    public void Clear(bool returnToPool = false)
    {
        foreach (var space in spaces)
        {
            space.Clear(returnToPool);
        }

        Debug.Log("Inventory cleared.");
    }

    public int GetItemCount(ItemData itemData)
    {
        if (itemData == null) return 0;

        int count = 0;
        foreach (var space in spaces)
        {
            if (!space.IsEmpty && space.Item?.ItemData == itemData)
                count++;
        }

        return count;
    }

    public int GetTotalValue()
    {
        int total = 0;
        foreach (var space in spaces)
        {
            if (!space.IsEmpty && space.Item != null)
            {
                total += Mathf.RoundToInt(space.Item.ItemData.Price);
            }
        }

        return total;
    }

    public int GetEmptySpaceCount()
    {
        int count = 0;
        foreach (var space in spaces)
        {
            if (space.IsEmpty)
                count++;
        }

        return count;
    }

    public ItemSpace GetFirstEmptySpace()
    {
        foreach (var space in spaces)
        {
            if (space.IsEmpty)
                return space;
        }

        return null;
    }
}
