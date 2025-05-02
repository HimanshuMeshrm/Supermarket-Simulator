using System;
using System.Collections.Generic;

[Serializable]
public class ShoppingList
{
    private Queue<ItemRequest> _itemsToBuy = new Queue<ItemRequest>();

    public void AddItem(ItemData data, int quantity)
    {
        _itemsToBuy.Enqueue(new ItemRequest(data, quantity));
    }

    public bool HasItems => _itemsToBuy.Count > 0;

    public ItemRequest PeekNextItem() => _itemsToBuy.Peek();

    public ItemRequest DequeueItem() => _itemsToBuy.Dequeue();
}

[Serializable]
public class ItemRequest
{
    public ItemData Data { get; private set; }
    public int Quantity { get; private set; }

    public ItemRequest(ItemData data, int quantity)
    {
        Data = data;
        Quantity = quantity;
    }
}
