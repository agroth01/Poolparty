using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Poolparty.Examples
{
    /// <summary>
    /// Continiously spawn cubes utilizing pool for performance.
    /// </summary>
    public class CubeSpawner : MonoBehaviour
    {
        public GameObject m_cubePrefab; // The prefab which to spawn
        public float m_spawnDelay;      // How long to wait between each spawn
        public int m_poolSize;          // Initial size of pool

        private float m_timer;          // Timer for tracking spawns

        private void Start()
        {
            // Create a pool for the cube
            PoolManager.Instance.CreatePool(m_cubePrefab, m_poolSize);
        }

        private void Update()
        {
            if (m_timer <= 0.0f)
            {
                // Reset timer
                m_timer = m_spawnDelay;

                // Spawn new object
                PoolManager.Instance.SpawnObject(m_cubePrefab, transform.position, transform.rotation);
            }
        }
    }
}