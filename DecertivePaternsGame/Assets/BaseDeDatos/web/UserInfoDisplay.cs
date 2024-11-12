using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UserInfoDisplay : MonoBehaviour
{
    public Servidor servidor;
    public GameObject userInfoCanvas;
    public TMP_Text userNameTMP;
    public TMP_Text userEmailTMP;
    public Button closeButton;

    private void Start()
    {
        userInfoCanvas.SetActive(false); // Asegurarse de que el canvas esté oculto al inicio

        // Asignar la acción para cerrar el canvas al botón
        closeButton.onClick.AddListener(() => userInfoCanvas.SetActive(false));
    }

    // Método para solicitar y mostrar los datos del usuario
    public void MostrarDatosUsuario()
    {
        StartCoroutine(SolicitarDatosUsuario());
    }

    private IEnumerator SolicitarDatosUsuario()
    {
        // Enviar solicitud al servidor para obtener la información del usuario
        string[] datos = new string[1];
        datos[0] = login.nombreRollActual; // Usamos el nombre de usuario almacenado al iniciar sesión

        StartCoroutine(servidor.ConsumirServicio("obtener_datos_usuario", datos, ProcesarDatosUsuario));
        yield return new WaitUntil(() => !servidor.ocupado); // Esperar a que se complete la solicitud
    }

    // Método para procesar la respuesta del servidor y actualizar el UI
    private void ProcesarDatosUsuario()
    {
        // Verificar el código de la respuesta
        if (servidor.respuesta.codigo == 200)
        {
            userNameTMP.text = servidor.respuesta.nombrecompleto;
            userEmailTMP.text = servidor.respuesta.correo;

            // Activar el canvas para mostrar la información del usuario
            userInfoCanvas.SetActive(true);
        }
        else
        {
            Debug.LogError("Error al obtener los datos del usuario: " + servidor.respuesta.mensaje);
        }
    }
}
