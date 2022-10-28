using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Poolparty.Examples
{
    /// <summary>
    /// Removes itself after being alive for a set amount of time
    /// </summary>
    public class CubeScript : MonoBehaviour
    {
        public float m_lifeTime; // How long to stay alive

        private float m_timer;   // Timer for tracking life

        private void Start()
        {
            m_timer = m_lifeTime;
        }

        private void Update()
        {
            m_timer -= Time.deltaTime;

            if (m_timer <= 0.0f)
            {
                // Return to pool
                PoolManager.Instance.RemoveObject(gameObject);
            }
        }
    }
}
