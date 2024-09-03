function showErrorAlert(message) {
    Swal.fire({
        title: 'Error',
        text: message,
        icon: 'error'
    });
}
// Captura el checkbox "Soy menor de edad"
var isMinorCheckbox = document.getElementById("isMinorCheckbox");

// Campos de nombre, apellido y fecha de nacimiento
const nameInput = document.querySelector('input[name="Name"]');
const lastNameInput = document.querySelector('input[name="LastName"]');
const birthDateInput = document.querySelector('input[name="DateOfBirth"]');
const duiInput = document.getElementById("inputDui");

// Función para formatear la fecha en DDMMAA
function formatBirthDate(birthDate) {
    const day = String(birthDate.getDate() + 1).padStart(2, '0');
    const month = String(birthDate.getMonth() + 1).padStart(2, '0');
    const year = String(birthDate.getFullYear()).slice(-2);
    return `${day}${month}${year}`;
}

// Función para generar el valor para Dui
function generateDuiValue() {
    // Verifica que los campos de nombre, apellido y fecha de nacimiento no estén vacíos
    const nameValue = nameInput.value.trim();
    const lastNameValue = lastNameInput.value.trim();
    const birthDateValue = birthDateInput.value.trim();

    // Si alguno de los campos está vacío, no establecer ningún valor en `Dui`
    if (!nameValue || !lastNameValue || !birthDateValue) {
        return;
    }

    // Extrae las iniciales de los campos de nombre y apellido
    const nameInitial = nameValue.charAt(0).toUpperCase();
    const lastNameInitial = lastNameValue.charAt(0).toUpperCase();

    // Formatea la fecha de nacimiento
    const birthDate = new Date(birthDateValue);
    const formattedBirthDate = formatBirthDate(birthDate);

    // Concatenar iniciales y fecha de nacimiento
    const newDuiValue = `${nameInitial}${lastNameInitial}${formattedBirthDate}`;

    // Establecer el valor generado como valor del campo Dui
    duiInput.value = newDuiValue;

    // Actualizar el código de barras con el valor de Dui
    actualizarCodigoDeBarras();
}

// Función para manejar el evento de cambio del checkbox
function handleCheckboxChange() {
    if (isMinorCheckbox.checked) {
        // Si el checkbox está marcado, marcar Dui como solo lectura
        duiInput.readOnly = true;
        // Generar el valor para Dui
        generateDuiValue();
    } else {
        // Si el checkbox está desmarcado, hacer Dui editable
        duiInput.readOnly = false;
        // Limpiar el campo Dui
        duiInput.value = '';
    }
}

// Función para actualizar el código de barras
function actualizarCodigoDeBarras() {
    var codeDui = duiInput.value;
    JsBarcode("#barcode", codeDui); // Generar el código de barras usando el valor actual
}

// Asignar el evento de cambio al checkbox
isMinorCheckbox.addEventListener("change", handleCheckboxChange);

// Asignar evento `input` a `duiInput` para que actualice el código de barras cada vez que se modifique `Dui`
duiInput.addEventListener("input", function () {
    actualizarCodigoDeBarras();
});

