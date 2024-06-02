using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    [SerializeField] private string playerTag;
    [SerializeField] private Transform teleportTo;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == playerTag && !string.IsNullOrEmpty(playerTag))
        {
            other.GetComponent<CharacterController>().enabled = false;
            other.transform.position = teleportTo.position;
            other.GetComponent<CharacterController>().enabled = true;

            GameManager.Instance.UpdateState(GameManager.gameState.Tutorial);
        }
    }
}
