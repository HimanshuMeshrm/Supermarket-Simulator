using System.Collections.Generic;
using UnityEngine;

public class CashCounter : Interactable
{
    [SerializeField] private Transform queueStartPoint;
    [SerializeField] private float spacing = 1.5f;

    private Queue<Customer> _queue = new Queue<Customer>();
    private Customer _currentCustomer;

    private enum BillingState
    {
        Idle,
        WaitingForCustomer,
        Billing
    }

    private BillingState _billingState = BillingState.Idle;

    public Vector3 Position => queueStartPoint.position;

    private void Update()
    {
        UpdateQueuePositions();

        switch (_billingState)
        {
            case BillingState.Idle:
                if (_currentCustomer == null && _queue.Count > 0)
                {
                    _currentCustomer = _queue.Dequeue();
                    _billingState = BillingState.WaitingForCustomer;
                    _currentCustomer.SetDestination(queueStartPoint.position);
                }
                break;

            case BillingState.WaitingForCustomer:
                if (_currentCustomer != null &&
                    !_currentCustomer.NavMeshAgent.pathPending &&
                    _currentCustomer.NavMeshAgent.remainingDistance <= 1.2f &&
                    _currentCustomer.NavMeshAgent.velocity.sqrMagnitude <= 0.2f)
                {
                    _currentCustomer.thoughtUI.SetIcon(DataHolder.Instance.GetTaskSprite("Billing"));
                    _billingState = BillingState.Billing;
                    Invoke(nameof(FinishBilling), 2f);
                }
                break;

            case BillingState.Billing:
                _billingState = BillingState.Idle;
                break;
        }
    }

    public void QueueCustomer(Customer customer)
    {
        if (_queue.Contains(customer))
        {
            Debug.LogWarning($"{customer.name} is already in the queue.");
            return;
        }

        _queue.Enqueue(customer);
        Debug.Log($"Customer {customer.name} queued at {gameObject.name}");
    }

    private void UpdateQueuePositions()
    {
        if (_billingState == BillingState.Billing) return;

        Vector3 basePosition = queueStartPoint.position;
        Vector3 direction = -queueStartPoint.right.normalized;

        Customer[] queueArray = _queue.ToArray(); // Needed to index items in t Queue

        for (int i = 0; i < queueArray.Length; i++)
        {
            Customer customer = queueArray[i];
            Vector3 desiredPosition = basePosition + direction * spacing * (i + 1); // +1 since 0 is billing

            if (NavMeshHelper.IsPathClear(customer.transform.position, desiredPosition))
            {
                customer.SetDestination(desiredPosition);
            }
            else
            {
                Vector3 adjusted = NavMeshHelper.FindNearbyClearPosition(desiredPosition, customer.transform.position);
                customer.SetDestination(adjusted);
            }
        }
    }

    private void FinishBilling()
    {
        if (_currentCustomer != null)
        {
            
            GameManager.Instance.Player.MoneyAccount.AddMoney(_currentCustomer._currentCart.Inventory.GetTotalValue());
            _currentCustomer._currentCart.Inventory.Clear(true);
            _currentCustomer.LeaveCart(_currentCustomer);
            _currentCustomer._currentCart = null;
            _currentCustomer.HasBeenBilled = true;
            _currentCustomer = null;
           
        }
    }

    public override void Interact(IInteractor interactor) { }

    private void OnDrawGizmos()
    {
        if (!queueStartPoint) return;

        // Draw queue start marker
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(queueStartPoint.position, 0.25f);

        // Draw billing point
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(queueStartPoint.position, 0.35f);

        // Draw ideal queue positions
        Vector3 basePosition = queueStartPoint.position;
        Vector3 direction = -queueStartPoint.right.normalized;
        int index = 0;

        foreach (var customer in _queue)
        {
            Vector3 pos = basePosition + direction * spacing * (index + 1);
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(pos, 0.2f);
            index++;
        }
    }
}
