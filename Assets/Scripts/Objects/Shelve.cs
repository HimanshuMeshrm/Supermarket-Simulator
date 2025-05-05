
using System.Collections.Generic;
using UnityEngine;
using static Entity;

public class Shelve : Interactable
{
    [field: SerializeField] public Inventory Inventory { get; private set; }
    [field: SerializeField] public ItemData Item { get; private set; }
    [SerializeField] private Transform spaceParent;

    public void Start()
    {
        Inventory.Initialize(Item, spaceParent);
        AddItems();
    }

    public void AddItems()
    {
        int count = 10;
        for (int i = 0; i < count; i++)
        {
            Item item = PoolManager.Instance.GetItemPool(Item).Get().GetComponent<Item>();
            Inventory.AddItem(item);
        }
    }

    public override void Interact(IInteractor interactor)
    {
        if (interactor is Entity entity)
        {
            switch (entity.Type)
            {
                case EntityType.Customer:
                    Customer customer = entity as Customer;
                    break;
                case EntityType.Employee:
                    Employee employee = entity as Employee;
                    break;
                case EntityType.Player:
                    Player player = entity as Player;
                    int remaining = Inventory.GetEmptySpaceCount();

                    for (int i = 0; i < remaining; i++)
                    {
                        Item capturedItem = PoolManager.Instance.GetItemPool(Item).Get().GetComponent<Item>();
                        ItemSpace targetSpace = Inventory.ReserveFirstEmptySpace();

                        if (targetSpace == null)
                        {
                            Debug.LogWarning("No empty item space found.");
                            break;
                        }




                        ItemSpace capturedSpace = targetSpace;

                        SmoothMover.Move(
                         item: capturedItem.transform,
                         target: capturedSpace.Transform,
                         duration: 0.5f,
                         ease: SmoothMover.EaseType.Smooth,
                         onComplete: () =>
                         {
                             Inventory.AddItemToSpace(capturedSpace, capturedItem);
                             ItemsFull();
                         });

                    }

                    /*AnimatorUtil.MoveItems(capturedItem.transform, capturedSpace.Transform, 1f, () =>
                    {
                        Inventory.AddItemToSpace(capturedSpace, capturedItem);
                        ItemsFull();
                    });*/

                    break;

                default:
                    Debug.LogWarning("Unknown entity type.");
                    break;
            }
        }
    }
    public void ItemsFull()
    {
        Debug.Log("Boo");
    }
}
   
