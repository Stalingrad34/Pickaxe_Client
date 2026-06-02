using System.Collections;
using System.Collections.Generic;

namespace Core.Scripts.Loggers
{
    public class LimitStack<T> : IEnumerable<T>
    {
        private T[] _elements;
        private int _length;
        private int _current = -1;

        public LimitStack(int length)
        {
            _length = length;
            _elements = new T[_length];
        }

        public void Add(T item)
        {
            _current++;
            if (_current == _length)
                _current = 0;
            
            _elements[_current] = item;
        }

        public void Clear()
        {
            _elements = new T[_length];
            _current = -1;
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            var iterator = _current;
            if (iterator == -1)
                yield break;
      
            do
            {
                var element = _elements[iterator];
                if (element == null)
                    yield break;

                iterator = iterator - 1 < 0 ? _length - 1 : iterator - 1;
                yield return element;
            } 
            while (iterator != _current);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}