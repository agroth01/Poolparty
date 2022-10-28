using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Poolparty
{
    /// <summary>
    /// This is a container for any items that are in a object pool.
    /// </summary>
    public class PoolItem<T>
    {
        public bool InUse { get; private set; }   // Is the item currently being used
        public T Item { get; set; } // The item that is being pooled

        /// <summary>
        /// Flags the item as in use
        /// </summary>
        public void Use()
        {
            InUse = true;
        }

        /// <summary>
        /// Flags item as no longer in use
        /// </summary>
        public void Release()
        {
            InUse = false;
        }
    }
}