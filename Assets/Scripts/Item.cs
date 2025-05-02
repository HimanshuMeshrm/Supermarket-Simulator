using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemData _itemData;
    public ItemData ItemData => _itemData;
    public void SetData(ItemData itemData)
    {
        _itemData = itemData;
    }
}