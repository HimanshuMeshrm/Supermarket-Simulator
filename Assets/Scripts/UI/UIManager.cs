using TMPro;
using UnityEngine;

public class UIManager :  Singleton<UIManager>
{
    public EntityThoughtUI Prefab;
    public ObjectPool ThoughtsUIPool;

    public TMP_Text MoneyText;

    private void Awake()
    {
        CreateThoughtUIPool();
        GameManager.Instance.Player.MoneyAccount.OnMoneyChanged.AddListener(UpdateMoney);
    }
    public void CreateThoughtUIPool()
    {
        int count = 20;
        GameObject go = new GameObject("ThougsUIHolder");
        go.transform.SetParent(transform);
        ThoughtsUIPool = new ObjectPool(Prefab.gameObject, count, go.transform, "ThoughtUI");
    }
    void UpdateMoney(int money)
    {
        MoneyText.text = money.ToString();
    }
}
