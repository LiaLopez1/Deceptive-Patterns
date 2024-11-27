using UnityEngine;
using UnityEngine.UI;

public class EncuestaManager : MonoBehaviour
{
    [System.Serializable]
    public class Pregunta
    {
        public Toggle toggleSi; // Toggle para la opci�n "S�"
        public Toggle toggleNo; // Toggle para la opci�n "No"
        [HideInInspector]
        public string respuestaSeleccionada; // Para almacenar la respuesta seleccionada ("S�" o "No")
    }

    public Pregunta[] preguntas; // Lista de preguntas con sus toggles
    public Button botonSiguiente; // El bot�n que se activar�

    void Start()
    {
        // Inicializamos el bot�n como desactivado
        botonSiguiente.interactable = false;

        // Inicializamos los toggles y agregamos listeners
        foreach (var pregunta in preguntas)
        {
            // Ambos toggles comienzan desactivados
            pregunta.toggleSi.isOn = false;
            pregunta.toggleNo.isOn = false;
            pregunta.respuestaSeleccionada = ""; // Sin respuesta seleccionada

            // Agregamos listeners para controlar la exclusividad y verificar respuestas
            pregunta.toggleSi.onValueChanged.AddListener(delegate { OnToggleValueChanged(pregunta, "S�", pregunta.toggleNo); });
            pregunta.toggleNo.onValueChanged.AddListener(delegate { OnToggleValueChanged(pregunta, "No", pregunta.toggleSi); });
        }
    }

    // Este m�todo asegura que al activar un toggle, el otro se desactive y guarda la respuesta
    void OnToggleValueChanged(Pregunta pregunta, string respuesta, Toggle otherToggle)
    {
        if (otherToggle.isOn) // Si el otro toggle estaba activo, lo desactivamos
        {
            otherToggle.isOn = false;
        }

        // Guardamos la respuesta si este toggle est� activado
        if (pregunta.toggleSi.isOn || pregunta.toggleNo.isOn)
        {
            pregunta.respuestaSeleccionada = respuesta;
        }
        else
        {
            pregunta.respuestaSeleccionada = ""; // Sin respuesta si ambos toggles est�n apagados
        }

        // Verificamos si todas las preguntas est�n correctamente contestadas
        VerificarPreguntas();
    }

    void VerificarPreguntas()
    {
        // Iteramos sobre todas las preguntas
        foreach (var pregunta in preguntas)
        {
            // Si ambas opciones est�n desactivadas, el bot�n no se activa
            if (string.IsNullOrEmpty(pregunta.respuestaSeleccionada))
            {
                botonSiguiente.interactable = false;
                return; // Salimos de la funci�n porque hay preguntas sin contestar
            }
        }

        // Si llegamos aqu�, todas las preguntas tienen exactamente una respuesta v�lida
        botonSiguiente.interactable = true;
    }

    // M�todo para imprimir las respuestas seleccionadas (puedes usarlo al presionar el bot�n)
    public void ImprimirRespuestas()
    {
        foreach (var pregunta in preguntas)
        {
            Debug.Log($"Pregunta: {pregunta.toggleSi.name} / Respuesta seleccionada: {pregunta.respuestaSeleccionada}");
        }
    }
}
