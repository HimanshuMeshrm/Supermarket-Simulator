using UnityEngine;

public abstract class EntityState
{
    public abstract string AnimName { get; }
    protected Animator Animator { get; private set; }
    protected Entity Entity { get; private set; }
    protected EntityState(Entity entity, Animator animator)
    {
        Animator = animator;
        Entity = entity;
    }
    public abstract void Enter();
    public abstract void FixedUpdateState(float deltaTime);
    public abstract void UpdateState(float deltaTime);
    public abstract void Exit();
}