document.addEventListener("DOMContentLoaded", function () {
    // Insercion de (-) al Dui, Ademas de Generar Codigo de Barras En Base Al Dui
    function formatDui(inputValue) {
        inputValue = inputValue.replace(/\D/g, ''); // Remover todos los caracteres que no sean dígitos
        var formattedValue = '';
        for (var i = 0; i < inputValue.length; i++) {
            if (i == 8) {
                formattedValue += '-' + inputValue.charAt(i);
            } else {
                formattedValue += inputValue.charAt(i);
            }
        }
        formattedValue = formattedValue.substring(0, 10); // Limitar la longitud total a 10 caracteres, incluyendo el guion
        return formattedValue;
    }

    function actualizarCodigoDeBarras() {
        var codeDui = document.getElementById("inputDui").value;
        JsBarcode("#barcode", codeDui); // Generar el código de barras usando el valor actual
    }

    // Calculo De La Edad En Base a La Fecha De Nacimiento
    const birthDateInput = document.querySelector('input[name="DateOfBirth"]');
    const ageInput = document.querySelector('input[name="Age"]');

    function calculateAge() {
        const birthDateValue = birthDateInput.value.trim();
        if (birthDateValue !== "") {
            const birthDate = new Date(birthDateValue);
            const currentDate = new Date();
            let age = currentDate.getFullYear() - birthDate.getFullYear(); // Calcule la edad restando el año de nacimiento del año actual

            // Ajusta la edad si la fecha de nacimiento aún no ha ocurrido este año
            if (birthDate.getMonth() > currentDate.getMonth() ||
                (birthDate.getMonth() === currentDate.getMonth() && birthDate.getDate() > currentDate.getDate())) {
                age--;
            }
            ageInput.value = age; // Actualizar el campo de edad
        } else {
            ageInput.value = ""; // Desactive el campo de edad si no se proporciona ninguna fecha de nacimiento
        }
    }

    // Insercion de (-) despues de cada 4 numeros en los numeros de telefono
    const inputPhone = document.getElementById('inputPhone');
    const inputPhoneEmergency = document.getElementById('inputPhoneEmergency');

    function formatPhoneNumber(input) {
        let phoneNumber = input.value.replace(/\D/g, ''); // Elimina cualquier carácter que no sea número
        if (phoneNumber.length > 4) {
            phoneNumber = phoneNumber.slice(0, 4) + '-' + phoneNumber.slice(4, 8); // Formatea el número con un guion después de los primeros 4 dígitos
        }
        input.value = phoneNumber;
    }

    // Asignar eventos
    document.getElementById('inputDui').addEventListener('input', function (event) {
        var inputValue = event.target.value;
        var formattedValue = formatDui(inputValue);
        event.target.value = formattedValue;
        actualizarCodigoDeBarras();
    });

    birthDateInput.addEventListener('change', calculateAge); // Agregar detector de eventos para el cambio en la fecha de nacimiento
    inputPhone.addEventListener('input', function (e) {
        formatPhoneNumber(e.target);
    });
    inputPhoneEmergency.addEventListener('input', function (e) {
        formatPhoneNumber(e.target);
    });

    // Llamadas iniciales
    actualizarCodigoDeBarras(); // Generar el código de barras inicial
    calculateAge(); // Calcular la antigüedad cuando se carga la página
});

// Inicializamos el Plugin de Autocompletado y Busqueda De Profesion u Oficio
$(document).ready(function () {
    $('#IdProfessionOrStudy').select2();
});

// Método Para Mostrar Vista Previa De La Imagen Seleccionada
function mostrarImagen() {
    var archivo = document.getElementById('imagen').files[0];
    var reader = new FileReader();

    reader.onload = function (e) {
        var imagenPreview = document.getElementById('imagenPreview');
        imagenPreview.src = e.target.result;
        imagenPreview.style.display = 'block';
    };
    reader.readAsDataURL(archivo);
}

// Alerta con SweetAlert2
function CreateRecord(e) {
    e.preventDefault();
    const form = document.getElementById('Form');
    const inputs = document.querySelectorAll('.form-input');
    const nameFields = ['Name', 'LastName', 'PastorsName', 'SupervisorsName', 'LeadersName', 'TimeToGather'];
    const regex = /^[A-ZÑa-zñáéíóúÁÉÍÓÚ. ]+$/;

    let isValid = true;

    // Validar que los campos nameFields solo contengan letras, punto y espacios
    nameFields.forEach((field) => {
        const input = document.querySelector(`#${field}`);
        if (!regex.test(input.value)) {
            isValid = false;
        }
    });

    // Si la validación falla, muestra una alerta advirtiendo al usuario
    if (!isValid) {
        Swal.fire({
            title: '!Advertencia!',
            text: 'Ciertos Campos Solo Deben Tener Letras, Revisa Tu Ficha',
            icon: 'warning',
            confirmButtonText: 'Aceptar'
        });
        return;
    }

    // Si todos los campos son válidos, muestra la alerta de confirmación
    Swal.fire({
        title: '¿Guardar Nuevo Registro?',
        icon: 'question',
        showCancelButton: true,
        confirmButtonText: 'Sí, Guardar Registro'
    }).then((result) => {
        if (result.isConfirmed) {
            form.submit();
        }
    });
}