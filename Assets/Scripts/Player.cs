using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : Entity, IInteractor
{
    public bool IsShiftPressed => Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    public Vector2 MoveInput => new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

    [SerializeField] private float Gravity = -9.81f;
    [SerializeField] private float RotationSpeed = 10f;

    private CharacterController _controller;
    private Vector3 _velocity;
    private string _currentAnim;

    private Interactable _current;
    public bool IsInteracting { get; set; }

    private PlayerInfo PlayerInfo = new PlayerInfo();

    public Transform defaultSpawn;
    private static class AnimStates
    {
        public const string Idle = "Idle";
        public const string Walk = "Walk";
        public const string Run = "Run";
    }

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }
    private void Start()
    {
        LoadPlayer();
        SetEntityType(EntityType.Player);
    }
    private void LoadPlayer()
    {
        PlayerInfo = SaveManager.GetCurrentGameSave().PlayerInfo;
        MoneyAccount.AddMoney(PlayerInfo.MoneyAccount.Money);

        if (PlayerInfo.PlayerPosition.ToVector3() == Vector3.zero)
        {
            transform.position = defaultSpawn.position;
        }
        else
        {
            transform.position = PlayerInfo.PlayerPosition.ToVector3();
        }
    }
    public override void Update()
    {
        HandleMovement();
        HandleInteraction();
        UpdateAnimationState();
        base.Update();
    }

    private void HandleMovement()
    {
        if (GameManager.Instance.Camera == null)
            return;

        Transform cam = GameManager.Instance.Camera.transform;

       
        Vector3 forward = cam.forward;
        Vector3 right = cam.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 inputDirection = (right * MoveInput.x + forward * MoveInput.y).normalized;

        if (inputDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
        }

        float speed = IsShiftPressed ? RunSpeed : WalkSpeed;
        Vector3 move = inputDirection * speed;

        _velocity.y += Gravity * Time.deltaTime;

        if (_controller.isGrounded && _velocity.y < 0f)
            _velocity.y = -2f;

        _controller.Move((move + _velocity) * Time.deltaTime);
    }

    private void UpdateAnimationState()
    {
        string newAnim;

        if (MoveInput == Vector2.zero)
            newAnim = AnimStates.Idle;
        else
            newAnim = IsShiftPressed ? AnimStates.Run : AnimStates.Walk;

        if (_currentAnim == newAnim)
            return;

        _currentAnim = newAnim;

        switch (_currentAnim)
        {
            case AnimStates.Idle:
                SwitchState(new IdleState(this, Animator));
                break;
            case AnimStates.Walk:
                SwitchState(new WalkState(this, Animator));
                break;
            case AnimStates.Run:
                SwitchState(new RunState(this, Animator));
                break;
        }
    }

    private void HandleInteraction()
    {
        if (_current != null && !IsInteracting && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    public void SetInteractable(Interactable interactable)
    {
        _current = interactable;
    }

    public void Interact()
    {
        if (_current == null) return;

        IsInteracting = true;
        _current.TryInteract();
        IsInteracting = false;
    }

    public override void SetEntityType(EntityType type)
    {
        this.Type = type;
    }
}
