using UnityEngine;

namespace SimpleWaypointIndicators
{
    [HelpURL("https://assetstore.unity.com/packages/slug/232484")]
    public class Arrow : MonoBehaviour
    {
        [HideInInspector]
        public bool isDestroy;

        [SerializeField]
        private float animSpeed;

        private float t;
        private bool isShown, isPlayAnim;

        /// <summary>
        /// Show arrow
        /// </summary>
        public void Show()
        {
            if (isShown)
                return;
            isShown = true;
            t = 0f;
            isPlayAnim = true;
        }

        /// <summary>
        /// Hide the arrow
        /// </summary>
        public void Hide()
        {
            if (!isShown)
                return;
            isShown = false;
            t = 1f;
            isPlayAnim = true;
        }

        /// <summary>
        /// Paint the arrow in a certain color
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = color;
        }

        /// <summary>
        /// Reset parameters
        /// </summary>
        private void Start()
        {
            transform.localScale = Vector3.zero;
        }

        /// <summary>
        /// Playing animations
        /// </summary>
        private void Update()
        {
            if (!isPlayAnim)
                return;
            t += animSpeed * (isShown ? 1f : -1f) * Time.deltaTime;
            if (t < 0f || t > 1f)
            {
                if (isDestroy)
                    Destroy(gameObject);
                transform.localScale = Vector3.one * Mathf.Clamp(t, 0f, 1f);       
                isPlayAnim = false;
            }
            else
                transform.localScale = Vector3.one * t;
        }
    }
}