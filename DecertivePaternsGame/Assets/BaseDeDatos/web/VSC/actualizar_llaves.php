<?php
try {
    // Conexión a la base de datos
    $conexion = new mysqli("localhost", "root", "", "juegounity");

    if ($conexion->connect_error) {
        echo json_encode(array("codigo" => 400, "mensaje" => "Error al conectar con la base de datos", "respuesta" => ""));
    } else {
        if (isset($_POST['nombreroll']) && isset($_POST['llaves'])) {
            $usuarioNombre = $_POST['nombreroll'];
            $llavesNuevas = intval($_POST['llaves']);

            // Consulta para actualizar las llaves
            $sql = "UPDATE `jugadores` SET llaves = ? WHERE nombreroll = ?";
            $stmt = $conexion->prepare($sql);
            $stmt->bind_param("is", $llavesNuevas, $usuarioNombre);

            if ($stmt->execute()) {
                echo json_encode(array("codigo" => 200, "mensaje" => "Llaves actualizadas correctamente", "respuesta" => ""));
            } else {
                echo json_encode(array("codigo" => 500, "mensaje" => "Error al actualizar las llaves", "respuesta" => ""));
            }

            $stmt->close();
        } else {
            echo json_encode(array("codigo" => 402, "mensaje" => "Faltan datos para ejecutar la acción solicitada", "respuesta" => ""));
        }
    }

    $conexion->close();
} catch (Exception $e) {
    echo json_encode(array("codigo" => 500, "mensaje" => "Error interno del servidor", "respuesta" => ""));
}
?>
