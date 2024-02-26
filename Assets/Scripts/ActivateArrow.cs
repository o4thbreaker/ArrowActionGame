using UnityEngine;

public class ActivateArrow : MonoBehaviour
{
    [SerializeField] private Transform arrow;
    [SerializeField] private Transform player;

    public void TriggerArrow()
    {
        arrow.transform.parent = null;
        arrow.transform.rotation = player.transform.rotation;
        arrow.gameObject.SetActive(true);
        InputManager.EnableActionMap(InputManager.playerInput.Arrow);
        CameraSwitcher.Instance.SwitchCameraPriority();
        RewindManager.Instance.TrackingEnabled = true;
    }
}
