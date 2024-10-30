using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Exploring,
        Dialogue,
        ResolviendoPuzzle1,
        Puzzle1Resuelto,
        ResolviendoPuzzle2,
        Puzzle2Resuelto,
    }

    public GameState currentState = GameState.Exploring;
    public int dialogueSequence = 0;

    // Referencias a las series de triggers
    public GameObject[] triggerSerie1;
    public GameObject[] triggerSerie2;

    public void ChangeState(GameState newState)
    {
        currentState = newState;
        Debug.Log("State changed to: " + newState.ToString());

        switch (newState)
        {
            case GameState.Puzzle1Resuelto:
                ActivateTriggers(triggerSerie2);
                DeactivateTriggers(triggerSerie1);
                ResetDialogueSequence(); // Restablecer la secuencia de diálogos
                break;

            case GameState.Puzzle2Resuelto:
                // Puedes agregar lógica para cambiar triggers cuando el puzzle 2 esté resuelto
                break;
        }
    }

    public void LlaveRecolectada(string llaveNombre)
    {
        if (llaveNombre == "Llave1")
        {
            ChangeState(GameState.Puzzle1Resuelto);
            Debug.Log("Has recolectado la Llave del Puzzle 1. Ahora puedes abrir la puerta al siguiente área.");
        }
        if (llaveNombre == "Llave2")
        {
            ChangeState(GameState.Puzzle2Resuelto);
            Debug.Log("Has recolectado la Llave del Puzzle 2. Ahora puedes abrir la puerta al siguiente área.");
        }
        // Agrega condiciones similares para otras llaves
    }

    public void AdvanceDialogueSequence()
    {
        dialogueSequence++;
    }

    private void ActivateTriggers(GameObject[] triggers)
    {
        foreach (GameObject trigger in triggers)
        {
            if (trigger != null)
            {
                trigger.SetActive(true);
            }
        }
    }

    private void DeactivateTriggers(GameObject[] triggers)
    {
        foreach (GameObject trigger in triggers)
        {
            if (trigger != null)
            {
                trigger.SetActive(false);
            }
        }
    }

    private void ResetDialogueSequence()
    {
        dialogueSequence = 0;
        Debug.Log("Dialogue sequence reset to 0.");
    }
}
