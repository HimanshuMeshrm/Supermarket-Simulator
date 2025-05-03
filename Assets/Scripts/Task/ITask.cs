public interface ITask
{
    bool IsCompleted { get; }
    void Start(Entity entity);
    void Update(Entity entity);
}
