using UnityEngine;

public class Cover : MonoBehaviour
{
    public bool isCoverOccupied = false;
    
    public void SetCoverOccupied()
    {
        isCoverOccupied = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(transform.position, Vector3.one * 0.3f);
    }
}
