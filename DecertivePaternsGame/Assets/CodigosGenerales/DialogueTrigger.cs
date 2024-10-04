using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueTrigger : MonoBehaviour
{
    public GameManager gameManager;
    public float delayBeforeDialogue = 2.0f;
    public float delayBetweenDialogues = 1.0f; // Retraso entre diálogos consecutivos
    public float durationOfDialogue = 5.0f; // Duración en segundos que el diálogo es visible
    public string[] dialogos; // Array de diálogos para este trigger
    public GameObject dialoguePanel; // Panel que contiene el texto del diálogo
    public Text dialogueText; // Componente de texto para mostrar el diálogo
    public int triggerSequence; // Secuencia necesaria para activar este trigger
    public GameManager.GameState nextState; // Estado a cambiar después de este diálogo
    public bool changeState; // Indica si este trigger cambia el estado del juego

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameManager.dialogueSequence == triggerSequence)
        {
            StartCoroutine(HandleDialogue());
        }
    }

    private IEnumerator HandleDialogue()
    {
        yield return new WaitForSeconds(delayBeforeDialogue);

        dialoguePanel.SetActive(true);
        foreach (string dialogo in dialogos)
        {
            dialogueText.text = dialogo;
            yield return new WaitForSeconds(durationOfDialogue);

            // Si quieres un retraso entre diálogos (opcional)
            yield return new WaitForSeconds(delayBetweenDialogues);
        }
        dialoguePanel.SetActive(false);

        if (changeState)
        {
            gameManager.ChangeState(nextState);
        }
        gameManager.AdvanceDialogueSequence();
    }
}
