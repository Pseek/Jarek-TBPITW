using UnityEngine;

namespace SimpleWaypointIndicators
{
    [HelpURL("https://assetstore.unity.com/packages/slug/232484")]
    public class EnemyPoint : MonoBehaviour
    {
        [SerializeField]
        private Arrow arrowPrefab;

        [SerializeField]
        private Color arrowColor;

        private Arrow arrow;

        /// <summary>
        /// Creating an Arrow instance
        /// </summary>
        private void Awake()
        {
            arrow = Instantiate(arrowPrefab, FindObjectOfType<WaypointController>().transform);
            arrow.SetColor(arrowColor);
        }

        /// <summary>
        /// Adding an arrow to the dictionary
        /// </summary>
        private void OnEnable()
        {
            WaypointController.dictionary.Add(this, arrow);
        }

        /// <summary>
        /// Removing the arrow in the dictionary
        /// </summary>
        private void OnDisable()
        {
            WaypointController.dictionary.Remove(this);
            arrow.Hide();
        }

        /// <summary>
        /// Destroying the arrow after playing the animation
        /// </summary>
        private void OnDestroy()
        {
            arrow.isDestroy = true;
        }
    }
}