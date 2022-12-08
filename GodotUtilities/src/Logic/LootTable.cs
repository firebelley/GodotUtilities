using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace GodotUtilities.Logic
{
    public class LootTable<T>
    {
        public int WeightSum { get; protected set; }
        private readonly List<TableData> table = new();
        private RandomNumberGenerator random;

        public class TableData
        {
            public T Obj { get; private set; }
            public int Weight { get; private set; }

            public TableData(T o, int w)
            {
                Obj = o;
                Weight = w;
            }
        }

        public LootTable()
        {
            random = MathUtil.RNG;
        }

        public void SetSeed(ulong seed)
        {
            random.Seed = seed;
        }

        public void SetRandom(RandomNumberGenerator random)
        {
            this.random = random;
        }

        public void AddItem(T obj, int weight)
        {
            table.Add(new TableData(obj, weight));
            CalculateWeightSum();
        }

        public void AddItem(TableData tableData)
        {
            table.Add(tableData);
            CalculateWeightSum();
        }

        public void AddRange(List<TableData> range)
        {
            table.AddRange(range);
            CalculateWeightSum();
        }

        public void SetData(List<TableData> tableData)
        {
            table.Clear();
            AddRange(tableData);
        }

        public T PickItem()
        {
            return PickItem(table, WeightSum);
        }

        public TableData PickTableData()
        {
            return PickTableData();
        }

        /// <summary>
        /// Picks an item from the loot table and returns it if <paramref name="canPickConditionalFn"/> returns <c>true</c>. 
        /// </summary>
        /// <param name="canPickConditionalFn"></param>
        /// <returns></returns>
        public T PickItemConditional(Func<TableData, bool> canPickConditionalFn)
        {
            var tableCopy = table.ToList();
            var weightSum = WeightSum;

            TableData pickedTableData;
            do
            {
                pickedTableData = PickTableData(tableCopy, weightSum);
                if (pickedTableData != null)
                {
                    weightSum -= pickedTableData.Weight;
                    tableCopy.Remove(pickedTableData);
                }
            } while (pickedTableData != null && !canPickConditionalFn(pickedTableData));

            return pickedTableData != null ? pickedTableData.Obj : default;
        }

        public T PickRange(int startIdx, int count)
        {
            var range = table.GetRange(startIdx, count);
            var weightSum = range.Sum(x => x.Weight);
            return PickItem(range, weightSum);
        }

        public List<T> GetLootTableItems()
        {
            return table.Select(x => x.Obj).ToList();
        }

        public List<TableData> GetLootTableData()
        {
            return table;
        }

        public int GetCount()
        {
            return table.Count;
        }

        public void CalculateWeightSum()
        {
            WeightSum = table.Sum(x => x.Weight);
        }

        private T PickItem(List<TableData> table, int weightSum)
        {
            var tableData = PickTableData(table, weightSum);
            return tableData != null ? tableData.Obj : default;
        }

        private TableData PickTableData(List<TableData> table, int weightSum)
        {
            int sum = 0;
            int val = random.RandiRange(1, weightSum);
            foreach (var data in table)
            {
                sum += data.Weight;
                if (val <= sum)
                {
                    return data;
                }
            }
            return null;
        }
    }
}
