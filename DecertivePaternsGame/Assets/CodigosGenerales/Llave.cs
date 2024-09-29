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
            StartCoroutine(ActualizarLlavesEnBD());
            Destroy(gameObject);
        }
    }

    IEnumerator ActualizarLlavesEnBD()
    {
        // Usamos el nombre del usuario logueado en lugar de un valor fijo
        string[] datos = new string[1];
        datos[0] = login.nombreRollActual;  // Aquí utilizamos el nombre del usuario logueado

        StartCoroutine(servidor.ConsumirServicio("actualizar_llaves", datos, null));
        yield return new WaitUntil(() => !servidor.ocupado);
    }
}
