using UnityEngine;

public class ShopTask : ITask
{
    private bool _started;
    public bool IsCompleted { get; private set; }
    private int _itemsCollected;

    private bool _isItemBeingTransferred;  // Flag to track if item is currently being transferred

    public void Start(Entity entity)
    {
        _started = true;
        _itemsCollected = 0;
        _isItemBeingTransferred = false;

        if (entity is Customer customer)
            customer.GoToNextItemShelf();
    }

    public void Update(Entity entity)
    {
        

        if (!_started || IsCompleted || _isItemBeingTransferred) return;

        

        if (entity is Customer customer && !customer.IsInteracting)
        {
            if (customer.DestinationReached)
            {
                TryCollectItem(customer);
            }
        }
    }

    private void TryCollectItem(Customer customer)
    {
        if (customer._current is not Shelve shelve) return;
        if (shelve.Item != customer._currentItemRequest.Data) return;

        Transform itemTransform = shelve.Inventory.RemoveItem();

        if (itemTransform != null)
        {
            if (customer._currentCart == null)
            {
                Debug.LogWarning("[ShopTask] No cart available for customer.");
                return;
            }

            _isItemBeingTransferred = true;
            InventoryHelper.TransferItem(
                shelve.Inventory,
                customer._currentCart.Inventory,
                itemTransform,
                1f,
                () => OnItemTransferred(customer)
            );
        }
        else
        {
            Debug.LogWarning("[ShopTask] Shelf is empty.");
            ProceedToNextItem(customer);
        }
    }

    private void OnItemTransferred(Customer customer)
    {
        _itemsCollected++;
        Debug.Log($"[ShopTask] Transferred item. {_itemsCollected} / {customer._currentItemRequest.Quantity}");

        if (_itemsCollected >= customer._currentItemRequest.Quantity)
        {
            customer.SetInteractable(null);
            ProceedToNextItem(customer);
        }
        else
        {
            customer.SetDestination(customer._current.transform.position);
        }

        _isItemBeingTransferred = false; 
    }

    private void ProceedToNextItem(Customer customer)
    {
        _itemsCollected = 0;
        IsCompleted = !customer.HasMoreShoppingItems();

        if (!IsCompleted)
            customer.GoToNextItemShelf();
    }
}
