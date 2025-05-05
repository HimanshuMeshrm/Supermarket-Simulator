using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Customer : Entity, IInteractor
{
    private const string IdleAnim = "Idle";
    private const string WalkAnim = "Walk";
    private const string RunAnim = "Run";

   
    private string _currentAnim;
    [field: SerializeField] public Interactable _current { get; set; }

    public bool IsInteracting { get; set; }
    public bool HasBeenBilled { get; set; }
    public string TaskDebug;

    [SerializeField] public ShoppingList _shoppingList { get; private set; } = new ShoppingList();
    [SerializeField] public ItemRequest _currentItemRequest;

    private Queue<ITask> _tasks = new Queue<ITask>();
    private ITask _currentTask;


    public Cart _currentCart { get; set; }
    public void InitializeCustomer()
    {
        thoughtUI = UIManager.Instance.ThoughtsUIPool.Get().GetComponent<EntityThoughtUI>();
        thoughtUI.Follow(this);
        HasBeenBilled = false;
        MoneyAccount = new MoneyAccount();
        SetEntityType(EntityType.Customer);
        SetShoppingList();
        InitializeTasks();
    }
    private void InitializeTasks()
    {
        _tasks.Clear();
        _tasks.Enqueue(new GetCartTask());
        _tasks.Enqueue(new ShopTask());
        _tasks.Enqueue(new GoToCashCounterTask());
        _tasks.Enqueue(new ShopExitTask());

        SetNextTask();
    }

    private void SetNextTask()
    {
        if (_tasks.Count == 0)
        {
            _currentTask = null;
            Debug.Log($"No tasks left for customer: {name}");
            return;
        }

        _currentTask = _tasks.Dequeue();
        TaskDebug = _currentTask.GetType().Name;
        Debug.Log($"Starting task: {TaskDebug} for customer: {name}");
        _currentTask.Start(this);
    }

    public override void Update()
    {
        UpdateAnimationState();

        if (_currentTask != null)
        {
            _currentTask.Update(this);
            if (_currentTask.IsCompleted)
            {
                Debug.Log($"Task {TaskDebug} completed for customer: {name}");
                SetNextTask();
            }
        }

        base.Update();
    }

    private void UpdateAnimationState()
    {
        NavMeshAgent.speed = NavMeshAgent.isPathStale ? RunSpeed : WalkSpeed;

        bool isMoving = NavMeshAgent.velocity.magnitude > 0.1f;

        string newAnim = isMoving
            ? (NavMeshAgent.remainingDistance > 10f ? RunAnim : WalkAnim)
            : IdleAnim;

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
        if(_current is Cart cart)
        {
            cart.SetInteractor(this);
        }
    }

    public void Interact()
    {
        if (_current == null || IsInteracting)
            return;

        IsInteracting = true;
        Debug.Log($"{name} is trying to interact with {_current}");
        _current.TryInteract();
        IsInteracting = false;

        if (_current is Cart cart)
        {
            _currentCart = cart;
            _current = null;
        }
    }

    public bool HasMoreShoppingItems() => _shoppingList.HasItems;

    private void SetShoppingList()
    {
        _shoppingList = new ShoppingList();
        _shoppingList.AddItem(DataHolder.Instance.GetRandomItem(), Random.Range(0, 3));
        _shoppingList.AddItem(DataHolder.Instance.GetRandomItem(), Random.Range(0, 3));
        _shoppingList.AddItem(DataHolder.Instance.GetRandomItem(), Random.Range(0, 3));
       
        MoneyAccount.AddMoney(_shoppingList.GetTotalCost());
        Debug.Log($"Shopping list initialized for customer: {name}");
    }

    public void GoToNextItemShelf()
    {
        if (!_shoppingList.HasItems)
            return;

        _currentItemRequest = _shoppingList.DequeueItem();
        thoughtUI.SetIcon(_currentItemRequest.Data.Icon, $"{_currentCart.Inventory.GetItemCount(_currentItemRequest.Data) } / { _currentItemRequest.Quantity}");
        Shelve shelve = GameManager.Shop.GetShelve(_currentItemRequest.Data);

        Vector3 targetPosition;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(shelve.transform.position, out hit, 5f, NavMesh.AllAreas))
        {
            targetPosition = hit.position;
            SetDestination(targetPosition);
            SetInteractable(shelve);
            Debug.Log($"Customer {name} going to shelf for item: {_currentItemRequest.Data.name}, target position: {targetPosition}");
        }
        else
        {
            Debug.LogWarning($"No valid NavMesh position found near shelf for item: {_currentItemRequest.Data.name}");
        }
    }


    public override void SetEntityType(EntityType type)
    {
        this.Type = type;
    }

    private void OnDrawGizmos()
    {
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, 0.3f);

       
        if (NavMeshAgent.hasPath)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, NavMeshAgent.destination);
        }
    }
}
