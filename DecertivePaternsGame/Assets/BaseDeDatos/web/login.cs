using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class login : MonoBehaviour
{
    public Servidor servidor;
    public TMP_InputField InputUsuario;
    public GameObject Loading;
    public TMP_Text TxtIncorrecto;
    public TMP_Text TxtError;
    public TMP_Text TxtCampoVacio;
    public GameObject CrearUsuario;

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
        Loading.SetActive(true);
        string[] datos = new string[1];
        datos[0] = InputUsuario.text;

        StartCoroutine(servidor.ConsumirServicio("login", datos, PosCarga));
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !servidor.ocupado);
        Loading.SetActive(false);
    }

    public void PosCarga()
    {
        TxtIncorrecto.gameObject.SetActive(false);
        TxtError.gameObject.SetActive(false);
        Loading.SetActive(false);

        switch (servidor.respuesta.codigo)
        {
            case 204: // El usuario es incorrecto
                TxtIncorrecto.gameObject.SetActive(true);
                Loading.SetActive(true);
                break;

            case 205: // Inicio de sesión correcto
                SceneManager.LoadScene("SampleScene");
                break;

            case 404: // Error DB
                TxtError.gameObject.SetActive(true);
                Loading.SetActive(true);
                break;

            default:
                break;
        }
    }

    public void ActivarCrearUsuarioCanvas()
    {
        TxtCampoVacio.gameObject.SetActive(false);
        TxtIncorrecto.gameObject.SetActive(false);
        TxtError.gameObject.SetActive(false);
        CrearUsuario.SetActive(true);
    }
  
}
