using UnityEngine;

public class WalkState : EntityState
{
    public WalkState(Entity entity, Animator animator) : base(entity, animator)
    {

    }

    public override string AnimName => "Walk";

    public override void Enter()
    {
        Animator.CrossFade(AnimName, 0f);
    }
    public override void Exit()
    {
        
    }

    public override void FixedUpdateState(float deltaTime)
    {
        
    }
    public override void UpdateState(float deltaTime)
    {
        
    }
}
