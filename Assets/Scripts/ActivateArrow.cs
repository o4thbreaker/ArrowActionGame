using UnityEngine;
using UnityEngine.Events;

public class ActivateArrow : MonoBehaviour
{
    [SerializeField] private Transform arrow;
    [SerializeField] private Transform player;

    public void TriggerArrow()
    {
        arrow.transform.parent = null;
        arrow.transform.rotation = player.transform.rotation;
        arrow.gameObject.SetActive(true);

        GameManager.Instance.UpdateState(GameManager.State.ControllingArrow);
        RewindManager.Instance.TrackingEnabled = true;
    }
}
