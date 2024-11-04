<?php
try {
    // Conexión a la base de datos
    $conexion = new mysqli("localhost", "root", "", "juegounity");

    if ($conexion->connect_error) {
        echo json_encode(array("codigo" => 400, "mensaje" => "Error al conectar con la base de datos", "respuesta" => ""));
    } else {
        if (isset($_POST['nombreroll'])) {
            $usuarioNombre = $_POST['nombreroll'];

            // Consulta para obtener el número de llaves
            $sql = "SELECT llaves FROM `jugadores` WHERE nombreroll = ?";
            $stmt = $conexion->prepare($sql);
            $stmt->bind_param("s", $usuarioNombre);
            $stmt->execute();
            $resultado = $stmt->get_result();

            if ($resultado->num_rows > 0) {
                $row = $resultado->fetch_assoc();
                // Devolver las llaves bajo la clave 'llaves'
                echo json_encode(array("codigo" => 200, "mensaje" => "Llaves obtenidas correctamente", "llaves" => $row['llaves']));
            } else {
                echo json_encode(array("codigo" => 404, "mensaje" => "Usuario no encontrado", "llaves" => 0));
            }

            $stmt->close();
        } else {
            echo json_encode(array("codigo" => 402, "mensaje" => "Faltan datos para ejecutar la acción solicitada", "llaves" => 0));
        }
    }

    $conexion->close();
} catch (Exception $e) {
    echo json_encode(array("codigo" => 500, "mensaje" => "Error interno del servidor", "llaves" => 0));
}
?>
