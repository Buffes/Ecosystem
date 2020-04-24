using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ecosystem
{
    /// <summary>
    /// Enumerable collection as a scriptable object.
    /// </summary>
    /// <typeparam name="T">Type of the values in the collection</typeparam>
    public abstract class CollectionSO<T> : ScriptableObject, IEnumerable<T>
    {
        [SerializeField]
        private List<T> values = new List<T>();

        /// <summary>
        /// The collection.
        /// </summary>
        public List<T> Values => values;

        public IEnumerator<T> GetEnumerator() => values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
