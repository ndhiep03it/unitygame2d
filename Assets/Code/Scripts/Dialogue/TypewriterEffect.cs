using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TypewriterEffect : MonoBehaviour
{
    public Text dialogueText; // Reference to the UI Text element
    public string[] dialogueLines; // Array of dialogue lines
    public float typeSpeed = 0.05f; // Speed of the typing effect

    private int currentLineIndex = 0;
    private bool isTyping = false;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartDialogue();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isTyping)
        {
            DisplayNextLine();
        }
    }

    public void StartDialogue()
    {
        currentLineIndex = 0;
        DisplayNextLine();
    }

    void DisplayNextLine()
    {
        if (currentLineIndex < dialogueLines.Length)
        {
            StartCoroutine(TypeLine(dialogueLines[currentLineIndex]));
            currentLineIndex++;
            audioSource.Play();
        }
        else
        {
            dialogueText.text = ""; // Clear text when dialogue ends
        }
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typeSpeed);
        }
        isTyping = false;
        audioSource.Stop();

    }
}
