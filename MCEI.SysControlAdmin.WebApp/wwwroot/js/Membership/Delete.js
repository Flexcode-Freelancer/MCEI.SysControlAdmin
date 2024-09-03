// Función para mostrar una alerta de error utilizando SweetAlert2
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
    var duiElement = document.getElementById("Dui"); // Selecciona el elemento <dd> con el id "Dui"
    var duiValue = duiElement.innerText; // Obtén el valor de Dui del elemento
    var barcodeSvg = document.getElementById("barcode"); // Selecciona el elemento <svg> con el id "barcode"
    JsBarcode(barcodeSvg, duiValue); // Genera el código de barras basado en el valor de Dui

    // Verificar si el usuario es menor de edad y mostrar mensaje
    if (age <= 17) {
        const message = `${name} ${lastName} Actualmente Es Menor De Edad`; // Concatenamos el nombre, apellido y el mensaje requerido.
        minorSpan.textContent = message; // Establecemos el texto del span con el mensaje.
    } else {
        minorSpan.textContent = ''; // Si la edad es mayor o igual a 18, vaciamos el texto del span.
    }
});

// Función para mostrar una alerta de confirmación antes de eliminar un registro
function CreateRecord(e) {
    e.preventDefault();
    const form = document.getElementById('Form');

    Swal.fire({
        title: '¿Eliminar Este Registro?',
        text: '¿Estás seguro de eliminar el siguiente registro? Recuerda que no puedes revertir los cambios.',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sí, Eliminar Registro',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            form.submit();
        }
    });
}

// Variables globales para manejar Razor en JavaScript
const age = parseInt(ageValue);
const name = nameValue;
const lastName = lastNameValue;
const minorSpan = document.getElementById('minor'); // Obtenemos el elemento span con ID "minor".
