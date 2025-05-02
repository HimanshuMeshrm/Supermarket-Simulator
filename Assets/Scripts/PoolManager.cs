using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    [SerializeField] private List<ObjectPool> ItemPools = new List<ObjectPool>();
    private Transform _itemHolder;

    [SerializeField] private ObjectPool CustomerPool;
    private Transform _customerHolder;

    [SerializeField] private ObjectPool CartPool;
    private Transform _cartHolder;

    private void Awake()
    {
        CreateItemPool();
        CreateCartPool();
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
        GameObject CartPrefab = DataHolder.Instance.CartPrefab.gameObject;
        int count = 10;
        _cartHolder = new GameObject("CartHolder").transform;
        _cartHolder.SetParent(transform);
        ObjectPool pool = new ObjectPool(CartPrefab, count, _cartHolder.transform, CartPrefab.name);
        ItemPools.Add(pool);
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
    public ObjectPool GetPool(ItemData Item)
    {
        foreach (ObjectPool pool in ItemPools)
        {
            if(pool.Name == Item.Name)
            {
                return pool;
            }
        }
        return null;
    }
}
