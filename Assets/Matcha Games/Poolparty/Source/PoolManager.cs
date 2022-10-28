using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Poolparty
{
    /// <summary>
    /// The class for managing the pools
    /// </summary>
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance { get; private set; }

        public Transform m_rootObject; // The root object for all the pools

        private Dictionary<GameObject, Pool<GameObject>> m_prefabLookupTable;
        private Dictionary<GameObject, Pool<GameObject>> m_instanceLookupTable;

        private void Awake()
        {
            // Load static instance
            Instance = this;

            // Create lookup tables
            m_prefabLookupTable = new Dictionary<GameObject, Pool<GameObject>>();
            m_instanceLookupTable = new Dictionary<GameObject, Pool<GameObject>>();
        }

        /// <summary>
        /// Creates a new pool of a given prefab
        /// </summary>
        /// <param name="prefab">The prefab to create the pool from</param>
        /// <param name="size">The size of the pool.</param>
        public void CreatePool(GameObject prefab, int size)
        {
            // Check if there already exists a pool for this prefab
            if (m_prefabLookupTable.ContainsKey(prefab))
            {
                Debug.LogError("There already exists a pool for prefab: " + prefab.name);
                return;
            }

            // Create the new pool and add to lookup
            Pool<GameObject> pool = new Pool<GameObject>(() => CreateNewInstance(prefab), size);
            m_prefabLookupTable[prefab] = pool;
        }

        /// <summary>
        /// Spawns a new object from a pool
        /// </summary>
        /// <param name="prefab">The prefab to spawn</param>
        /// <returns></returns>
        public GameObject SpawnObject(GameObject prefab)
        {
            return SpawnObject(prefab, Vector3.zero, Quaternion.identity);
        }

        /// <summary>
        /// Spawns a new object at the given position
        /// </summary>
        /// <param name="prefab">Prefab to spawn</param>
        /// <param name="position">The world position to spawn in</param>
        /// <param name="rotation">The rotation of the object</param>
        /// <returns></returns>
        public GameObject SpawnObject(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            // Make sure the pool exists. Create one if not
            if (!m_prefabLookupTable.ContainsKey(prefab))
            {
                CreatePool(prefab, 1);
            }

            // Find the pool
            Pool<GameObject> pool = m_prefabLookupTable[prefab];

            // Get next item from pool and set up its transform
            GameObject item = pool.GetItem();
            item.transform.SetPositionAndRotation(position, rotation);
            item.SetActive(true);

            // Add it to our instance lookup to track it
            m_prefabLookupTable.Add(item, pool);

            return item;
        }

        /// <summary>
        /// Removes a currently active instance. This should be used as a replacement to Destroy()
        /// </summary>
        /// <param name="instance">The instance to remove</param>
        public void RemoveObject(GameObject instance)
        {
            // Mark as not active
            instance.SetActive(false);
            
            // Release item if in pool
            if (m_instanceLookupTable.ContainsKey(instance))
            {
                m_instanceLookupTable[instance].ReleaseItem(instance);
                m_instanceLookupTable.Remove(instance);
            }
            
            // Throw debug if not found
            else
            {
                Debug.LogError("There is no pool for this instance: " + instance.name);
            }
        }

        /// <summary>
        /// Creates a new instance of the prefab
        /// </summary>
        /// <param name="prefab">The prefab to create</param>
        /// <returns></returns>
        private GameObject CreateNewInstance(GameObject prefab)
        {
            // Create the new object
            GameObject obj = Instantiate(prefab);

            // Assign root object if specified
            if (m_rootObject != null)
            {
                obj.transform.SetParent(m_rootObject);
            }

            // Disable and return
            obj.SetActive(false);
            return obj;
        }
    }
}