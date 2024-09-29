using System.Collections;
using UnityEngine;
using TMPro;

public class ContadorLlaves : MonoBehaviour
{
    public TextMeshProUGUI textoLlaves;  // El campo de texto en la UI donde se mostrar� el contador
    public int llavesActuales = 0;       // Contador local de llaves

    public Servidor servidor;            // Referencia al servidor para obtener llaves de la base de datos

    void Start()
    {
        textoLlaves.text = llavesActuales.ToString();

        // Cargar las llaves actuales del jugador desde la base de datos
        StartCoroutine(CargarLlavesDesdeBD());
    }

    // M�todo para actualizar el texto del contador en la UI
    public void ActualizarContador()
    {
        textoLlaves.text = llavesActuales.ToString();
    }

    // M�todo para cargar el n�mero de llaves desde la base de datos
    IEnumerator CargarLlavesDesdeBD()
    {
        string[] datos = new string[1];
        datos[0] = login.nombreRollActual;  // Usar el nombre del usuario logueado

        // Consumir el servicio para obtener las llaves del servidor
        StartCoroutine(servidor.ConsumirServicio("obtener_llaves", datos, CallbackCargarLlaves));
        yield return new WaitUntil(() => !servidor.ocupado);
    }

    // Callback para actualizar el n�mero de llaves desde el servidor 
    void CallbackCargarLlaves()
    {
        // Aqu� obtienes el n�mero de llaves en la respuesta del servidor
        llavesActuales = servidor.respuesta.llaves;
        ActualizarContador();  // Actualizar el contador en la UI
    }

    // Llamar a este m�todo cuando el jugador recoja una llave
    public void RecolectarLlave()
    {
        llavesActuales++;  // Incrementar el n�mero de llaves localmente 
        ActualizarContador();  // Actualizar la UI 
    }
}
