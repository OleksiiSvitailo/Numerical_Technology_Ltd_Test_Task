using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace Numerical_Technology_Ltd_Test_Task
{
    public class CustomCollection<T> :IEnumerable
                 where T : struct, 
                           IConvertible, 
                           IComparable
    {
        private Dictionary<int, T> collection;

        public T Max { get; private set; }

        public int Length { get => collection.Count; }

        private bool IsNumericType()
        {
            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
            }
            return false;
        }

        public CustomCollection()
        {
            if (IsNumericType())
                collection = new Dictionary<int, T>();
            else
                throw new InvalidOperationException("Type of collection must be numeric");
        }

        public void Add(T item)
        {
            if (item.CompareTo(Max) > 0)
                Max = item;
            collection.Add(Length, item);
        }

        public void InsertAt(int index, T item)
        {
            if (index < 0 || index >= Length)
                throw new ArgumentOutOfRangeException($"Collection doesn`t contains element with index:{index}");
            if (item.CompareTo(Max) > 0)
                Max = item;
            this.Add(collection[Length - 1]);
            for (int i = Length - 1; i > index; i--)
                collection[i] = collection[i - 1];
            collection[index] = item;
        }

        public bool Remove(T item)
        {
            for (int i = 0; i < Length; i++)
            {
                if (item.Equals(collection[i]))
                    return collection.Remove(i);
            }
            return false;
        }

        public bool RemoveAt(int index)
        {
            if(index<0||index>=Length)
            throw new ArgumentOutOfRangeException($"Collection doesn`t contains element with index:{index}");
            return collection.Remove(index);
        }

        public IEnumerator GetEnumerator() => collection.GetEnumerator();

        public T this[int index] { get => collection[index]; set => collection[index] = value; }

        public async Task<T> SingleAsync(Func<T, bool> predicate)
        {
            if (Length == 0)
                throw new ArgumentException("Collection must contains at least one member");
            if (predicate == null)
                throw new ArgumentNullException("Predicate can`t be a NULL");
            T result = collection[0];
            var resultsCount = 0;
            const int batchLength = 100;
            var threadsAmount = Length / batchLength + 1;
            List<Task> tasks = new List<Task>();
            for (var threadsNumber = 0; threadsNumber < threadsAmount; threadsNumber++)
                tasks.Add(Task.Run(() =>
                {
                    var startPosition = threadsNumber * batchLength;
                    var endPosition = startPosition + batchLength;
                    for (var index = startPosition; index < endPosition; index++)//lock?
                    {
                        if(predicate(collection[index]))
                        {
                             if (resultsCount>1)
                                 throw new ArgumentException("Collectoin contains more than one element which satisfy predicate");
                            result = collection[index];
                            resultsCount++;
                        }
                    }
                }));
            Task.WaitAll(tasks.ToArray());
            if(resultsCount==0)
                throw new ArgumentException("Collectoin doesn`t contains element which satisfy predicate");
            return result;
        }
    }
}