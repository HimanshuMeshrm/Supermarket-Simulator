using UnityEngine;
using UnityEngine.AI;

public abstract class Entity : StateController
{
    protected float WalkSpeed = 5f;
    protected float RunSpeed = 10f;

    [SerializeField] private Transform CartPosition;
    [field:SerializeField] public NavMeshAgent NavMeshAgent { get; private set; }

    public bool DestinationReached
    {
        get
        {
            if (!NavMeshAgent.pathPending && NavMeshAgent.remainingDistance <= NavMeshAgent.stoppingDistance && !NavMeshAgent.hasPath || NavMeshAgent.velocity.sqrMagnitude == 0f)
            {
                return true;
            }
            return false;
        }
    }
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
}
