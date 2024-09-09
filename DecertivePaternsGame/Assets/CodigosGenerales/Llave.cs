using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Llave : MonoBehaviour
{
    public Puerta doorToUnlock; // Referencia a la puerta que la llave desbloquear�

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Aseg�rate de que tu jugador tenga el tag "Player"
        {
            doorToUnlock.UnlockDoor(); // Desbloquea la puerta asignada
            Destroy(gameObject); // Opcional: Destruye la llave despu�s de usarla
        }
    }
}
