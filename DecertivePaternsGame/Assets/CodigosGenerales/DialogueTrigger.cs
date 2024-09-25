using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueTrigger : MonoBehaviour
{
    public GameManager gameManager;
    public float delayBeforeDialogue = 2.0f;
    public float durationOfDialogue = 5.0f; // Duración en segundos que el diálogo es visible
    public string dialogos; // Array de diálogos para este trigger
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
        dialogueText.text = dialogos; // Asegúrate de que este índice exista en el array /este por si le colocamos [] [gameManager.dialogueSequence]
        dialoguePanel.SetActive(true);
        yield return new WaitForSeconds(durationOfDialogue);
        dialoguePanel.SetActive(false);

        if (changeState)
        {
            gameManager.ChangeState(nextState);
        }
        gameManager.AdvanceDialogueSequence();
    }
}
