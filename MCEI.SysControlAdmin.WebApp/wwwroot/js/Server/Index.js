// Inicializamos el Plugin de Autocompletado y Busqueda De Profesion u Oficio
$(document).ready(function () {
    $('#IdMembership').select2();
    $('#IdPrivilege').select2();
});
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