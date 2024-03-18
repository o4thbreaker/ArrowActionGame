using Unity.VisualScripting;
using UnityEngine;

public class DebugDrawLine : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Debug.DrawLine(transform.position, transform.position + transform.forward * 50);
    }
}
