using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    public GameManager gameManager;
    public float delayBeforeDialogue = 2.0f;
    public float delayBetweenDialogues = 1.0f;
    public float durationOfDialogue = 5.0f;

    [System.Serializable]
    public struct DialogueEntry
    {
        public string texto; // Texto principal del diálogo
        public bool mostrarMision; // Indica si se debe mostrar el texto de misión
        public string textoMision; // Texto de misión específico para este diálogo
    }

    public DialogueEntry[] dialogos; // Array de diálogos con información adicional
    public GameObject dialoguePanel; // Panel que contiene el texto del diálogo
    public Text dialogueText; // Componente de texto para mostrar el diálogo principal
    public Text missionText; // Componente de texto opcional para la misión

    public int triggerSequence; // Secuencia necesaria para activar este trigger
    public GameManager.GameState nextState; // Estado a cambiar después de este diálogo
    public bool changeState; // Indica si este trigger cambia el estado del juego

    private bool isDialogueActive = false; // Para evitar colisiones durante el diálogo
    private int dialoguesShownCount = 0; // Contador de diálogos mostrados

    // Variables para la verificación de triggers completados
    private static int completedTriggers = 0; // Contador global de triggers completados
    private const int totalTriggersToComplete = 14; // Total de triggers necesarios para cambiar de estado

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
        foreach (DialogueEntry dialogo in dialogos)
        {
            dialogueText.text = dialogo.texto;
            dialoguesShownCount++; // Aumentamos el contador de diálogos mostrados

            // Controlamos si mostramos o actualizamos el texto de misión
            if (dialogo.mostrarMision)
            {
                missionText.text = dialogo.textoMision;
                missionText.gameObject.SetActive(true); // Aseguramos que el texto de misión esté visible
            }

            // Mantenemos el diálogo principal visible por el tiempo especificado
            yield return new WaitForSeconds(durationOfDialogue);

            // Retraso entre diálogos consecutivos (si está configurado)
            yield return new WaitForSeconds(delayBetweenDialogues);
        }

        // Desactivamos el panel de diálogo después de que todos los diálogos hayan terminado
        dialoguePanel.SetActive(false);

        // Condicional para mantener o desactivar el texto de misión según el último diálogo
        DialogueEntry ultimoDialogo = dialogos[dialogos.Length - 1];
        if (!ultimoDialogo.mostrarMision)
        {
            missionText.gameObject.SetActive(false); // Solo ocultamos si el último diálogo no necesita mostrar misión
        }

        // Marcamos este trigger como completado
        completedTriggers++;

        // Avanzamos en la secuencia de diálogos
        gameManager.AdvanceDialogueSequence();

        // Verificamos si los diálogos se reinician (es decir, dialoguesShownCount se va a poner en 0)
        if (dialoguesShownCount > 0)
        {
            if (completedTriggers >= totalTriggersToComplete)
            {
                Debug.Log("Todos los diálogos de los 14 triggers fueron activados antes de reiniciar.");
            }
            else
            {
                int triggersRestantes = totalTriggersToComplete - completedTriggers;
                Debug.LogWarning($"Reinicio de diálogos realizado faltando {triggersRestantes} triggers por activar.");
            }
        }

        // Reiniciamos el contador de diálogos para la próxima secuencia
        dialoguesShownCount = 0;
        isDialogueActive = false; // Marcamos que el diálogo ha finalizado
    }
}