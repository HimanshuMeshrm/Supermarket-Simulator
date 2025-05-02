using UnityEngine;

public class Cart : Interactable
{
    [field: SerializeField] public Inventory Inventory { get; private set; }
    [field: SerializeField] public ItemData Item { get; private set; }

    [SerializeField] private Transform spaceParent;

    private void Start()
    {
        if (Inventory == null)
        {
            Debug.LogError($"[Cart] Inventory not assigned on {gameObject.name}");
            return;
        }

        if (Item == null)
        {
            Debug.LogWarning($"[Cart] ItemData is missing on {gameObject.name}, initializing with null.");
        }

        if (spaceParent == null)
        {
            Debug.LogError($"[Cart] spaceParent not assigned on {gameObject.name}");
            return;
        }

        Inventory.Initialize(Item, spaceParent);
    }

    public override void Interact(IInteractor interactor)
    {
        if (!IsInteractable)
        {
            Debug.LogWarning($"[Cart] Tried to interact with cart {name}, but it's already being held.");
            return;
        }

        if (interactor is Entity entity)
        {
            entity.HoldCart(this);
            IsInteractable = false;
            Debug.Log($"[Cart] {entity.name} is now holding cart {name}");
        }
        else
        {
            Debug.LogWarning($"[Cart] Interactor is not an Entity.");
        }
    }
        
}
