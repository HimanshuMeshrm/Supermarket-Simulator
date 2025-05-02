using UnityEngine;

public class UIManager :  Singleton<UIManager>
{
    public EntityThoughtUI Prefab;
    public ObjectPool ThoughtsUIPool;

    private void Awake()
    {
        CreateThoughtUIPool();
    }
    public void CreateThoughtUIPool()
    {
        int count = 20;
        GameObject go = new GameObject("ThougsUIHolder");
        go.transform.SetParent(transform);
        ThoughtsUIPool = new ObjectPool(Prefab.gameObject, count, go.transform, "ThoughtUI");
    }
}
