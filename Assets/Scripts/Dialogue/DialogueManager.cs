using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public GameObject continueButton;
    public GameObject Popup;
    public float typingSpeed = 2f;

    private Queue<Sentence> sentences;

    void Start()
    {
        sentences = new Queue<Sentence>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        sentences.Clear();
        continueButton.SetActive(true);

        foreach (Sentence sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            Popup.SetActive(false);
            return;
        }

        Sentence sentence = sentences.Dequeue();

        nameText.text = sentence.name;
        dialogueText.text = sentence.text;

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence.text));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1f / typingSpeed);
        }
    }

    void EndDialogue()
    {
        continueButton.SetActive(false);
    }
}