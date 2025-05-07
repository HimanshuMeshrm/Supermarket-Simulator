using UnityEngine;
using System.Collections.Generic;

public class Shop : MonoBehaviour
{
    public string ShopName = "Shanu Supermarket";
    public int ShopLevel => CalculateShopLevel();

    public List<Shelve> Shelves = new List<Shelve>();
    public List<Employee> Employees = new List<Employee>();
    public List<CashCounter> CashCounters = new List<CashCounter>();

    public Transform CartSpawnPosition;
    public Transform ShopExitPosition;

    void Awake()
    {
        Shelves.AddRange(FindObjectsByType<Shelve>(FindObjectsSortMode.None));
        Employees.AddRange(FindObjectsByType<Employee>(FindObjectsSortMode.None));
        CashCounters.AddRange(FindObjectsByType<CashCounter>(FindObjectsSortMode.None));
    }

    private int CalculateShopLevel()
    {
        int shelfScore = Shelves.Count;
        int employeeScore = Employees.Count * 2;
        int counterScore = CashCounters.Count * 3;
        int totalScore = shelfScore + employeeScore + counterScore;

        if (totalScore >= 30) return 5;
        if (totalScore >= 20) return 4;
        if (totalScore >= 12) return 3;
        if (totalScore >= 6) return 2;
        return 1;
    }

    public CashCounter GetClosetCashCounter(Vector3 position)
    {
        CashCounter closest = null;
        float closestDistance = float.MaxValue;
        foreach (var cashCounter in CashCounters)
        {
            float distance = Vector3.Distance(position, cashCounter.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = cashCounter;
            }
        }
        return closest;
    }

    public Shelve GetShelve(ItemData item)
    {
        foreach (var shelve in Shelves)
        {
            if (shelve.Item == item)
            {
                return shelve;
            }
        }
        return null;
    }
}
