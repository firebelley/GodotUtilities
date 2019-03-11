using System;
using System.Collections.Generic;
using System.Linq;

namespace GodotTools.Logic
{
    public class LootTable<T>
    {
        public int WeightSum { get; protected set; }
        private List<TableData> _table = new List<TableData>();
        private Random _random = new Random();

        public class TableData
        {
            public T Obj;
            public int Weight;

            public TableData(T o, int w)
            {
                Obj = o;
                Weight = w;
            }
        }

        public void SetSeed(int seed)
        {
            _random = new Random(seed);
        }

        public void SetRandom(Random random)
        {
            _random = random;
        }

        public void AddItem(T obj, int weight)
        {
            _table.Add(new TableData(obj, weight));
            CalculateWeightSum();
        }

        public void AddRange(List<TableData> range)
        {
            _table.AddRange(range);
            CalculateWeightSum();
        }

        public T PickItem()
        {
            return PickItem(_table, WeightSum);
        }

        public T PickRange(int startIdx, int count)
        {
            var range = _table.GetRange(startIdx, count);
            var weightSum = range.Sum(x => x.Weight);
            return PickItem(range, weightSum);
        }

        public List<T> GetLootTableItems()
        {
            return _table.Select(x => x.Obj).ToList();
        }

        public List<TableData> GetLootTableData()
        {
            return _table;
        }

        public int GetCount()
        {
            return _table.Count;
        }

        public void CalculateWeightSum()
        {
            WeightSum = _table.Sum(x => x.Weight);
        }

        private T PickItem(List<TableData> table, int weightSum)
        {
            int sum = 0;
            int val = _random.Next(0, weightSum);
            foreach (var data in table)
            {
                sum += data.Weight;
                if (val < sum)
                {
                    return data.Obj;
                }
            }
            return default(T);
        }

    }

}