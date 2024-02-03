using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HitBox : MonoBehaviour
{

    public event EventHandler OnCollideWithArrow;
    public CapsuleCollider arrow;
    public static event Action OnHeadshot;
    public static event Action OnKill;
    private bool isKIlled;
    private bool isKilled;


    private void OnTriggerExit(Collider other)
    {
        if (other == arrow)
        {
            OnCollideWithArrow?.Invoke(this, EventArgs.Empty);
            arrow.isTrigger = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInChildren<CapsuleCollider>() == arrow)
        {
            arrow.isTrigger = true;
            if (!isKilled)
            {
                OnKill?.Invoke();
                //isKilled = true;
            }
            
            Debug.Log(gameObject.tag);
            Debug.Log(gameObject);
            if (gameObject.name == "mixamorig:Head" && !isKIlled)
            {
                Debug.Log("Headshot");
                OnHeadshot?.Invoke();
                isKIlled = true;
            }
        }
        else
        {
            arrow.isTrigger = false;
        }
    }

   /* private IEnumerator Kill()
    {
        yield return WaitForSeconds(1f);
       
    }*/

    

}
