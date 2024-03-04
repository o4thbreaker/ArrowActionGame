using System;
using UnityEngine;
using UnityEngine.UI;

public class ArrowCooldownTimer : MonoBehaviour
{
    [SerializeField] private Image cooldownMeter;
    [SerializeField] private float cooldownTime = 10f;

    private float currentCooldownTime;
    public bool IsCooldown { get; private set; }

    private void Start()
    { 
        cooldownMeter.fillAmount = 0;
        IsCooldown = true;
    }

    public void ActivateCooldown()
    {
        IsCooldown = true;
    }

    public void ResetCooldown()
    {
        IsCooldown = false;
        currentCooldownTime = cooldownTime;

        cooldownMeter.fillAmount = 0;
    }

    private void Update()
    {
        if (IsCooldown)
        {
            cooldownMeter.fillAmount += 1 / currentCooldownTime * Time.deltaTime;

            if (cooldownMeter.fillAmount >= 1)
                IsCooldown = false;
        }
    }
}
