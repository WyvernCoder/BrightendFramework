using System;
using UnityEngine;

namespace Scenes.ChapterMenu
{
    public class CameraTouchMoving : MonoBehaviour
    {
        private Vector3 dragOrigin;
        private Vector3 camOrigin;
        private Vector3 targetCamPos;

        [SerializeField] private float smoothSpeed = 10f; // higher = snappier, lower = smoother

        private void Start()
        {
            targetCamPos = Camera.main.transform.position;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                dragOrigin = GetMouseOnGroundPlane();
                camOrigin = Camera.main.transform.position;
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 current = GetMouseOnGroundPlane();
                Vector3 delta = dragOrigin - current;

                targetCamPos = camOrigin + delta;
                targetCamPos.z = -10f;
            }

            // Smoothly move camera toward target
            Camera.main.transform.position = Vector3.Lerp(
                Camera.main.transform.position,
                targetCamPos,
                Time.deltaTime * smoothSpeed
            );
            
            
            
        }

        private Vector3 GetMouseOnGroundPlane()
        {
            Plane plane = new Plane(Vector3.forward, Vector3.zero); // z=0 plane
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out float enter))
                return ray.GetPoint(enter);
            return Vector3.zero;
        }

    }
}
