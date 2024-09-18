using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Llave : MonoBehaviour
{
    public Puerta doorToUnlock; // Referencia a la puerta que la llave desbloqueará
    public GameManager gameManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Asegúrate de que tu jugador tenga el tag "Player"
        {
            doorToUnlock.UnlockDoor(); // Desbloquea la puerta asignada
            gameManager.LlaveRecolectada(gameObject.name); // Cambia el estado del juego según la llave
            Destroy(gameObject); // Opcional: Destruye la llave después de usarla
        }
    }
}
