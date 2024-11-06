using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SumaValores : MonoBehaviour
{
    public Text totalText;  // Texto del UI donde se mostrará la suma total de los precios
    public GameObject warningPanel; // Panel para mostrar el mensaje de advertencia
    public Text warningText; // Texto dentro del panel de advertencia
    public string mensajeAdvertencia = "¡Un objeto de sabotaje ha salido del área!"; // Mensaje personalizable
    public float mensajeDuracion = 3f; // Duración del mensaje de advertencia en segundos

    private int totalSum = 0;
    public GameObject objetoSabotajePrefab; // Prefab del objeto que se añadirá automáticamente
    public List<Transform> spawnPoints; // Lista de puntos donde se generarán los objetos de sabotaje
    public float tiempoEntreSabotajes = 5f; // Tiempo entre la aparición de nuevos objetos de sabotaje

    private int objetosGenerados = 0;
    private int maxObjetosSabotaje = 2; // Máximo número de objetos de sabotaje a generar
    private bool primerObjetoColocado = false;

    private void Start()
    {
        if (totalText != null)
        {
            totalText.text = "Total: $0";
        }

        if (warningPanel != null)
        {
            warningPanel.SetActive(false); // Inicialmente ocultamos el panel de advertencia
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto tiene el script 'ObjetosMercado'
        ObjetosMercado item = other.GetComponent<ObjetosMercado>();
        if (item != null)
        {
            // Sumamos el precio del objeto
            totalSum += item.precio;
            // Actualizamos el texto en el UI
            UpdateTotalText();

            // Añadir objetos de sabotaje cuando se activa el trigger por primera vez
            if (!primerObjetoColocado)
            {
                primerObjetoColocado = true;
                StartCoroutine(AgregarObjetosSabotajeConRetraso());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Verificar si el objeto tiene el script 'ObjetosMercado'
        ObjetosMercado item = other.GetComponent<ObjetosMercado>();
        if (item != null)
        {
            // Restar el precio del objeto cuando se retira del trigger
            totalSum -= item.precio;
            // Actualizamos el texto en el UI
            UpdateTotalText();
        }

        // Verificar si el objeto es un objeto de sabotaje al salir del Trigger
        if (other.CompareTag("objeto"))
        {
            // Mostrar el mensaje de advertencia con retraso
            StartCoroutine(ShowWarningMessageWithDelay(1f)); // 1 segundo de retraso antes de mostrar el mensaje

            // Opcional: borrar el mensaje después de unos segundos
            StartCoroutine(ClearWarningMessageAfterDelay(mensajeDuracion + 1f)); // Agregar el retraso al tiempo total
        }
    }

    private void UpdateTotalText()
    {
        if (totalText != null)
        {
            totalText.text = "Total: $" + totalSum.ToString();
        }
    }

    private IEnumerator AgregarObjetosSabotajeConRetraso()
    {
        while (objetosGenerados < maxObjetosSabotaje)
        {
            yield return new WaitForSeconds(tiempoEntreSabotajes);

            // Instanciar el objeto de sabotaje en un punto de spawn según el número de objetos generados
            if (objetoSabotajePrefab != null && spawnPoints.Count > objetosGenerados)
            {
                Transform spawnPoint = spawnPoints[objetosGenerados];
                GameObject nuevoObjeto = Instantiate(objetoSabotajePrefab, spawnPoint.position, spawnPoint.rotation);
                nuevoObjeto.tag = "objeto"; // Asegurarse de que el objeto tenga el tag correcto

                // Copiar todas las propiedades del prefab
                ObjetosMercado nuevoItem = nuevoObjeto.GetComponent<ObjetosMercado>();
                if (nuevoItem != null)
                {
                    nuevoItem.precio = objetoSabotajePrefab.GetComponent<ObjetosMercado>().precio;
                }

                // Asegurarse de que el objeto tenga un Rigidbody para ser interactuado
                if (nuevoObjeto.GetComponent<Rigidbody>() == null)
                {
                    nuevoObjeto.AddComponent<Rigidbody>();
                }

                objetosGenerados++;
            }
        }
    }

    private IEnumerator ShowWarningMessageWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Espera antes de mostrar el mensaje

        if (warningPanel != null && warningText != null)
        {
            warningText.text = mensajeAdvertencia; // Mostrar el mensaje personalizado
            warningPanel.SetActive(true); // Activar el panel de advertencia
        }
    }

    private IEnumerator ClearWarningMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (warningPanel != null)
        {
            warningPanel.SetActive(false); // Ocultar el panel de advertencia después del retraso
        }
    }
}
