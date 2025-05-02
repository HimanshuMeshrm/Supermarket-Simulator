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
    [field:SerializeField] public EntityThoughtUI thoughtUI { get; protected set; }
    public enum EntityType
    {
        Player,
        Customer,
        Employee
    }
    public bool DestinationReached => Vector3.Distance(Destination, transform.position) <= 2f;
    protected override void CustomFixedUpdate(float deltaTime)
    {
        
    }
    protected override void CustomUpdate(float deltaTime)
    {
        
    }
    public void HoldCart(Interactable interactable)
    {
        interactable.transform.position = CartPosition.position;
        interactable.transform.rotation = CartPosition.rotation;
        interactable.transform.SetParent(CartPosition);
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
        gameObject.SetActive(false);
    }
}
