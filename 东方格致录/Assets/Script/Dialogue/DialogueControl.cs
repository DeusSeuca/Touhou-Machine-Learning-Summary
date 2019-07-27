using Command;
using UnityEngine;

public class DialogueControl : MonoBehaviour
{
    public GameObject DialogueUI;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!DialogueUI.activeSelf)
            {
                DialogueUI.SetActive(true);
                DialogueCommand.play(1, 1);
            }
            else
            {
                DialgueInfo.DialgueInfos.IsNext = true;
            }
        }
    }
}
