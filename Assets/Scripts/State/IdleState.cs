using UnityEngine;

public class IdleState : EntityState
{
    public IdleState(Entity entity, Animator animator) : base(entity, animator)
    {

    }

    public override string AnimName => "Idle";

    string[] Anims  = { "IdleA", "IdleB" };

    public override void Enter()
    {
        //int RandomIndex = Random.Range(0, Anims.Length);
       // string RandomAnim = Anims[RandomIndex];
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
