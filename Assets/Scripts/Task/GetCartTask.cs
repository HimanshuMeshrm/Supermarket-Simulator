using UnityEngine;

public class GetCartTask : ITask
{
    private bool _started;
    public bool IsCompleted { get; private set; }

    public void Start(Entity entity)
    {
        _started = true;
        Debug.Log($"Starting GetCartTask for entity: {entity.name}");
        entity.SetDestination(GameManager.Shop.CartSpawnPosition.position);
        entity.thoughtUI.SetIcon(DataHolder.Instance.GetTaskSprite(this.GetType().Name));
    }

    public void Update(Entity entity)
    {
        if (!_started || IsCompleted) return;

        if (entity.DestinationReached)
        {
            Debug.Log($"Entity {entity.name} has reached the cart spawn position.");

            if (entity is Customer customer)
            {
                Debug.Log($"Entity {customer.name} is interacting with cart.");
                Cart cart = PoolManager.Instance.GetCart();
                cart.Initialize();
                customer.SetInteractable(cart);
                customer.Interact();
              
                IsCompleted = true;
            }
        }
    }
}
