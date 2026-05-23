using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace _2026_Csh_Advanced.sprint5_Collections
{
    public class Collections
    {
        public static void RunCollections()
        {
            Console.WriteLine("========== 1. List<T> – динамічний масив ==========");
            List<string> list = new List<string> { "apple", "banana" };
            Console.WriteLine("Початковий List: " + string.Join(", ", list));

            list.Add("cherry");
            Console.WriteLine("list.Add(\"cherry\") -> додали в кінець: " + string.Join(", ", list));

            list.Insert(1, "blueberry");
            Console.WriteLine("list.Insert(1, \"blueberry\") -> вставили на позицію 1: " + string.Join(", ", list));

            list.Remove("banana");
            Console.WriteLine("list.Remove(\"banana\") -> видалили 'banana': " + string.Join(", ", list));

            list.RemoveAt(0);
            Console.WriteLine("list.RemoveAt(0) -> видалили елемент з індексом 0: " + string.Join(", ", list));

            bool hasApple = list.Contains("apple");
            Console.WriteLine($"list.Contains(\"apple\") -> {hasApple}");

            int index = list.IndexOf("cherry");
            Console.WriteLine($"list.IndexOf(\"cherry\") -> індекс {index}");

            list.Sort();
            Console.WriteLine("list.Sort() -> відсортували: " + string.Join(", ", list));

            list.Reverse();
            Console.WriteLine("list.Reverse() -> обернули порядок: " + string.Join(", ", list));

            string[] arr = list.ToArray();
            Console.WriteLine("list.ToArray() -> перетворили в масив: " + string.Join(", ", arr));
            Console.WriteLine();

            Console.WriteLine("========== 2. Dictionary<TKey, TValue> – словник ==========");
            Dictionary<int, string> dict = new Dictionary<int, string>();
            dict.Add(1, "Alice");
            Console.WriteLine("dict.Add(1, \"Alice\") -> додали пару (1, Alice)");
            dict[2] = "Bob";
            Console.WriteLine("dict[2] = \"Bob\" -> додали/перезаписали ключ 2 значенням Bob");
            bool added = dict.TryAdd(2, "Bill");
            Console.WriteLine($"dict.TryAdd(2, \"Bill\") -> {(added ? "додано" : "не додано, ключ вже існує")}");
            string val = dict[1];
            Console.WriteLine($"dict[1] -> отримали значення: {val}");
            bool gotValue = dict.TryGetValue(3, out string? val3);
            Console.WriteLine($"dict.TryGetValue(3, out val3) -> {(gotValue ? $"знайдено {val3}" : "ключ 3 не знайдено")}");
            bool containsKey = dict.ContainsKey(2);
            Console.WriteLine($"dict.ContainsKey(2) -> {containsKey}");
            dict.Remove(1);
            Console.WriteLine("dict.Remove(1) -> видалили ключ 1");
            Console.WriteLine("Вміст словника після операцій:");
            foreach (var kvp in dict)
                Console.WriteLine($"  {kvp.Key} -> {kvp.Value}");
            Console.WriteLine();

            Console.WriteLine("========== 3. Queue<T> – черга FIFO ==========");
            Queue<string> queue = new Queue<string>();
            queue.Enqueue("first");
            Console.WriteLine("queue.Enqueue(\"first\") -> додали 'first'");
            queue.Enqueue("second");
            Console.WriteLine("queue.Enqueue(\"second\") -> додали 'second'");
            string first = queue.Dequeue();
            Console.WriteLine($"queue.Dequeue() -> витягли '{first}' з початку");
            string peek = queue.Peek();
            Console.WriteLine($"queue.Peek() -> дивимось на початок без видалення: '{peek}'");
            bool isEmpty = queue.Count == 0;
            Console.WriteLine($"queue.Count == 0 -> {isEmpty}");
            Console.WriteLine($"Поточна черга: {string.Join(", ", queue)}");
            Console.WriteLine();

            Console.WriteLine("========== 4. Stack<T> – стек LIFO ==========");
            Stack<string> stack = new Stack<string>();
            stack.Push("first");
            Console.WriteLine("stack.Push(\"first\") -> поклали 'first' на вершину");
            stack.Push("second");
            Console.WriteLine("stack.Push(\"second\") -> поклали 'second' на вершину");
            string top = stack.Pop();
            Console.WriteLine($"stack.Pop() -> зняли з вершини '{top}'");
            string topPeek = stack.Peek();
            Console.WriteLine($"stack.Peek() -> дивимось на вершину без видалення: '{topPeek}'");
            Console.WriteLine($"Поточний стек (зверху вниз): {string.Join(", ", stack)}");
            Console.WriteLine();

            Console.WriteLine("========== 5. HashSet<T> – множина унікальних елементів ==========");
            HashSet<int> set1 = new HashSet<int> { 1, 2, 3 };
            Console.WriteLine("Початковий set1: " + string.Join(", ", set1));
            HashSet<int> set2 = new HashSet<int> { 2, 3, 4 };
            Console.WriteLine("set2: " + string.Join(", ", set2));
            set1.Add(4);
            Console.WriteLine("set1.Add(4) -> додали 4: " + string.Join(", ", set1));
            bool addedDuplicate = set1.Add(2);
            Console.WriteLine($"set1.Add(2) -> {(addedDuplicate ? "додано" : "не додано, дублікат")}");
            set1.UnionWith(set2);
            Console.WriteLine("set1.UnionWith(set2) -> об'єднання: " + string.Join(", ", set1));
            set1.IntersectWith(new[] { 2, 3, 5 });
            Console.WriteLine("set1.IntersectWith(new[] {2,3,5}) -> перетин: " + string.Join(", ", set1));
            set1.ExceptWith(new[] { 2 });
            Console.WriteLine("set1.ExceptWith(new[] {2}) -> різниця: " + string.Join(", ", set1));
            bool isSubset = set1.IsSubsetOf(set2);
            Console.WriteLine($"set1.IsSubsetOf(set2) -> {isSubset}");
            Console.WriteLine();

            Console.WriteLine("========== 6. LinkedList<T> – двозв'язний список ==========");
            LinkedList<string> linked = new LinkedList<string>();
            linked.AddLast("last");
            Console.WriteLine("linked.AddLast(\"last\") -> додали в кінець: " + string.Join(", ", linked));
            linked.AddFirst("first");
            Console.WriteLine("linked.AddFirst(\"first\") -> додали на початок: " + string.Join(", ", linked));
            var node = linked.Find("last");
            linked.AddBefore(node!, "beforeLast");
            Console.WriteLine("linked.AddBefore(node, \"beforeLast\") -> вставили перед 'last': " + string.Join(", ", linked));
            linked.AddAfter(node!, "afterLast");
            Console.WriteLine("linked.AddAfter(node, \"afterLast\") -> вставили після 'last': " + string.Join(", ", linked));
            linked.Remove("first");
            Console.WriteLine("linked.Remove(\"first\") -> видалили 'first': " + string.Join(", ", linked));
            linked.RemoveFirst();
            Console.WriteLine("linked.RemoveFirst() -> видалили перший вузол: " + string.Join(", ", linked));
            linked.RemoveLast();
            Console.WriteLine("linked.RemoveLast() -> видалили останній вузол: " + string.Join(", ", linked));
            Console.WriteLine();

            Console.WriteLine("========== 7. SortedDictionary<TKey, TValue> – відсортований словник ==========");
            SortedDictionary<string, int> sortedDict = new SortedDictionary<string, int>();
            sortedDict["banana"] = 2;
            Console.WriteLine("sortedDict[\"banana\"] = 2");
            sortedDict["apple"] = 5;
            Console.WriteLine("sortedDict[\"apple\"] = 5");
            sortedDict["cherry"] = 1;
            Console.WriteLine("sortedDict[\"cherry\"] = 1");
            Console.WriteLine("Елементи в порядку ключів:");
            foreach (var kvp in sortedDict)
                Console.WriteLine($"  {kvp.Key} -> {kvp.Value}");
            Console.WriteLine();

            Console.WriteLine("========== 8. SortedList<TKey, TValue> – гібрид словника та списку ==========");
            SortedList<string, int> sortedList = new SortedList<string, int>();
            sortedList.Add("x", 10);
            Console.WriteLine("sortedList.Add(\"x\", 10)");
            sortedList.Add("y", 20);
            Console.WriteLine("sortedList.Add(\"y\", 20)");
            sortedList.Add("z", 30);
            Console.WriteLine("sortedList.Add(\"z\", 30)");
            int valueAtIndex = sortedList.Values[1];
            Console.WriteLine($"sortedList.Values[1] -> {valueAtIndex} (значення за індексом 1)");
            int indexOfKey = sortedList.IndexOfKey("y");
            Console.WriteLine($"sortedList.IndexOfKey(\"y\") -> {indexOfKey} (індекс ключа 'y')");
            Console.WriteLine();

            Console.WriteLine("========== 9. ObservableCollection<T> – сповіщає про зміни ==========");
            ObservableCollection<string> observable = new ObservableCollection<string>();
            observable.CollectionChanged += (sender, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                    Console.WriteLine($"  Подія: Додано -> {e.NewItems?[0]}");
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                    Console.WriteLine($"  Подія: Видалено -> {e.OldItems?[0]}");
            };
            observable.Add("item1");
            Console.WriteLine("observable.Add(\"item1\")");
            observable.Remove("item1");
            Console.WriteLine("observable.Remove(\"item1\")");
        }
    }
}