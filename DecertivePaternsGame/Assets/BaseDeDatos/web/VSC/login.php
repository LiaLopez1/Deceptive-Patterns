<?php
header("Content-Type: application/json; charset=UTF-8");

try {
    // Conexión a la base de datos
    $conexion = mysqli_connect("localhost", "root", "", "juegounity");
    if (!$conexion) {
        echo json_encode(array("codigo" => 400, "mensaje" => "Error intentando conectar", "respuesta" => ""));
    } else {
        if (isset($_POST['nombreroll'])) {
            $usuarioNombre = $_POST['nombreroll'];
            
            // Consulta para buscar el usuario
            $sql = "SELECT * FROM `jugadores` WHERE nombreroll='".$usuarioNombre."'";
            $resultado = $conexion->query($sql);

            if ($resultado->num_rows > 0) {
                // Sí existe un usuario con esos datos
                $row = $resultado->fetch_assoc();
                
                // Crear el array con los datos del usuario, incluyendo el checkpoint
                $data = array(
                    "id" => $row['id'],
                    "terminos" => $row['terminos'],
                    "nombreroll" => $row['nombreroll'],
                    "nombrecompleto" => $row['nombrecompleto'],
                    "checkpoint" => json_decode($row['checkpoint']) // Decodificar el JSON de checkpoint
                );
                
                // Devolver el JSON con los datos del usuario
                echo json_encode(array("codigo" => 205, "mensaje" => "Inicio de sesión correcto", "respuesta" => $data));
            } else {
                // No existe un usuario con esos datos
                echo json_encode(array("codigo" => 204, "mensaje" => "El usuario es incorrecto", "respuesta" => "0"));
            }
        } else {
            // Falta el dato nombreroll en la solicitud POST
            echo json_encode(array("codigo" => 402, "mensaje" => "Faltan datos para ejecutar la acción solicitada", "respuesta" => ""));
        }
    }
} catch (Exception $e) {
    echo json_encode(array("codigo" => 400, "mensaje" => "Error intentando conectar", "respuesta" => ""));
}
?>
