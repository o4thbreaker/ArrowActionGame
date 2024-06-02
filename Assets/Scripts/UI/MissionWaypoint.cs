using UnityEngine;
using UnityEngine.UI;

public class MissionWaypoint : MonoBehaviour
{
    [SerializeField] private Image waypointImage;
    [SerializeField] private Transform target;
    [SerializeField] private Transform playerCamera; // replace to make it modifiable

    private void Update()
    {
        float minX = waypointImage.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;

        float minY = waypointImage.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.height - minX;

        Vector2 position = Camera.main.WorldToScreenPoint(target.position);

        if (Vector3.Dot((target.position - playerCamera.position), playerCamera.forward) < 0)
        {
            // Target is behind the player
            if (position.x < Screen.width / 2)
            {
                position.x = maxX;
            }
            else
                position.x = minX;
        }

        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);

        waypointImage.transform.position = position;
    }
}
