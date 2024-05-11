
using System.Collections.Generic;

namespace KrazyKrakenGames.DetectiveGame.Utility
{
    public class MessageQueue<T>
    {
        private Queue<T> queue;

        public MessageQueue() 
        { 
            queue = new Queue<T>();
        }

        public void Add(T item)
        {
            queue.Enqueue(item);
        }

        public T Pop()
        {
            if (queue.Count > 0)
            {
                return queue.Dequeue();
            }
            else
            {
                return default(T);
            }
        }

        public T Peek()
        {
            if(queue.Count > 0)
            {
                return queue.Peek();
            }
            else
            {
                return default(T);
            }
        }

        public int Count => queue.Count;

        public void Clear() => queue.Clear();
    }
}
