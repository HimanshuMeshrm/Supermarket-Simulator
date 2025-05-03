using UnityEngine;

public class ShopExitTask : ITask
{
    private bool _started;
    public bool IsCompleted { get; private set; }

    public void Start(Entity entity)
    {
        _started = true;
        Debug.Log($"Starting ShopExitTask for entity: {entity.name}");
        entity.SetDestination(GameManager.Shop.ShopExitPosition.position);
        entity.thoughtUI.SetIcon(DataHolder.Instance.GetTaskSprite(this.GetType().Name));
    }

    public void Update(Entity entity)
    {
        if (!_started) return;
        
        if (Vector3.Distance(entity.transform.position, GameManager.Shop.ShopExitPosition.position) <= 2f)
        {
            Debug.Log($"Entity {entity.name} has reached the shop exit position.");

            entity.ReturnToPool();
            entity.thoughtUI.ReturnToPool();
            IsCompleted = true;
        }
    }
}
