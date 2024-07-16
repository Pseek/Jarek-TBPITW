using UnityEngine;
using System.Collections;

namespace SimpleWaypointIndicators.Demo
{
    [HelpURL("https://assetstore.unity.com/packages/slug/232484")]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private SceneType sceneType;

        [SerializeField]
        private float moveSpeed, rotationSpeed, acceleration, smoothnessSpeed, minPosY;

        private Vector3 targetPos;

        private enum SceneType { _2D, _3D }

        /// <summary>
        /// Initializing the controller
        /// </summary>
        /// <returns></returns>
        private IEnumerator Start()
        {
            if (sceneType == SceneType._3D)
                Cursor.lockState = CursorLockMode.Locked;
            targetPos = transform.position;
            yield return null;
            transform.eulerAngles = Vector3.zero;
        }

        /// <summary>
        /// Camera free flight control
        /// </summary>
        private void Update()
        {
            targetPos += ((sceneType == SceneType._3D ? transform.forward : (transform.up * 0.5f)) * (Input.GetKey(KeyCode.W) ? 1f : (Input.GetKey(KeyCode.S) ? -1f : 0f)) +
                          transform.right * (Input.GetKey(KeyCode.D) ? 1f : (Input.GetKey(KeyCode.A) ? -1f : 0f)) +
                          (sceneType == SceneType._3D ? transform.up : Vector3.zero) * 0.5f * (Input.GetKey(KeyCode.Q) ? 1f : (Input.GetKey(KeyCode.E) ? -1f : 0f))) * moveSpeed * (Input.GetKey(KeyCode.LeftShift) ? acceleration : 1f) * Time.deltaTime;
            if (targetPos.y < minPosY)
                targetPos = new Vector3(targetPos.x, minPosY, targetPos.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, smoothnessSpeed * Time.deltaTime);
            if (sceneType == SceneType._3D)
                transform.eulerAngles += new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0f) * rotationSpeed;
        }
    }
}