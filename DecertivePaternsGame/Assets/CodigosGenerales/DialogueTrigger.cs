using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    public float delayBeforeDialogue = 2.0f;
    public string dialogue = "Este es el diálogo que aparecerá.";
    public Text dialogueText;
    public GameObject dialoguePanel;
    public GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !gameManager.currentState.Equals(GameManager.GameState.ResolviendoPuzzle1))
        {
            StartCoroutine(ShowDialogueAfterDelay());
        }
    }

    private IEnumerator ShowDialogueAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeDialogue);
        dialogueText.text = dialogue;
        dialoguePanel.SetActive(true);
        yield return new WaitForSeconds(5); // Asumiendo que el diálogo necesita 5 segundos para leerse
        dialoguePanel.SetActive(false);
        gameManager.ChangeState(GameManager.GameState.ResolviendoPuzzle1);
        GetComponent<Collider>().enabled = false; // Desactiva el collider después de usar
    }
}
