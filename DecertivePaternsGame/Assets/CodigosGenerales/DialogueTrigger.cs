using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    public GameManager gameManager;
    public float delayBeforeDialogue = 2.0f;
    public float delayBetweenDialogues = 1.0f;
    public float durationOfDialogue = 5.0f;
    public string[] dialogos; // Array de di�logos para este trigger
    public GameObject dialoguePanel; // Panel que contiene el texto del di�logo
    public Text dialogueText; // Componente de texto para mostrar el di�logo
    public int triggerSequence; // Secuencia necesaria para activar este trigger
    public GameManager.GameState nextState; // Estado a cambiar despu�s de este di�logo
    public bool changeState; // Indica si este trigger cambia el estado del juego

    private bool isDialogueActive = false; // Para evitar colisiones durante el di�logo

    private void OnTriggerEnter(Collider other)
    {
        // Verificamos si el di�logo est� activo para no activar m�ltiples di�logos simult�neamente
        if (other.CompareTag("Player") && gameManager.dialogueSequence == triggerSequence && !isDialogueActive)
        {
            StartCoroutine(HandleDialogue());
        }
    }

    private IEnumerator HandleDialogue()
    {
        isDialogueActive = true; // Marcamos que un di�logo est� en progreso

        // Retraso opcional antes de que comience el di�logo
        yield return new WaitForSeconds(delayBeforeDialogue);

        // Mostramos el panel de di�logo
        dialoguePanel.SetActive(true);

        // Recorremos todos los di�logos del array
        foreach (string dialogo in dialogos)
        {
            dialogueText.text = dialogo;
            yield return new WaitForSeconds(durationOfDialogue);

            // Retraso entre di�logos consecutivos (si est� configurado)
            yield return new WaitForSeconds(delayBetweenDialogues);
        }

        // Desactivamos el panel despu�s de que todos los di�logos hayan terminado
        dialoguePanel.SetActive(false);

        // Cambiamos de estado si es necesario
        if (changeState)
        {
            gameManager.ChangeState(nextState);
        }

        // Avanzamos en la secuencia de di�logos
        gameManager.AdvanceDialogueSequence();

        isDialogueActive = false; // Marcamos que el di�logo ha finalizado
    }
}
