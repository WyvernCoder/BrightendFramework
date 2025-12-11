using System;
using UnityEngine;

namespace Scenes.ChapterMenu
{
    /// <summary>
    /// Make the children arrangement like sin math function. 
    /// </summary>
    public class SineArrangement : MonoBehaviour
    {
    public enum ArrangementDirection
    {
        PositiveX,
        NegativeX,
        PositiveY,
        NegativeY
    }

    public ArrangementDirection direction = ArrangementDirection.PositiveX;

    [Header("Sine Settings")]
    public float deltaDistance = 1f;   // spacing along main axis
    public float amplitude = 1f;       // sine wave height
    public float frequency = 1f;       // sine wave frequency

    [ContextMenu("Arrange Children")]
    public void DoArrangement()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            float t = i * deltaDistance;
            Vector3 pos = Vector3.zero;

            switch (direction)
            {
                case ArrangementDirection.PositiveX:
                    pos = new Vector3(t, Mathf.Sin(t * frequency) * amplitude, 0f);
                    break;

                case ArrangementDirection.NegativeX:
                    pos = new Vector3(-t, Mathf.Sin(-t * frequency) * amplitude, 0f);
                    break;

                case ArrangementDirection.PositiveY:
                    pos = new Vector3(Mathf.Sin(t * frequency) * amplitude, t, 0f);
                    break;

                case ArrangementDirection.NegativeY:
                    pos = new Vector3(Mathf.Sin(-t * frequency) * amplitude, -t, 0f);
                    break;
            }

            child.localPosition = pos;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 prevPoint = transform.position;
        //int resolution = Mathf.Max(transform.childCount, 50);

        for (int i = 0; i <= 200; i++)
        {
            float t = i * deltaDistance;
            Vector3 point = Vector3.zero;

            switch (direction)
            {
                case ArrangementDirection.PositiveX:
                    point = transform.position + new Vector3(t, Mathf.Sin(t * frequency) * amplitude, 0f);
                    break;

                case ArrangementDirection.NegativeX:
                    point = transform.position + new Vector3(-t, Mathf.Sin(-t * frequency) * amplitude, 0f);
                    break;

                case ArrangementDirection.PositiveY:
                    point = transform.position + new Vector3(Mathf.Sin(t * frequency) * amplitude, t, 0f);
                    break;

                case ArrangementDirection.NegativeY:
                    point = transform.position + new Vector3(Mathf.Sin(-t * frequency) * amplitude, -t, 0f);
                    break;
            }

            Gizmos.DrawSphere(point, 0.05f);
            Gizmos.DrawLine(prevPoint, point);
            prevPoint = point;
        }
    }
    }
}