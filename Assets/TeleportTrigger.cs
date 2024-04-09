using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    [SerializeField] private string playerTag;
    [SerializeField] private Transform teleportTo;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == playerTag && !string.IsNullOrEmpty(playerTag))
        {
            other.transform.position = teleportTo.position;

            GameManager.Instance.UpdateState(GameManager.gameState.LevelEntered);
        }
    }
}
