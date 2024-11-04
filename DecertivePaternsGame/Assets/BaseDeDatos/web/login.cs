using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class login : MonoBehaviour
{
    public Servidor servidor;
    public TMP_InputField InputUsuario;
    public TMP_Text TxtIncorrecto;
    public TMP_Text TxtError;
    public TMP_Text TxtCampoVacio;
    public GameObject CrearUsuario;

    // Panel que se muestra al iniciar sesi�n correctamente
    public GameObject PanelInicioSesionCorrecto;

    // Variable para almacenar el nombre del usuario logueado
    public static string nombreRollActual;

    public void IniciarSesion()
    {
        if (string.IsNullOrEmpty(InputUsuario.text))
        {
            TxtCampoVacio.gameObject.SetActive(true);
            return;
        }

        StartCoroutine(Iniciar());
    }

    IEnumerator Iniciar()
    {
        string[] datos = new string[1];
        datos[0] = InputUsuario.text;  // Aqu� se captura el nombreRoll

        StartCoroutine(servidor.ConsumirServicio("login", datos, PosCarga));
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !servidor.ocupado);
    }

    public void PosCarga()
    {
        TxtCampoVacio.gameObject.SetActive(false);
        TxtIncorrecto.gameObject.SetActive(false);
        TxtError.gameObject.SetActive(false);

        switch (servidor.respuesta.codigo)
        {
            case 205: // Inicio de sesi�n correcto
                // Almacenar el nombreRoll actual cuando el inicio de sesi�n sea correcto
                nombreRollActual = InputUsuario.text;

                // Mostrar el panel de inicio de sesi�n correcto
                PanelInicioSesionCorrecto.SetActive(true);

                // Iniciar la espera de 6 segundos antes de cambiar de escena
                StartCoroutine(EsperarYCambiarEscena());
                break;

            case 204: // Usuario incorrecto
                TxtIncorrecto.gameObject.SetActive(true);
                break;

            case 404: // Error en la base de datos
                TxtError.gameObject.SetActive(true);
                break;

            default:
                break;
        }
    }

    IEnumerator EsperarYCambiarEscena()
    {
        // Esperar 6 segundos
        yield return new WaitForSeconds(6f);

        // Cambiar a la escena indicada
        SceneManager.LoadScene("CasaVer4");
    }

    public void ActivarCrearUsuarioCanvas()
    {
        TxtCampoVacio.gameObject.SetActive(false);
        TxtIncorrecto.gameObject.SetActive(false);
        TxtError.gameObject.SetActive(false);
        CrearUsuario.SetActive(true);
    }
}
