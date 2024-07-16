using UnityEngine;
using System.Collections.Generic;

namespace SimpleWaypointIndicators
{
    [HelpURL("https://assetstore.unity.com/packages/slug/232484")]
    public class WaypointController : MonoBehaviour
    {
        public static Dictionary<EnemyPoint, Arrow> dictionary = new Dictionary<EnemyPoint, Arrow>();

        [SerializeField]
        private Transform playerPoint;

        private Camera cam;
        private float[] arrowRotationsZ = new float[] { 90f, -90f, 180f, 0f };

        /// <summary>
        /// Caching the component
        /// </summary>
        private void Start()
        {
            cam = Camera.main;
        }

        /// <summary>
        /// Displaying arrows at the edges of the screen
        /// </summary>
        private void LateUpdate()
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
            foreach (var element in dictionary)
            {
                Vector3 direction = element.Key.transform.position - playerPoint.position;
                Ray ray = new Ray(playerPoint.position, direction);
                float rayMinDistance = Mathf.Infinity;
                int index = 0;
                for (int i = 0; i < 4; i++)
                    if (planes[i].Raycast(ray, out float distance) && distance < rayMinDistance)
                    {
                        rayMinDistance = distance;
                        index = i;
                    }
                rayMinDistance = Mathf.Clamp(rayMinDistance, 0f, direction.magnitude);
                if (direction.magnitude > rayMinDistance)
                    element.Value.Show();
                else
                    element.Value.Hide();
                element.Value.transform.SetPositionAndRotation(cam.WorldToScreenPoint(ray.GetPoint(rayMinDistance)), Quaternion.Euler(0f, 0f, arrowRotationsZ[index]));
            }
        }
    }
}