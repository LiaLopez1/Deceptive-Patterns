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
        public string texto; // Texto principal del di�logo
        public bool mostrarMision; // Indica si se debe mostrar el texto de misi�n
        public string textoMision; // Texto de misi�n espec�fico para este di�logo
    }

    public DialogueEntry[] dialogos; // Array de di�logos con informaci�n adicional
    public GameObject dialoguePanel; // Panel que contiene el texto del di�logo
    public Text dialogueText; // Componente de texto para mostrar el di�logo principal
    public Text missionText; // Componente de texto opcional para la misi�n

    public int triggerSequence; // Secuencia necesaria para activar este trigger
    public GameManager.GameState nextState; // Estado a cambiar despu�s de este di�logo
    public bool changeState; // Indica si este trigger cambia el estado del juego

    private bool isDialogueActive = false; // Para evitar colisiones durante el di�logo
    private int dialoguesShownCount = 0; // Contador de di�logos mostrados

    // Variables para la verificaci�n de triggers completados
    private static int completedTriggers = 0; // Contador global de triggers completados
    private const int totalTriggersToComplete = 14; // Total de triggers necesarios para cambiar de estado

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
        foreach (DialogueEntry dialogo in dialogos)
        {
            dialogueText.text = dialogo.texto;
            dialoguesShownCount++; // Aumentamos el contador de di�logos mostrados

            // Controlamos si mostramos o actualizamos el texto de misi�n
            if (dialogo.mostrarMision)
            {
                missionText.text = dialogo.textoMision;
                missionText.gameObject.SetActive(true); // Aseguramos que el texto de misi�n est� visible
            }

            // Mantenemos el di�logo principal visible por el tiempo especificado
            yield return new WaitForSeconds(durationOfDialogue);

            // Retraso entre di�logos consecutivos (si est� configurado)
            yield return new WaitForSeconds(delayBetweenDialogues);
        }

        // Desactivamos el panel de di�logo despu�s de que todos los di�logos hayan terminado
        dialoguePanel.SetActive(false);

        // Condicional para mantener o desactivar el texto de misi�n seg�n el �ltimo di�logo
        DialogueEntry ultimoDialogo = dialogos[dialogos.Length - 1];
        if (!ultimoDialogo.mostrarMision)
        {
            missionText.gameObject.SetActive(false); // Solo ocultamos si el �ltimo di�logo no necesita mostrar misi�n
        }

        // Marcamos este trigger como completado
        completedTriggers++;

        // Avanzamos en la secuencia de di�logos
        gameManager.AdvanceDialogueSequence();

        // Verificamos si los di�logos se reinician (es decir, dialoguesShownCount se va a poner en 0)
        if (dialoguesShownCount > 0)
        {
            if (completedTriggers >= totalTriggersToComplete)
            {
                Debug.Log("Todos los di�logos de los 14 triggers fueron activados antes de reiniciar.");
            }
            else
            {
                int triggersRestantes = totalTriggersToComplete - completedTriggers;
                Debug.LogWarning($"Reinicio de di�logos realizado faltando {triggersRestantes} triggers por activar.");
            }
        }

        // Reiniciamos el contador de di�logos para la pr�xima secuencia
        dialoguesShownCount = 0;
        isDialogueActive = false; // Marcamos que el di�logo ha finalizado
    }
}