using UnityEngine;

public class ActivateArrow : MonoBehaviour 
{
    [SerializeField] private Transform arrow;
    [SerializeField] private Transform player;

    public void TriggerArrow()
    {
        arrow.parent = null;
        //arrow.rotation = player.rotation;
        arrow.GetComponent<Rigidbody>().isKinematic = false;

        GameManager.Instance.UpdateState(GameManager.State.ControllingArrow);
    }
}
