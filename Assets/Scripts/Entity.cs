using UnityEngine;
using UnityEngine.AI;

public abstract class Entity : StateController
{
    protected float WalkSpeed = 3f;
    protected float RunSpeed = 8f;

    [SerializeField] private Transform CartPosition;
    [field:SerializeField] public NavMeshAgent NavMeshAgent { get; private set; }
    private Vector3 Destination = Vector3.zero;
    [field: SerializeField] public EntityType Type { get; protected set; } = EntityType.Customer;
    public EntityThoughtUI thoughtUI { get; protected set; }

    
    public enum EntityType
    {
        Player,
        Customer,
        Employee
    }
    public bool DestinationReached => Vector3.Distance(Destination, transform.position) <= 2f;

    [field: SerializeField] public MoneyAccount MoneyAccount { get; protected set; } = new MoneyAccount();
    protected override void CustomFixedUpdate(float deltaTime)
    {
        
    }
    protected override void CustomUpdate(float deltaTime)
    {
        
    }

    public int PrefabID { get; set; } = -1;
    public void HoldCart(Interactable interactable)
    {
        if (interactable is Cart cart)
        {
            interactable.transform.position = CartPosition.position;
            interactable.transform.rotation = CartPosition.rotation;
            interactable.transform.SetParent(CartPosition);
           
        }
        else
        {
            Debug.LogError($"HoldCart: {interactable.name} is not t Cart.");
        }
    }
    public void LeaveCart(Customer customer)
    {
        if (customer == null)
        {
            Debug.LogWarning("LeaveCart called with t null customer.");
            return;
        }

        if (customer._currentCart == null)
        {
            Debug.LogWarning($"Customer {customer.name} has no cart to leave.");
            return;
        }

       

        Debug.Log($"Customer {customer.name} is leaving cart: {customer._currentCart.name}");
        customer._currentCart.transform.SetParent(null);
        PoolManager.Instance.CartPool.ReturnToPool(customer._currentCart.gameObject);
        customer._currentCart = null;
    }

    public abstract void SetEntityType(EntityType type);
    public void SetDestination(Vector3 position)
    {
        if(NavMeshAgent.SetDestination(position))
        {
            Destination = position;
        }
    }
   
    public void ReturnToPool()
    {
        PoolManager.Instance.GetCustomerPool(PrefabID).ReturnToPool(gameObject);
    }
}
