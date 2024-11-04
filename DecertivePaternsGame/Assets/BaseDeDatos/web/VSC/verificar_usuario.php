<?php

try {
    $conexion = mysqli_connect("localhost", "root", "", "juegounity");
    if (!$conexion) {
        echo '{"codigo":400, "mensaje":"Error intentando conectar", "respuesta":""}';
    } else {
        // echo '{"codigo":200, "mensaje":"Conectado correctamente", "respuesta":""}';

        if(isset($_GET['nombreroll']))
                {
                  $usuarioNombre = $_GET['nombreroll'];
          
                  $sql = "SELECT * FROM `jugadores` WHERE nombreroll='".$usuarioNombre."'";
                  $resultado = $conexion->query($sql);

                  if ($resultado->num_rows > 0) {
                      echo '{"codigo":202, "mensaje":"El usuario ya existente", "respuesta":"'.$resultado->num_rows.'"}';
                  } else {
                      echo '{"codigo":203, "mensaje":"El usuario NO existe", "respuesta":"0"}';
                  }

                }else{
                  echo '{"codigo":402, "mensaje":"Faltan datos para ejecutar la accion solicitada", "respuesta":""}';
                }
    }
} catch (Exception $e) {
    echo '{"codigo":400, "mensaje":"Error intentando conectar", "respuesta":""}';
}

// include 'footer.php';

?>
