using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float shootForce = 50f;
    [SerializeField] private string targetTag = "Player";

    private Rigidbody rb;
   
    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * shootForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<ThirdPersonController>() != null)
        {
            Debug.Log($"<color=orange>Player got hit!</color>");
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
        }

        Destroy(gameObject);
    }
}
