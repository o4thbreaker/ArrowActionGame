using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Waypoints : MonoBehaviour
{

    [Range(0f, 2f)] [SerializeField] private float waypointSize = 1f;
    [SerializeField] private bool canLoop = false;
    public bool isLastWaypointReached = false;

    private void OnDrawGizmos()
    {
        foreach (Transform t in transform)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(t.position, waypointSize);
            //Handles.Label(t.position, t.GetSiblingIndex().ToString());
        }

        Gizmos.color = Color.red;
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
        }

        if (canLoop)
        {
            Gizmos.DrawLine(transform.GetChild(transform.childCount - 1).position, transform.GetChild(0).position);
        }
    }

    public Transform GetNextWaypoint(Transform currentWaypoint)
    {
        if (currentWaypoint == null)
        {
            return transform.GetChild(0);
        }

        if (currentWaypoint.GetSiblingIndex() < transform.childCount - 1)
        {
            return transform.GetChild(currentWaypoint.GetSiblingIndex() + 1);
        }
        else // here is the last waypoint
        {
            if (canLoop)
            {
                isLastWaypointReached = false;
                return transform.GetChild(0);
            }
            else
            {
                isLastWaypointReached = true;
                return transform.GetChild(currentWaypoint.GetSiblingIndex());
            }
        }
    }
}
