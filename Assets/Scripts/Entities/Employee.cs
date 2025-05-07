using UnityEngine;

public class Employee : Entity, IInteractor
{
    private const string IdleAnim = "Idle";
    private const string WalkAnim = "Walk";
    private const string RunAnim = "Run";

    private string _currentAnim;
    private Interactable _current;

    public bool IsInteracting { get; set; }


    public enum EmployeeMode
    {
        Idle,
        GoToItem,
        GoToShelve,
        GoToCashCounter,
        GoToRest
    }

    private void Start()
    {
        SetEntityType(EntityType.Employee);
    }
    public override void SetEntityType(EntityType type)
    {
        this.Type = type;
    }
    public override void Update()
    {
        UpdateAnimationState();
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
