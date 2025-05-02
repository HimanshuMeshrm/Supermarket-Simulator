using Unity.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class StateController : MonoBehaviour
{
    #region Serialized Fields (Debug Only)

    [SerializeField, ReadOnly] public string currentStateDebug;

    #endregion

    #region Protected Properties

    protected EntityState CurrentState { get; private set; }
    protected float DeltaTime => Time.deltaTime;
    protected float FixedDeltaTime => Time.fixedDeltaTime;

    #endregion

    #region Component References

    [field: SerializeField] public Animator Animator { get; protected set; }
    [field: SerializeField] public Rigidbody Rigidbody { get; protected set; }
    [field: SerializeField] public Collider Collider { get; protected set; }

    #endregion

    #region Public API

    public void SwitchState(EntityState newState)
    {
        if (newState == null || CurrentState?.GetType() == newState.GetType())
            return;

        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState?.Enter();
        
        UpdateDebugStateName();
    }

    #endregion

    #region Unity Lifecycle

    public virtual void Update()
    {
        CurrentState?.UpdateState(DeltaTime);
        CustomUpdate(DeltaTime);
    }

    public virtual void FixedUpdate()
    {
        CurrentState?.FixedUpdateState(FixedDeltaTime);
        CustomFixedUpdate(FixedDeltaTime);
    }

    #endregion

    #region Protected Abstracts

    protected abstract void CustomUpdate(float deltaTime);
    protected abstract void CustomFixedUpdate(float deltaTime);

    #endregion

    #region Debug

    private void UpdateDebugStateName()
    {
#if UNITY_EDITOR
        currentStateDebug = CurrentState?.GetType().Name ?? "None";
#endif
    }

    #endregion
}
