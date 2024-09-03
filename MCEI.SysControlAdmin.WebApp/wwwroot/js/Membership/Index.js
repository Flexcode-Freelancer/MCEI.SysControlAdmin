// Inicializamos el Plugin de Autocompletado y Busqueda De Profesion u Oficio
$(document).ready(function () {
    $('#IdProfessionOrStudy').select2();

    // Ocultar el select original
    $('#IdProfessionOrStudy').hide();
});

document.getElementById('inputDui').addEventListener('input', function (event) {
    var inputValue = event.target.value;
    var formattedValue = formatDui(inputValue);
    event.target.value = formattedValue;
});

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
    // Limitar la longitud total a 10 caracteres, incluyendo el guion
    formattedValue = formattedValue.substring(0, 10);
    return formattedValue;
}

new DataTable('#ResultData', {
    info: false,
    order: false,
    Response: true,
    pagingType: 'simple_numbers',
    language: {
        search: 'Busqueda Rapida :',
        searchPlaceholder: 'Ingresar',
        lengthMenu: '_MENU_ Mostrar',
        emptyTable: 'No Hay Datos Que Coincidan Con Su Criterio De Búsqueda.',
        zeroRecords: 'No Existen Registros Segun La Busqueda',
        processing: 'Procesando...',
    }
});

const Toast = Swal.mixin({
    toast: true,
    position: "top-end",
    showConfirmButton: false,
    timer: 8000,
    timerProgressBar: true,
    didOpen: (toast) => {
        toast.addEventListener('mouseenter', Swal.stopTimer);
        toast.addEventListener('mouseleave', Swal.resumeTimer);
    }
});

// Comprobar si hay mensajes de éxito y mostrar notificaciones del sistema
const successMessages = {
    Creado: '@TempData["SuccessMessageCreate"]',
    Modificado: '@TempData["SuccessMessageUpdate"]',
    Eliminado: '@TempData["SuccessMessageDelete"]'
};

for (const messageType in successMessages) {
    const message = successMessages[messageType];
    if (message) {
        Toast.fire({
            icon: "success",
            title: `¡${messageType} Exitosamente!`,
            text: message
        });
    }
}