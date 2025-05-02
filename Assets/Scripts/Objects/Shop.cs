using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public string ShopName = "Shanu Supermarket";

    public List<Shelve> Shelves = new List<Shelve>();
    public List<Employee> Employees = new List<Employee>();
    public List<CashCounter> CashCounters = new List<CashCounter>();

    public Transform CartSpawnPosition;
    public Transform ShopExitPosition;

    void Awake()
    {
        List<Shelve> shelve = FindObjectsByType<Shelve>(sortMode: FindObjectsSortMode.None).ToList();
        Shelves.AddRange(shelve);

        List<Employee> Employee = FindObjectsByType<Employee>(sortMode: FindObjectsSortMode.None).ToList();
        Employees.AddRange(Employee);

        List<CashCounter> CashCounter = FindObjectsByType<CashCounter>(sortMode: FindObjectsSortMode.None).ToList();
        CashCounters.AddRange(CashCounter);
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
