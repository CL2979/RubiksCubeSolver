using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCubeSolver
{
    public class Queue<T>
    {
        private T[] queue;
        private HashSet<T> contains;
        private int maxSize;
        private int size;
        private int frontptr;
        private int backptr;
        public Queue(int maxSize)
        {
            queue = new T[maxSize];
            contains = new HashSet<T>();
            this.maxSize = maxSize;
            size = 0;
            frontptr = 0;
            backptr = -1;
        }
        public void Enqueue(T item)
        {
            backptr = (backptr + 1) % maxSize;
            queue[backptr] = item;
            size++;
        }
        public T Dequeue()
        {
            if (size > 0)
            {
                T item = queue[frontptr];
                frontptr = (frontptr + 1) % maxSize;
                size--;
                return item;
            }
            else
            {
                throw new InvalidOperationException("Queue is empty.");
            }
        }
        public T Peek()
        {
            if (size > 0)
            {
                return queue[frontptr];
            }
            else
            {
                throw new InvalidOperationException("Queue is empty.");
            }
        }
        public bool IsEmpty()
        {
            return size == 0;
        }
        public long Count()
        {
            return size;
        }
        public bool Contains(T item)
        {
            return contains.Contains(item);
        }
        public void Clear()
        {
            Array.Clear(queue, 0, maxSize);
            size = 0;
            frontptr = 0;
            backptr = -1;
            contains.Clear();
        }
    }
}
