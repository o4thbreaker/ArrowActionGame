using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float shootForce = 50f;

    private Rigidbody rb;
    private string targetTag = "TargetPoint";
    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * shootForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //TODO: change to GetComponent instead of tag
        if (collision.gameObject.tag == targetTag)
        {
            Debug.Log($"<color=red>Target got hit!</color>");
        }

        Destroy(gameObject);
    }
}
