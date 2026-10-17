using UnityEngine;

// Lives on the medal view. Tells the player when the medals are in order.
public class MedalPuzzle : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public Dialogue solvedDialogue;

    public void Solved()
    {
        Debug.Log("Medals solved!");

        if (dialogueManager == null)
            return;

        dialogueManager.Popup.SetActive(true);
        dialogueManager.StartDialogue(solvedDialogue);
    }
}
