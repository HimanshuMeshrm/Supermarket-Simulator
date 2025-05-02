using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectPool
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public GameObject Prefab { get; private set; }
    [field: SerializeField] public Transform Parent { get; private set; }

    private readonly Queue<GameObject> Pool = new();
    private readonly HashSet<GameObject> PooledSet = new();

    [field: SerializeField, Space(5)] public int Size { get; private set; }

    public int Active => Size - Pool.Count;
    public int NonActive => Pool.Count;
    public int Count => Pool.Count;

    public ObjectPool(GameObject prefab, int initialSize, Transform parent = null, string name = "PooledObj")
    {
        Prefab = prefab ?? throw new ArgumentNullException(nameof(prefab));
        Name = name;
        Parent = parent ?? new GameObject($"{name}_Parent").transform;

        for (int i = 0; i < initialSize; i++)
        {
            Create(i);
        }
    }

    private GameObject Create(int index)
    {
        GameObject obj = UnityEngine.Object.Instantiate(Prefab, Parent);
        obj.name = $"{Name}_{index:00}";
        PrepareObject(obj);
        return obj;
    }

    private void PrepareObject(GameObject obj)
    {
        obj.SetActive(false);
        Pool.Enqueue(obj);
        PooledSet.Add(obj);
        Size++;
    }

    private void Reparent(GameObject obj)
    {
        if (obj.transform.parent != Parent)
            obj.transform.SetParent(Parent, true);
    }

    public GameObject Get()
    {
        GameObject obj;

        if (Pool.Count > 0)
        {
            obj = Pool.Dequeue();
            PooledSet.Remove(obj);
        }
        else
        {
            Debug.Log($"[ObjectPool] {Name} exhausted. Creating new object.");
            obj = Create(Size);
        }

        Reparent(obj);
        obj.SetActive(true);
        return obj;
    }

    public GameObject Get(int instanceID)
    {
        foreach (var obj in Pool)
        {
            if (obj.GetInstanceID() == instanceID)
            {
                var tempQueue = new Queue<GameObject>();
                GameObject found = null;

                while (Pool.Count > 0)
                {
                    var item = Pool.Dequeue();
                    if (item.GetInstanceID() == instanceID && found == null)
                    {
                        found = item;
                        PooledSet.Remove(found);
                    }
                    else tempQueue.Enqueue(item);
                }

                while (tempQueue.Count > 0)
                    Pool.Enqueue(tempQueue.Dequeue());

                if (found != null)
                {
                    Reparent(found);
                    found.SetActive(true);
                    return found;
                }
            }
        }

        Debug.LogWarning($"[ObjectPool] InstanceID {instanceID} not found in {Name}. Returning any available.");
        return Get();
    }

    public void ReturnToPool(GameObject obj)
    {
        if (obj == null)
        {
            Debug.LogError("[ObjectPool] Tried to return null object.");
            return;
        }

        if (PooledSet.Contains(obj))
        {
            Debug.LogWarning($"[ObjectPool] {obj.name} already returned.");
            return;
        }

        obj.SetActive(false);
        Reparent(obj);
        Pool.Enqueue(obj);
        PooledSet.Add(obj);
    }

    public void Clear()
    {
        foreach (var obj in Pool)
        {
            if (obj != null)
                UnityEngine.Object.Destroy(obj);
        }

        Pool.Clear();
        PooledSet.Clear();
        Size = 0;
    }
}
