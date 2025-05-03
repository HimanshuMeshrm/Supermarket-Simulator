using System;
using UnityEngine;

[Serializable]
public class ItemSpace
{
    [field: SerializeField] public Item Item { get; private set; }
    [field: SerializeField] public Transform Transform { get; private set; }

    private bool _isReserved = false;
    public bool IsEmpty => Transform?.childCount == 0 && !_isReserved;
    public Transform ItemTransform
    {
        get
        {
            if (Item is not null)
            {
                return Item.transform;
            }
            return null;
        }
    }

    public ItemSpace(Item i, Transform t)
    {
        this.Item = i;
        this.Transform = t;
    }

    public void Reserve()
    {
        if (IsEmpty)
        {
            _isReserved = true;
        }
        else
        {
            Debug.LogWarning("Cannot reserve non-empty space.");
        }
    }

    public void Unreserve()
    {
        _isReserved = false;
    }

    public void SetItem(Item Item)
    {
        if ((Transform.childCount == 0 && !_isReserved) || _isReserved)
        {
            Transform itemTransform = Item.transform;
            this.Item = Item;
            _isReserved = false;
            itemTransform.SetParent(Transform);
            itemTransform.localPosition = Vector3.zero;


            Unreserve();
            if (!itemTransform.gameObject.activeInHierarchy)
            {
                itemTransform.gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.LogWarning("Item space is not empty or reserved.");
        }
    }

    public void Clear(bool returnToPool = false)
    {
        if (!IsEmpty)
        {
            if (returnToPool)
            {
                PoolManager.Instance.GetItemPool(Item.ItemData).ReturnToPool(ItemTransform.gameObject);
            }
            Item = null;
            Unreserve();
        }
    }
}
