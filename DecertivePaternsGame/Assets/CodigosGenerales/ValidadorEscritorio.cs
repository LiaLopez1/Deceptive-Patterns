using UnityEngine;

public class Chest : MonoBehaviour
{
    public Canvas interactionHintCanvas;   // Canvas que muestra "Presiona E para interactuar"
    public GameObject player;              // Referencia al jugador
    public int correctKeyID;               // ID de la llave que abre el cofre

    private bool playerInRange = false;    // Para verificar si el jugador est� dentro del rango de interacci�n

    private void Start()
    {
        // Desactivar el canvas al inicio
        interactionHintCanvas.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionHintCanvas.gameObject.SetActive(true);
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionHintCanvas.gameObject.SetActive(false);
            playerInRange = false;
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TryOpenChest();
        }
    }

    private void TryOpenChest()
    {
        PlayerInventory playerInventory = player.GetComponent<PlayerInventory>();
        if (playerInventory != null && playerInventory.GetKey() == correctKeyID)
        {
            Debug.Log("El cofre se ha abierto con �xito.");
            // Aqu� puedes a�adir l�gica para abrir el cofre (animaci�n, recompensas, etc.)
            playerInventory.RemoveKey(); // Remover la llave despu�s de abrir el cofre
        }
        else
        {
            Debug.Log("No tienes la llave correcta para abrir el cofre.");
        }
    }
}
