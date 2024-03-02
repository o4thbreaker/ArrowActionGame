using UnityEngine;

public class ArrowPathRepeater : MonoBehaviour
{

    private void Start()
    {
        GameManager.Instance.OnArrowPathRepeated += ShowMessage;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnArrowPathRepeated -= ShowMessage;
    }

    private void ShowMessage()
    {
        Debug.Log("Hello from ArrowPathRepeater");

        GameManager.Instance.UpdateState(GameManager.State.ControllingCharacter);
    }
}
