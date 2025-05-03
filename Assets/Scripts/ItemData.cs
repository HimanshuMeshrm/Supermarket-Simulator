using UnityEngine;

[System.Serializable]

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData", order = 1)]
public class ItemData : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public float Price { get; private set; } = 99;
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public Item ItemObject { get; private set; }

    public void Initialize(int id)
    {
        this.ItemObject.SetData(this);
    }
    public GameObject GetGameObject() => PoolManager.Instance.GetItemPool(this).Get();
}
