using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody rb;
    private const string targetPoint = "TargetPoint";
    private const string player = "PlayerCapsule";

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Transform target = GameObject.FindGameObjectWithTag(targetPoint).transform;
        Vector3 direction = target.position - transform.position;
        rb.AddForce(direction * speed * 10f * Time.deltaTime, ForceMode.Impulse);
    }


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.CompareTag(player))
        {
            //kill player
            Debug.Log("Player killed");
            //SceneManager.LoadScene("GameOver");           
        }
        Destroy(gameObject);
    }
}
