using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Poolparty
{
    /// <summary>
    /// A pool in which you can store any type of object.
    /// </summary>
    public class Pool<T>
    {
        private List<PoolItem<T>> m_items;                 // The items in the pool
        private Dictionary<T, PoolItem<T>> m_lookupTable;  // A lookup table for the items in the pool
        private Func<T> m_createFunc;                      // A function that creates new items for the pool
        private int m_lastItemGrabbed;                     // Tracks the last item that was grabbed from the pool

        /// <summary>
        /// The amount of items in this pool
        /// </summary>
        public int Size
        {
            get { return m_items.Count; }
        }

        /// <summary>
        /// The amount of items currently used
        /// </summary>
        public int UsedItems
        {
            get { return m_lookupTable.Count; }
        }

        /// <summary>
        /// Create a new pool object
        /// </summary>
        /// <param name="createFunc"></param>
        public Pool(Func<T> creationFunc, int startSize)
        {
            // Store the function that creates new items
            m_createFunc = creationFunc;

            // Create the lists
            m_items = new List<PoolItem<T>>();
            m_lookupTable = new Dictionary<T, PoolItem<T>>();

            // Warm the pool
            Warm(startSize);
        }
        
        /// <summary>
        /// Grabs a new item from the pool
        /// </summary>
        /// <returns></returns>
        public T GetItem()
        {
            // Loop through items in
            PoolItem<T> item = null;
            for (int i = 0; i < m_items.Count; i++)
            {
                // Increment the last item grabbed
                m_lastItemGrabbed++;
                if (m_lastItemGrabbed >= m_items.Count)
                {
                    m_lastItemGrabbed = 0;
                }

                // Find the next available item
                if (m_items[m_lastItemGrabbed].InUse) continue;
                else item = m_items[m_lastItemGrabbed];              
            }

            // In case that there are no available items, create a new one
            if (item == null) CreateItem();

            // Prepare item and return in
            item.Use();
            m_lookupTable.Add(item.Item, item);
            return item.Item;
        }

        /// <summary>
        /// Releases an item back into the pool
        /// </summary>
        /// <param name="item"></param>
        public void ReleaseItem(T item)
        {
            // Clear the item from pool as long as it exists
            if (m_lookupTable.ContainsKey(item))
            {
                PoolItem<T> containedItem = m_lookupTable[item];
                containedItem.Release();
                m_lookupTable.Remove(item);
            }

            // Fallback in case the item doesn't exist
            else
            {
                Debug.LogWarning("Item doesn't exist in pool");
            }
        }
        
        /// <summary>
        /// Creates a new item
        /// </summary>
        /// <returns></returns>
        private PoolItem<T> CreateItem()
        {
            // Create a new pool item
            PoolItem<T> poolItem = new PoolItem<T>();
            poolItem.Item = m_createFunc();

            // Return the pool item
            return poolItem;
        }

        /// <summary>
        /// Creates new items in the pool
        /// </summary>
        /// <param name="size"></param>
        private void Warm(int size)
        {
            for (int i = 0; i < size; i++)
            {
                CreateItem();
            }
        }
    }
}