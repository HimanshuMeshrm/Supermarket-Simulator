using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    [SerializeField] private List<ObjectPool> ItemPools = new List<ObjectPool>();
    private Transform _itemHolder;


    private Dictionary<int, ObjectPool> _customerPoolLookup = new();

    [SerializeField] private List<ObjectPool> CustomerPool;
    private Transform _customerHolder;
    public int ActiveCustomerCount
    {
        get
        {
            int count = 0;
            foreach (ObjectPool pool in CustomerPool)
            {
                count += pool.Active;
            }
            return count;
        }
    }

        [SerializeField] public ObjectPool CartPool { get; private set; }
    private Transform _cartHolder;

    private void Awake()
    {
        CreateItemPool();
        CreateCartPool();
        CreateCustomerPool();
    }
    private void CreateCartPool()
    {
        GameObject CartPrefab = DataHolder.Instance.CartPrefab.gameObject;
        int count = 10;
        _cartHolder = new GameObject("CartHolder").transform;
        _cartHolder.SetParent(transform);
        CartPool = new ObjectPool(CartPrefab, count, _cartHolder.transform, CartPrefab.name);
    }
    private void CreateCustomerPool()
    {
        _customerHolder = new GameObject("CustomerHolder").transform;
        _customerHolder.SetParent(transform);
        CustomerPool = new List<ObjectPool>();
        _customerPoolLookup = new Dictionary<int, ObjectPool>();
        int id = 0;
        foreach (Customer customer in DataHolder.Instance.CustomersPrefabs)
        {
            if (customer == null)
            {
                CustomerPool.Add(null);
                continue;
            }
            id++;
            
            ObjectPool pool = new ObjectPool(customer.gameObject, 5, _customerHolder, customer.name, id);
            CustomerPool.Add(pool);
            _customerPoolLookup[id] = pool;
        }
    }
    private void CreateItemPool()
    {
        List<ItemData> datas = DataHolder.Instance.Items;
        _itemHolder = new GameObject("ItemHolder").transform;
        _itemHolder.SetParent(transform);
        foreach (ItemData item in datas)
        {
            if (item == null)
            {
                ItemPools.Add(null);
                continue;
            }


            if (item.ItemObject == null)
            {
                ItemPools.Add(null);
                Debug.LogError("Gameobject is null");
                continue;
            }

            ObjectPool pool = new ObjectPool(item.ItemObject.gameObject, 5, _itemHolder.transform, item.Name);
            ItemPools.Add(pool);
        }


    }
    public Cart GetCart() => CartPool.Get().GetComponent<Cart>();
    public Customer GetRandomCustomer()
    {
        int pool = Random.Range(0, CustomerPool.Count);
        Customer obj = CustomerPool[pool].Get().GetComponent<Customer>();
        obj.PrefabID = CustomerPool[pool].PoolID;
        return obj;
    }
    public ObjectPool GetItemPool(ItemData Item)
    {
        foreach (ObjectPool pool in ItemPools)
        {
            if (pool.Name == Item.Name)
            {
                return pool;
            }
        }
        return null;
    }
    public ObjectPool GetCustomerPool(int prefabID)
    {
        if (_customerPoolLookup.TryGetValue(prefabID, out var pool))
            return pool;

        Debug.LogWarning($"No customer pool found for prefab: {prefabID}");
        return null;
    }

}
