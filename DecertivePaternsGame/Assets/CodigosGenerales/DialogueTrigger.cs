using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    public GameManager gameManager;
    public float delayBeforeDialogue = 2.0f;
    public float delayBetweenDialogues = 1.0f;
    public float durationOfDialogue = 5.0f;
    public string[] dialogos; // Array de diálogos para este trigger
    public GameObject dialoguePanel; // Panel que contiene el texto del diálogo
    public Text dialogueText; // Componente de texto para mostrar el diálogo
    public int triggerSequence; // Secuencia necesaria para activar este trigger
    public GameManager.GameState nextState; // Estado a cambiar después de este diálogo
    public bool changeState; // Indica si este trigger cambia el estado del juego

    private bool isDialogueActive = false; // Para evitar colisiones durante el diálogo

    private void OnTriggerEnter(Collider other)
    {
        // Verificamos si el diálogo está activo para no activar múltiples diálogos simultáneamente
        if (other.CompareTag("Player") && gameManager.dialogueSequence == triggerSequence && !isDialogueActive)
        {
            StartCoroutine(HandleDialogue());
        }
    }

    private IEnumerator HandleDialogue()
    {
        isDialogueActive = true; // Marcamos que un diálogo está en progreso

        // Retraso opcional antes de que comience el diálogo
        yield return new WaitForSeconds(delayBeforeDialogue);

        // Mostramos el panel de diálogo
        dialoguePanel.SetActive(true);

        // Recorremos todos los diálogos del array
        foreach (string dialogo in dialogos)
        {
            dialogueText.text = dialogo;
            yield return new WaitForSeconds(durationOfDialogue);

            // Retraso entre diálogos consecutivos (si está configurado)
            yield return new WaitForSeconds(delayBetweenDialogues);
        }

        // Desactivamos el panel después de que todos los diálogos hayan terminado
        dialoguePanel.SetActive(false);

        // Cambiamos de estado si es necesario
        if (changeState)
        {
            gameManager.ChangeState(nextState);
        }

        // Avanzamos en la secuencia de diálogos
        gameManager.AdvanceDialogueSequence();

        isDialogueActive = false; // Marcamos que el diálogo ha finalizado
    }
}
