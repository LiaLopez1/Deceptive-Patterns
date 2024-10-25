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

    public void ChangeState(GameState newState)
    {
        currentState = newState;
        Debug.Log("State changed to: " + newState.ToString());
    }
    public void LlaveRecolectada(string Llave1nombre)
    {
        if (Llave1nombre == "Llave1")
        {
            ChangeState(GameState.Puzzle1Resuelto);
            Debug.Log("Has recolectado la Llave del Puzzle 1. Ahora puedes abrir la puerta al siguiente �rea.");
        }
        if (Llave1nombre == "Llave2")
        {
            ChangeState(GameState.Puzzle2Resuelto);
            Debug.Log("Has recolectado la Llave del Puzzle 1. Ahora puedes abrir la puerta al siguiente �rea.");
        }


        // Agrega condiciones similares para otras llaves

    }
    public void AdvanceDialogueSequence()
    {
        dialogueSequence++;
    }
}