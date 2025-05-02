using System;
using UnityEngine;

[Serializable]
public class ItemSpace
{
    [field: SerializeField] public ItemData Item { get; private set; }
    [field: SerializeField] public Transform Transform { get; private set; }
    public bool IsEmpty => Transform?.childCount == 0;
    public Transform ItemTransform => Transform?.childCount > 0 ? Transform.GetChild(0) : null;

    public ItemSpace(ItemData i, Transform t)
    {
        this.Item = i;
        this.Transform = t;
    }

    public void SetItem(Transform item)
    {
        if (IsEmpty)
        {
            item.SetParent(Transform);
            item.localPosition = Vector3.zero;
            if (!item.gameObject.activeInHierarchy)
            {
                item.gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.LogWarning("Item space is not empty.");
        }
    }

    public void Clear(bool returnToPool = false)
    {
        if (!IsEmpty && returnToPool)
        {
            PoolManager.Instance.GetPool(Item).ReturnToPool(ItemTransform.gameObject);
        }
    }
}