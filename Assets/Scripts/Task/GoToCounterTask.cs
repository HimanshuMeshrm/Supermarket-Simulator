public class GoToCashCounterTask : ITask
{
    private bool _completed;
    private bool _queued;
    private CashCounter _targetCounter;

    public void Start(Entity entity)
    {
        var customer = (Customer)entity;
        _targetCounter = GameManager.Shop.GetClosetCashCounter(customer.transform.position);
        if (_targetCounter != null)
        {
            _queued = true;
            _targetCounter.QueueCustomer(customer);
        }
        entity.thoughtUI.SetIcon(DataHolder.Instance.GetTaskSprite(this.GetType().Name));
        
    }

    public void Update(Entity entity)
    {
        var customer = (Customer)entity;

        if (_queued && customer.HasBeenBilled)
        {
            _completed = true;
        }
    }

    public bool IsCompleted => _completed;
}
