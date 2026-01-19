using System.Collections.Generic;

namespace GodotUtilities.Logic;

public class WeightedLootTable<T>(RandomNumberGenerator rng = null)
{
    public class LootItem
    {
        public T Item { get; }
        public int Weight { get; }

        public LootItem(T item, int weight)
        {
            if (weight <= 0) throw new ArgumentException("Weight must be greater than 0", nameof(weight));

            Item = item;
            Weight = weight;
        }
    }

    private readonly List<LootItem> items = [];
    private RandomNumberGenerator rng = rng ?? MathUtil.RNG;
    private int totalWeight = 0;

    public float TotalWeight => totalWeight;

    public int ItemCount => items.Count;

    public bool IsEmpty => items.Count == 0;

    public void SetRandom(RandomNumberGenerator newRng)
    {
        rng = newRng;
    }

    public void AddItem(T item, int weight)
    {
        var lootItem = new LootItem(item, weight);
        items.Add(lootItem);
        totalWeight += weight;
    }

    public bool RemoveItem(T item)
    {
        bool removed = false;
        for (int i = items.Count - 1; i >= 0; i--)
        {
            if (EqualityComparer<T>.Default.Equals(items[i].Item, item))
            {
                totalWeight -= items[i].Weight;
                items.RemoveAt(i);
                removed = true;
            }
        }
        return removed;
    }

    public void Clear()
    {
        items.Clear();
        totalWeight = 0;
    }

    public T PickItem()
    {
        if (IsEmpty) return default;

        int randomValue = rng.RandiRange(0, totalWeight);
        int currentWeight = 0;

        foreach (var item in items)
        {
            currentWeight += item.Weight;
            if (randomValue <= currentWeight) return item.Item;
        }

        return items[^1].Item;
    }

    public T PickItem(Func<T, bool> condition)
    {
        var eligibleItems = items.Where(item => condition(item.Item)).ToList();
        if (eligibleItems.Count == 0) return default;

        int totalEligibleWeight = eligibleItems.Sum(item => item.Weight);
        int randomValue = rng.RandiRange(0, totalEligibleWeight);
        int currentWeight = 0;

        foreach (var item in eligibleItems)
        {
            currentWeight += item.Weight;
            if (randomValue <= currentWeight) return item.Item;
        }

        return eligibleItems[^1].Item;
    }

    public List<T> PickItems(int count, bool allowDuplicates = true)
    {
        if (count <= 0) return [];

        if (IsEmpty) return [];

        var result = new List<T>();

        if (allowDuplicates)
        {
            for (int i = 0; i < count; i++)
            {
                result.Add(PickItem());
            }
        }
        else
        {
            var availableItems = new List<LootItem>(items);
            int availableWeight = totalWeight;

            for (int i = 0; i < count && availableItems.Count > 0; i++)
            {
                int randomValue = rng.RandiRange(0, availableWeight);
                int currentWeight = 0;

                for (int j = 0; j < availableItems.Count; j++)
                {
                    currentWeight += availableItems[j].Weight;
                    if (randomValue <= currentWeight)
                    {
                        result.Add(availableItems[j].Item);
                        availableWeight -= availableItems[j].Weight;
                        availableItems.RemoveAt(j);
                        break;
                    }
                }
            }
        }

        return result;
    }

    public List<T> PickItems(int count, Func<T, bool> condition, bool allowDuplicates = true)
    {
        if (count <= 0) return [];

        var eligibleItems = items.Where(item => condition(item.Item)).ToList();
        if (eligibleItems.Count == 0) return [];

        var result = new List<T>();

        if (allowDuplicates)
        {
            int totalEligibleWeight = eligibleItems.Sum(item => item.Weight);

            for (int i = 0; i < count; i++)
            {
                int randomValue = rng.RandiRange(0, totalEligibleWeight);
                int currentWeight = 0;

                foreach (var item in eligibleItems)
                {
                    currentWeight += item.Weight;
                    if (randomValue <= currentWeight)
                    {
                        result.Add(item.Item);
                        break;
                    }
                }
            }
        }
        else
        {
            var availableItems = new List<LootItem>(eligibleItems);
            int availableWeight = availableItems.Sum(item => item.Weight);

            for (int i = 0; i < count && availableItems.Count > 0; i++)
            {
                int randomValue = rng.RandiRange(0, availableWeight);
                int currentWeight = 0;

                for (int j = 0; j < availableItems.Count; j++)
                {
                    currentWeight += availableItems[j].Weight;
                    if (randomValue <= currentWeight)
                    {
                        result.Add(availableItems[j].Item);
                        availableWeight -= availableItems[j].Weight;
                        availableItems.RemoveAt(j);
                        break;
                    }
                }
            }
        }

        return result;
    }

    public IEnumerable<LootItem> GetAllItems()
    {
        return items.AsReadOnly();
    }

    public IEnumerable<T> GetItems(Func<T, bool> condition)
    {
        return items.Where(item => condition(item.Item)).Select(item => item.Item);
    }

    public float GetItemWeight(T item)
    {
        return items
            .Where(lootItem => EqualityComparer<T>.Default.Equals(lootItem.Item, item))
            .Sum(lootItem => lootItem.Weight);
    }

    public bool Contains(T item)
    {
        return items.Any(lootItem => EqualityComparer<T>.Default.Equals(lootItem.Item, item));
    }
}
