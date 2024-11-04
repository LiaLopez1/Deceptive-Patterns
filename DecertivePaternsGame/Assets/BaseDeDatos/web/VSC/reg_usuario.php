<?php

try {
    // Establecer conexión con la base de datos
    $conexion = mysqli_connect("localhost", "root", "", "juegounity");
    if (!$conexion) {
        echo json_encode(array("codigo" => 400, "mensaje" => "Error intentando conectar", "respuesta" => ""));
    } else {
        // Verificar si los datos requeridos han sido enviados mediante POST
        if (isset($_POST['terminos']) && isset($_POST['nombreroll']) && isset($_POST['nombrecompleto'])) {

            // Asignar los datos POST a variables
            $rtaTerminos = $_POST['terminos'];
            $usuarioNombre = $_POST['nombreroll'];
            $usuarioNombreCompleto = $_POST['nombrecompleto'];

            // Comprobar si el nombre de usuario ya existe
            $sql = "SELECT * FROM `jugadores` WHERE nombreroll = '".$usuarioNombre."'";
            $resultado = $conexion->query($sql);

            if ($resultado->num_rows > 0) {
                // Si el usuario ya existe, devolver un código de error 403
                echo json_encode(array("codigo" => 403, "mensaje" => "Ya existe un usuario registrado con ese nombre", "respuesta" => $resultado->num_rows));
            } else {
                // Si el usuario no existe, insertar el nuevo registro
                $sql = "INSERT INTO `jugadores` (`id`, `terminos`, `nombreroll`, `nombrecompleto`) VALUES (NULL, '" . $rtaTerminos . "', '" . $usuarioNombre . "', '" . $usuarioNombreCompleto . "');";
                
                if ($conexion->query($sql) === TRUE) {
                    // Si la inserción es exitosa, buscar y devolver los datos del nuevo usuario
                    $sql = "SELECT * FROM `jugadores` WHERE nombreroll = '".$usuarioNombre."'";
                    $resultado = $conexion->query($sql);
                    $text = '';

                    // Construir la respuesta en JSON
                    while ($row = $resultado->fetch_assoc()) {
                        $text = json_encode(array(
                            "id" => $row['id'],
                            "terminos" => $row['terminos'],
                            "nombreroll" => $row['nombreroll'],
                            "nombrecompleto" => $row['nombrecompleto']
                        ));
                    }

                    echo json_encode(array("codigo" => 201, "mensaje" => "Usuario creado correctamente", "respuesta" => $text));
                } else {
                    // Si ocurre un error al insertar
                    echo json_encode(array("codigo" => 401, "mensaje" => "Error creando el usuario", "respuesta" => ""));
                }
            }

        } else {
            // Si faltan datos en la solicitud POST
            echo json_encode(array("codigo" => 402, "mensaje" => "Faltan datos para ejecutar la acción solicitada", "respuesta" => ""));
        }
    }
} catch (Exception $e) {
    // Manejar cualquier excepción
    echo json_encode(array("codigo" => 400, "mensaje" => "Error intentando conectar", "respuesta" => ""));
}

?>
