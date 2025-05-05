using Unity.AI.Navigation;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Player Player;
    public NavMeshSurface NavMeshSurface;
    public IsometricCameraController Camera;

    [SerializeField] private Shop _shop;

    [Range(0,10)] public float TimeScale = 1f;
    public static Shop Shop => Instance._shop;

    private float _spawnTimer;

 
    public float SpawnInterval => Mathf.Max(1f, 6f - Shop.ShopLevel);

    
    public int MaxCustomers => Shop.ShopLevel * 3;

    private void Awake()
    {
        _ = Instance;
    }
    private void Start()
    {
        SpawnCustomer();
    }
    public void Update()
    {
        if (UIManager.Instance.IsPaused) return;
        Time.timeScale = TimeScale;

        
        _spawnTimer += Time.deltaTime;

        if (_spawnTimer >= SpawnInterval)
        {
            _spawnTimer = 0f;

            if (PoolManager.Instance.ActiveCustomerCount < MaxCustomers)
            {
                SpawnCustomer();
            }
        }
    }
    public void SpawnCustomer()
    {
        Customer customer = PoolManager.Instance.GetRandomCustomer();

        Vector3 spawnPosition = Shop.ShopExitPosition.position;

        customer.NavMeshAgent.SetPosition(spawnPosition);

        customer.InitializeCustomer();
    }
}
