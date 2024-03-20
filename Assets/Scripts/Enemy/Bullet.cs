using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float shootForce = 50f;
    private void OnEnable()
    {
        /*rb = GetComponent<Rigidbody>();

        GameObject target = GameObject.FindGameObjectWithTag("TargetPoint");
        Vector3 direction = (target.transform.position - transform.position).normalized;

        rb.AddForce(direction * shootForce, ForceMode.Impulse);*/
    }
}
