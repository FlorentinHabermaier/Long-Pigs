using System;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    [SerializeField, Min(0)] private int startingMoney = 500;

    public int Money { get; private set; }
    public int Goods { get; private set; }

    public event Action ResourceChanged;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        Money = startingMoney;
        Goods = 0;
    }

    public bool CanAfford(int cost)
    {
        return cost >= 0 && Money >= cost;
    }

    public bool TrySpendMoney(int amount)
    {
        if (!CanAfford(amount))
        {
            return false;
        }

        Money -= amount;
        ResourceChanged?.Invoke();

        return true;
    }

    public void AddMoney(int amount)
    {
        if(amount <= 0)
        {
            return;
        }

        Money += amount;
        ResourceChanged?.Invoke();
    }

    public void AddGoods(int amount)
    {
        if(amount <= 0)
        {
            return;
        }

        Goods += amount;
        ResourceChanged?.Invoke();
    }

    private void OnDestroy()
    {
        if(Instance == this)
        {
            Instance = null;
        }
    }
}
