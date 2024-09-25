using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueTrigger : MonoBehaviour
{
    public GameManager gameManager;
    public float delayBeforeDialogue = 2.0f;
    public float durationOfDialogue = 5.0f; // Duraci�n en segundos que el di�logo es visible
    public string dialogos; // Array de di�logos para este trigger
    public GameObject dialoguePanel; // Panel que contiene el texto del di�logo
    public Text dialogueText; // Componente de texto para mostrar el di�logo
    public int triggerSequence; // Secuencia necesaria para activar este trigger
    public GameManager.GameState nextState; // Estado a cambiar despu�s de este di�logo
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
        dialogueText.text = dialogos; // Aseg�rate de que este �ndice exista en el array /este por si le colocamos [] [gameManager.dialogueSequence]
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
