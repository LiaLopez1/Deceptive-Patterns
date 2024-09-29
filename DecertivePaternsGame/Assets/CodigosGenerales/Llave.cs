using System.Collections;
using UnityEngine;

public class Llave : MonoBehaviour
{
    public Puerta doorToUnlock;
    public GameManager gameManager;
    public Servidor servidor;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorToUnlock.UnlockDoor();
            gameManager.LlaveRecolectada(gameObject.name);

            // Actualiza el contador de llaves
            GameObject.FindObjectOfType<ContadorLlaves>().RecolectarLlave();

            // Actualiza las llaves en la base de datos
            StartCoroutine(ActualizarLlavesEnBD());

            Destroy(gameObject);
        }
    }

    IEnumerator ActualizarLlavesEnBD()
    {
        string[] datos = new string[1];
        datos[0] = login.nombreRollActual;  // Utilizar el nombre del usuario logueado

        // Consumir el servicio "actualizar_llaves"
        StartCoroutine(servidor.ConsumirServicio("actualizar_llaves", datos, null));
        yield return new WaitUntil(() => !servidor.ocupado);
    }
}
