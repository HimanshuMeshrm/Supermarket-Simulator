using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "ScriptableObjects/DataHolder", order = 1)]
public class DataHolder : ScriptableObject
{
    public List<ItemData> Items = new List<ItemData>();
    public Cart CartPrefab;
    public List<Customer> CustomersPrefabs = new List<Customer>();

    [Space(20)]

    [SerializeField] private SerializedDictionary<string, Sprite> TaskSprite = new SerializedDictionary<string, Sprite>();
    private static DataHolder _instance;
    public static DataHolder Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<DataHolder>("Database/DataHolder");

                if (_instance == null)
                {
                    Debug.LogError("❌ DataHolder.asset not found at Resources/Database/DataHolder!");
                }
                else
                {
                    Debug.Log("✅ DataHolder.asset loaded successfully.");
                }
            }
            return _instance;
        }
    }

    public Sprite GetTaskSprite(string name)
    {
        if (TaskSprite.ContainsKey(name)) return TaskSprite[name];
        return null;
    }
    public ItemData GetRandomItem()
    {
        int random = Random.Range(0, Items.Count);
        return Items[random];
    }
    [ContextMenu("Initialize")]
    private void OnValidate()
    {
        for (int i = 1; i < Items.Count; i++)
        {
            ItemData data = Items[i];
            data.Initialize(i);
        }
    }
}
