using UnityEngine;

namespace Game_Content.Scripts.Chaos_Events
{
    public class ChaosEventHandler : MonoBehaviour
    {
        [Header("Event Acceptance Options")] 
        [Range(0f, 1f)]
        public float eventAcceptChance = 0.1f;
        [Tooltip("How much acceptance increases per second")]
        public float acceptanceIncrease = 0.01f;
        
        private void Update()
        {
            eventAcceptChance += acceptanceIncrease * Time.deltaTime;
        }
    }
}