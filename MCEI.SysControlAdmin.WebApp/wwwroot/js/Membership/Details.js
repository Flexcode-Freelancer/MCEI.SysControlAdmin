// Función para mostrar alertas de error utilizando SweetAlert2
function showErrorAlert(message) {
    Swal.fire({
        title: 'Error',
        text: message,
        icon: 'error'
    });
}

// Evento que se ejecuta una vez que el DOM ha sido completamente cargado
document.addEventListener("DOMContentLoaded", function () {

    // Generar el código de barras basado en el valor de Dui
    var duiElement = document.getElementById("Dui"); // Selecciona el elemento <dd> con el id "dui-field"
    var duiValue = duiElement.innerText; // Obtén el valor de Dui del elemento
    var barcodeSvg = document.getElementById("barcode"); // Selecciona el elemento <svg> con el id "barcode"
    JsBarcode(barcodeSvg, duiValue); // Genera el código de barras basado en el valor de Dui

    // Agregar el evento de clic al botón de modificar
    document.getElementById('modifyButton').addEventListener('click', function (event) {
        event.preventDefault(); // Prevenir la acción predeterminada del enlace
        Swal.fire({
            title: '¿Ir a La Vista Modificar?',
            text: "¿Estás seguro de que quieres ir a la vista Modificar?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Sí, Ir a La Vista Modificar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                window.location.href = modifyUrl; // Variable global para la URL de modificación
            }
        });
    });

    // Verificar si el usuario es menor de edad y mostrar mensaje
    if (age <= 17) {
        const message = `${name} ${lastName} Actualmente Es Menor De Edad`; // Concatenamos el nombre, apellido y el mensaje requerido.
        minorSpan.textContent = message; // Establecemos el texto del span con el mensaje.
    } else {
        minorSpan.textContent = ''; // Si la edad es mayor o igual a 18, vaciamos el texto del span.
    }
});

// Variables globales para manejar Razor en JavaScript
const age = parseInt(ageValue);
const name = nameValue;
const lastName = lastNameValue;
const minorSpan = document.getElementById('minor'); // Obtenemos el elemento span con ID "minor".
