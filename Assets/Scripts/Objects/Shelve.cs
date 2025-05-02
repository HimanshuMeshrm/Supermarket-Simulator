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
            Item item = PoolManager.Instance.GetPool(Item).Get().GetComponent<Item>();
            Inventory.AddItem(item.transform);
        }
    }

    public void Interact(GameObject interactor)
    {
        if (interactor.TryGetComponent<Entity>(out var entity))
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
                    break;
                default:
                    Debug.LogWarning("Unknown entity type.");
                    break;
            }
        }
    }

    public override void Interact(IInteractor interactor)
    {
        if (interactor is Customer customer)
        {
            Transform item = Inventory.RemoveItem();
            if (item != null)
            {
                if(customer._current is Cart cart)
                {
                    cart.Inventory.AddItem(item);
                }
            }
        }
    }
}
