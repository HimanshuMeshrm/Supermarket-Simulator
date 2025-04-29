using UnityEngine;

public class Customer : Entity, IInteractor
{
    private const string IdleAnim = "Idle";
    private const string WalkAnim = "Walk";
    private const string RunAnim = "Run";

    private string _currentAnim;
    private Interactable _current;

    public bool IsInteracting { get; set; }

    private void Start()
    {
        if (GameManager.Instance?.Player != null)
          
            NavMeshAgent.SetDestination(GameManager.Instance.Player.transform.position);
    }

    public override void Update()
    {
        UpdateAnimationState();

        if(DestinationReached)
        {
            NavMeshAgent.SetDestination(GameManager.Instance.Player.transform.position);
        }
        base.Update();
    }

    private void UpdateAnimationState()
    {
        NavMeshAgent.speed = NavMeshAgent.isPathStale ? RunSpeed : WalkSpeed;

        string newAnim = NavMeshAgent.isStopped
            ? IdleAnim
            : NavMeshAgent.remainingDistance > 10f ? RunAnim : WalkAnim;

        if (newAnim == _currentAnim)
            return;

        _currentAnim = newAnim;

        switch (_currentAnim)
        {
            case IdleAnim:
                SwitchState(new IdleState(this, Animator));
                break;
            case WalkAnim:
                SwitchState(new WalkState(this, Animator));
                break;
            case RunAnim:
                SwitchState(new RunState(this, Animator));
                break;
        }

        
    }

    public void SetInteractable(Interactable interactable)
    {
        _current = interactable;
    }

    public void Interact()
    {
        if (_current == null || IsInteracting)
            return;

        IsInteracting = true;
        _current.TryInteract();
        IsInteracting = false;
    }
}
