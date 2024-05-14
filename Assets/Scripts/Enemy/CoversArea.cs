using UnityEngine;

public class CoversArea : MonoBehaviour
{
    private Cover[] covers;

    private void Awake()
    {
        covers = GetComponentsInChildren<Cover>();
    }

    public Cover GetRandomCover()
    {
        return covers[Random.Range(0, covers.Length - 1)];
    }

    public Cover GetClosestCover(Vector3 agentLocation)
    {
        Cover cover = null;

        float minDistance = 50000f; // promote to variable to represent radius for searching cover

        for (int i = 0; i < covers.Length; i++)
        {
            float currentDistance = Vector3.Distance(agentLocation, covers[i].transform.position);

            if (currentDistance < minDistance && !covers[i].isCoverOccupied)
            {
                minDistance = currentDistance;
                cover = covers[i];
            }
        }

        return cover;
    }
}
