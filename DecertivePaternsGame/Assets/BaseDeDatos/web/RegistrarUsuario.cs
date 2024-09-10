using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RegistrarUsuario : MonoBehaviour
{
    public Servidor servidor;

    // Referencias a los elementos de la UI
    public TMP_InputField inputNombreCompleto;
    public TMP_InputField inputNombreRoll;
    public Toggle toggleAceptar;
    public Toggle toggleNoAceptar;
    public GameObject loading;
    public TMP_Text txtErrorDB;
    public TMP_Text txtCorrecto;
    public TMP_Text txtUsado;
    public TMP_Text txtCampos;
    public TMP_Text txtTerminos;
    public GameObject crearUsuarioCanvas;  // Referencia al Canvas de crear usuario
    public Button btnCerrarCanvas;  // El botón que aparecerá cuando el usuario se registre correctamente

    void Start()
    {
        // Asegurarse de que solo un toggle puede estar activo a la vez
        toggleAceptar.onValueChanged.AddListener(OnToggleAceptarChanged);
        toggleNoAceptar.onValueChanged.AddListener(OnToggleNoAceptarChanged);

        // Ocultar el botón al inicio
        btnCerrarCanvas.gameObject.SetActive(false);

        // Asignar la función de cerrar el canvas al botón
        btnCerrarCanvas.onClick.AddListener(CerrarCanvasCrearUsuario);
    }

    // Si se selecciona el toggle "Aceptar", desactiva el toggle "NoAceptar"
    private void OnToggleAceptarChanged(bool isOn)
    {
        if (isOn)
        {
            toggleNoAceptar.isOn = false;
        }
    }

    // Si se selecciona el toggle "NoAceptar", desactiva el toggle "Aceptar"
    private void OnToggleNoAceptarChanged(bool isOn)
    {
        if (isOn)
        {
            toggleAceptar.isOn = false;
        }
    }

    // Método llamado al presionar el botón de registro
    public void RegistrarNuevoUsuario()
    {
        StartCoroutine(RegistrarUsuarioCoroutine());
    }

    IEnumerator RegistrarUsuarioCoroutine()
    {
        // Mostrar animación de carga
        loading.SetActive(true);

        // Obtener los valores ingresados por el usuario
        string nombreCompleto = inputNombreCompleto.text;
        string nombreRoll = inputNombreRoll.text;
        int terminos = toggleAceptar.isOn ? 1 : 2;

        // Validar que los campos no estén vacíos
        if (string.IsNullOrEmpty(nombreCompleto) || string.IsNullOrEmpty(nombreRoll))
        {
            txtCampos.gameObject.SetActive(true);
            loading.SetActive(false);
            yield break;
        }

        // Validar que al menos uno de los toggles esté seleccionado
        if (!toggleAceptar.isOn && !toggleNoAceptar.isOn)
        {
            txtTerminos.gameObject.SetActive(true);
            loading.SetActive(false);
            yield break;
        }

        // Preparar los datos para enviar al servidor
        string[] datos = new string[3];
        datos[0] = terminos.ToString();
        datos[1] = nombreRoll;
        datos[2] = nombreCompleto;

        // Consumir el servicio de registro
        StartCoroutine(servidor.ConsumirServicio("registro", datos, PosRegistro));
        yield return new WaitUntil(() => !servidor.ocupado);

        loading.SetActive(false);
    }

    public void PosRegistro()
    {
        txtUsado.gameObject.SetActive(false);
        txtCorrecto.gameObject.SetActive(false);
        txtErrorDB.gameObject.SetActive(false);
        txtCampos.gameObject.SetActive(false);
        txtTerminos.gameObject.SetActive(false);

        switch (servidor.respuesta.codigo)
        {
            case 201: // Usuario creado correctamente
                txtCorrecto.gameObject.SetActive(true);
                btnCerrarCanvas.gameObject.SetActive(true);  // Mostrar el botón cuando el registro es exitoso
                break;

            case 403: // Usuario ya existe
                txtUsado.gameObject.SetActive(true);
                break;

            case 404: // Error de base de datos
                txtErrorDB.gameObject.SetActive(true);
                break;

            default:
                break;
        }
    }

    public void CerrarCanvasCrearUsuario()
    {
        crearUsuarioCanvas.SetActive(false); 
        btnCerrarCanvas.gameObject.SetActive(false);  
    }
}
