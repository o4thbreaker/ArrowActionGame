using UnityEngine;

[CreateAssetMenu()]
public class EnemyConfigurationSO : ScriptableObject
{
    public float rotateSpeed = 10f;
    public float pathUpdateDelay = 0.2f;
    public float maxDetectionDistance = 5f;
}
