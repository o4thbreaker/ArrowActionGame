using System.Collections;
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

        StartCoroutine(LaunchArrowCoroutine());

        //Debug.Log("Updating state....");
        //GameManager.Instance.UpdateState(GameManager.State.ControllingArrow);
    }

    private IEnumerator LaunchArrowCoroutine()
    {
        // TODO: take a better look at coroutines and how to use them
        arrow.GetComponent<Rigidbody>().AddForce(arrow.transform.forward * 20f, ForceMode.VelocityChange);

        yield return new WaitForSeconds(0.5f);

        PlayerStateManager.Instance.UpdateState(PlayerStateManager.playerState.ControllingArrow);
    }
}
