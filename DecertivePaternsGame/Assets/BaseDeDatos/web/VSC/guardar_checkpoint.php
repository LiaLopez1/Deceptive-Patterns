<?php
header("Content-Type: application/json; charset=UTF-8");

// Conexión a la base de datos
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "juegounity";

$conn = new mysqli($servername, $username, $password, $dbname);

// Verificar conexión
if ($conn->connect_error) {
    echo json_encode(array("codigo" => 500, "mensaje" => "Error de conexión: " . $conn->connect_error));
    exit;
}

// Verificar si se recibieron los datos necesarios
if (!isset($_POST["nombreroll"]) || !isset($_POST["checkpoint_x"]) || !isset($_POST["checkpoint_y"]) || !isset($_POST["checkpoint_z"])) {
    echo json_encode(array("codigo" => 400, "mensaje" => "Faltan datos en la solicitud"));
    exit;
}

// Recibir los datos de la solicitud POST
$nombreroll = $_POST["nombreroll"];
$checkpointX = $_POST["checkpoint_x"];
$checkpointY = $_POST["checkpoint_y"];
$checkpointZ = $_POST["checkpoint_z"];

// Crear el JSON para la posición
$checkpointPosition = json_encode(array("x" => $checkpointX, "y" => $checkpointY, "z" => $checkpointZ));

// Preparar y ejecutar la consulta para guardar el checkpoint usando nombreroll
$sql = "UPDATE jugadores SET checkpoint = ? WHERE nombreroll = ?";
$stmt = $conn->prepare($sql);
$stmt->bind_param("ss", $checkpointPosition, $nombreroll);

if ($stmt->execute()) {
    echo json_encode(array("codigo" => 200, "mensaje" => "Checkpoint guardado correctamente"));
} else {
    echo json_encode(array("codigo" => 500, "mensaje" => "Error al guardar el checkpoint: " . $stmt->error));
}

// Cerrar conexión
$stmt->close();
$conn->close();
?>
