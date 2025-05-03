using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class MoneyAccount
{
    public int Money { get; private set; }
    public UnityEvent<int> OnMoneyChanged = new UnityEvent<int>();

    public MoneyAccount(int initialAmount = 0)
    {
        Money = initialAmount;
    }
    public void AddMoney(int amount)
    {
        Money += amount;
        OnMoneyChanged?.Invoke(Money);
    }
    public void RemoveMoney(int amount)
    {
        Money -= amount;
        OnMoneyChanged?.Invoke(Money);
    }
}
