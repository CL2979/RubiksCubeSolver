using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCubeSolver
{
    // this is very slow and was causing a whole load of errors so improve later
    public interface IPriorityQueue<T>
    {
        void Enqueue(T item, long priority);
        T Dequeue();
        T Peek();
        bool IsEmpty();
        bool IsFull();
        long Count();
    }
    public class Element<T>
    {
        public T value { get; set; }
        public long priority { get; set; }
        public Element(T value, long priority)
        {
            this.value = value;
            this.priority = priority;
        }
    }
    public class PRIORITYQUEUE<T> : IPriorityQueue<T>
    {
        private List<Element<T>> queue;
        private long maxSize;
        public PRIORITYQUEUE(long maxSize)
        {
            this.maxSize = maxSize;
            queue = new List<Element<T>>();
        }
        public void Enqueue(T item, long priority)
        {
            if (!IsFull())
            {
                Element<T> element = new Element<T>(item, priority);
                int index = queue.FindIndex(e => e.priority > priority);
                if (index == -1) queue.Add(element);
                else queue.Insert(index, element);
            }
            else throw new Exception("Cannot enqueue element because queue is full.");
        }
        public T Dequeue()
        {
            if (!IsEmpty())
            {
                Element<T> element = queue[0];
                queue.RemoveAt(0); // Remove the element at the front
                return element.value;
            }
            else throw new Exception("Cannot dequeue element because queue is empty.");
        }
        public T Peek()
        {
            if (!IsEmpty())
            {
                Element<T> element = queue[0];
                return element.value;
            }
            else throw new Exception("Cannot peek at element because queue is empty.");
        }
        public bool IsEmpty()
        {
            return queue.Count == 0;
        }
        public bool IsFull()
        {
            return queue.Count == maxSize;
        }
        public long Count()
        {
            return queue.Count;
        }
        public bool Contains(T item)
        {
            bool b = false;
            foreach (Element<T> element in queue)
            {
                if (EqualityComparer<T>.Default.Equals(element.value, item)) b = true;
            }
            return b;
        }
    }

// cant use array as it's too big
/*public class Element<T>
{
    public T value { get; set; }
    public long priority { get; set; }
    public Element(T value, long priority)
    {
        this.value = value;
        this.priority = priority;
    }
}
public class PRIORITYQUEUE<T> : IPriorityQueue<T>
{
    private Element<T>[] queue;
    private long size;
    private long maxSize;
    private long frontptr;
    private long backptr;
    public PRIORITYQUEUE(long maxSize)
    {
        this.maxSize = maxSize;
        size = 0;
        frontptr = 0;
        backptr = -1;
        queue = new Element<T>[maxSize];
    }
    public void Enqueue(T item, long priority)
    {
        if (!IsFull())
        {
            Element<T> element = new Element<T>(item, priority);
            long i;
            for (i = size - 1; i >= 0 && queue[i].priority > priority; i--) // making lowest value highest priority
            { // this is so that the heuristic function works properly
                queue[i + 1] = queue[i];
            }
            queue[i + 1] = element;
            backptr++;
            size++;
        }
        else
        {
            throw new Exception("Cannot enqueue element because queue is full.");
        }
    }
    public T Dequeue()
    {
        if (!IsEmpty())
        {
            Element<T> element = queue[frontptr];
            T item = element.value;
            size--;
            frontptr++;
            return item;
        }
        else
        {
            throw new Exception("Cannot dequeue element because queue is empty.");
        }
    }
    public T Peek()
    {
        if (!IsEmpty())
        {
            Element<T> element = queue[frontptr];
            T item = element.value;
            return item;
        }
        else
        {
            throw new Exception("Cannot peek at element because queue is empty.");
        }
    }
    public bool IsEmpty()
    {
        return size == 0;
    }
    public bool IsFull()
    {
        return size == maxSize;
    }
    public long Count()
    {
        return size;
    }
}*/
}
