using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

[CreateAssetMenu(fileName = "Servidor", menuName = "Game/Servidor", order = 1)]
public class Servidor : ScriptableObject
{
    public string servidor;
    public Servicio[] servicios;

    public bool ocupado = false;
    public Respuesta respuesta;

    public IEnumerator ConsumirServicio(string nombre, string[] datos, UnityAction e)
    {
        ocupado = true;

        WWWForm formulario = new WWWForm();
        Servicio s = new Servicio();

        // Buscar el servicio correspondiente
        for (int i = 0; i < servicios.Length; i++)
        {
            if (servicios[i].nombre.Equals(nombre))
            {
                s = servicios[i];
            }
        }

        // A�adir los par�metros al formulario
        for (int i = 0; i < s.parametros.Length; i++)
        {
            formulario.AddField(s.parametros[i], datos[i]);
        }

        // Realizar la petici�n POST al servidor
        UnityWebRequest www = UnityWebRequest.Post(servidor + "/" + s.URL, formulario);
        Debug.Log(servidor + "/" + s.URL);
        yield return www.SendWebRequest();

        // Manejar errores de la solicitud
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error en la conexi�n: " + www.error);
            respuesta = new Respuesta(); // C�digo por defecto (404)
        }
        else
        {
            Debug.Log("Respuesta del servidor: " + www.downloadHandler.text);
            respuesta = JsonUtility.FromJson<Respuesta>(www.downloadHandler.text);
        }

        ocupado = false;
        e.Invoke();
    }
}

[System.Serializable]
public class Servicio
{
    public string nombre;  // Nombre del servicio (ej. "registro")
    public string URL;     // URL del archivo PHP en el servidor
    public string[] parametros;  // Lista de par�metros que requiere el servicio
}

[System.Serializable]
public class Respuesta
{
    public int codigo;     // C�digo de la respuesta (ej. 201, 403, etc.)
    public string mensaje; // Mensaje de la respuesta

    public Respuesta()
    {
        codigo = 404;      // C�digo por defecto en caso de error
        mensaje = "Error"; // Mensaje por defecto
    }
}
