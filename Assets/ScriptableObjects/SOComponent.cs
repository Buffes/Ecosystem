using System.Collections.Generic;
using UnityEngine;

namespace Ecosystem
{
    /// <summary>
    /// Scriptable object version of the composite pattern.
    /// </summary>
    /// <typeparam name="T">Leaf type</typeparam>
    public abstract class SOComponent<T> : ScriptableObject
    {
        /// <summary>
        /// Adds all the leaves under this component to the specified list.
        /// </summary>
        public abstract void AddLeaves(List<T> list);

        /// <summary>
        /// Returns all the leaves under the specified components.
        /// </summary>
        public static List<T> GetLeaves<E>(List<E> components) where E : SOComponent<T>
        {
            List<T> list = new List<T>();

            foreach (SOComponent<T> component in components)
            {
                component.AddLeaves(list);
            }

            return list;
        }
    }
}
