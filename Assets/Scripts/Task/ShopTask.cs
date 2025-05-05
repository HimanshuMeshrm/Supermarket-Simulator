using System.Collections.Generic;
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
        if (customer._currentCart == null) return;

        Item shelveItem = shelve.Inventory.RemoveItem()?.GetComponent<Item>();
        if (shelveItem == null)
        {
            customer.thoughtUI.SetIcon(DataHolder.Instance.GetTaskSprite("Waiting"));
            return;
        }

        ItemSpace reservedSpace = customer._currentCart.Inventory.ReserveFirstEmptySpace();
        if (reservedSpace == null)
        {
            Debug.LogWarning("[ShopTask] No empty space in cart.");
            reservedSpace.Unreserve();
            shelve.Inventory.AddItem(shelveItem);
            return;
        }

        _isItemBeingTransferred = true;

        /*AnimatorUtil.MoveItems(shelveItem.transform, reservedSpace.Transform, 1f, () => {
            customer._currentCart.Inventory.AddItemToSpace(reservedSpace, shelveItem);
            OnItemTransferred(customer);
        });
        */



        SmoothMover.Move(
        item: shelveItem.transform,
        target: reservedSpace.Transform,
        duration: 0.5f,
        ease: SmoothMover.EaseType.Smooth,
        onComplete: () =>
        {
            customer._currentCart.Inventory.AddItemToSpace(reservedSpace, shelveItem);
            OnItemTransferred(customer);
        });

    }

    private void OnItemTransferred(Customer customer)
    {
        _itemsCollected++;
        Debug.Log($"[ShopTask] Transferred item. {_itemsCollected} / {customer._currentItemRequest.Quantity}");
        customer.thoughtUI.SetIcon(customer._currentItemRequest.Data.Icon, $"{_itemsCollected} / {customer._currentItemRequest.Quantity}");
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
