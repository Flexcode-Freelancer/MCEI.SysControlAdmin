// ------------------------- Inicialización -------------------------
$(document).ready(function () {
    initializeForm();
    initializeSelect2();
    bindEvents();
    calculateAge();
    actualizarCodigoDeBarras();
});

function initializeForm() {
    $("#cbxGender").val(gender);
    $("#cbxCivilStatus").val(civilStatus);
    $("#cbxProfessionOrStudy").val(professionOrStudy);
    $("#cbxWaterBaptism").val(waterBaptism);
    $("#cbxBaptismOfTheHolySpirit").val(baptismOfTheHolySpirit);
    $("#cbxZone").val(zone);
    $("#cbxDistrict").val(district);
    $("#cbxSector").val(sector);
    $("#cbxCell").val(cell);
    $("#cbxStatus").val(status);

    const duiInput = document.getElementById("inputDui");
    const duiPattern = /^[A-Z]{2}\d{6}$/;

    if (duiPattern.test(duiInput.value.trim())) {
        isMinorCheckbox.checked = true;
        duiInput.readOnly = true;
        generateDuiValue();
    } else {
        isMinorCheckbox.checked = false;
        duiInput.readOnly = false;
    }
}

function initializeSelect2() {
    $('#cbxProfessionOrStudy').select2();
}

function bindEvents() {
    isMinorCheckbox.addEventListener("change", handleCheckboxChange);
    document.getElementById('inputDui').addEventListener('input', function () {
        actualizarCodigoDeBarras();
        this.value = formatDui(this.value);
    });
    document.querySelector('input[name="DateOfBirth"]').addEventListener('change', calculateAge);
    document.getElementById('inputPhone').addEventListener('input', function (e) {
        formatPhoneNumber(e.target);
    });
    document.getElementById('inputPhoneEmergency').addEventListener('input', function (e) {
        formatPhoneNumber(e.target);
    });
}

// Funciones restantes...
function showErrorAlert(message) {
    Swal.fire({
        title: 'Error',
        text: message,
        icon: 'error'
    });
}

function handleCheckboxChange() {
    const duiInput = document.getElementById("inputDui");
    if (isMinorCheckbox.checked) {
        duiInput.readOnly = true;
        generateDuiValue();
    } else {
        duiInput.readOnly = false;
    }
}

function formatDui(inputValue) {
    inputValue = inputValue.replace(/\D/g, '');
    return inputValue.length > 8 ? inputValue.slice(0, 8) + '-' + inputValue.slice(8) : inputValue;
}

function actualizarCodigoDeBarras() {
    var codeDui = document.getElementById("inputDui").value;
    JsBarcode("#barcode", codeDui);
}

function calculateAge() {
    const birthDateInput = document.querySelector('input[name="DateOfBirth"]');
    const ageInput = document.querySelector('input[name="Age"]');
    if (birthDateInput.value.trim() !== "") {
        const birthDate = new Date(birthDateInput.value.trim());
        const age = new Date().getFullYear() - birthDate.getFullYear();
        ageInput.value = birthDate.getMonth() > new Date().getMonth() || (birthDate.getMonth() === new Date().getMonth() && birthDate.getDate() > new Date().getDate()) ? age - 1 : age;
    } else {
        ageInput.value = "";
    }
}

function formatPhoneNumber(input) {
    input.value = input.value.replace(/\D/g, '').replace(/(\d{4})(\d+)/, '$1-$2').slice(0, 9);
}

function mostrarImagen() {
    var archivo = document.getElementById('imagen').files[0];
    var reader = new FileReader();
    reader.onload = function (e) {
        document.getElementById('imagenPreview').src = e.target.result;
        document.getElementById('imagenPreview').style.display = 'block';
    };
    reader.readAsDataURL(archivo);
}

function CreateRecord(e) {
    e.preventDefault();
    const form = document.getElementById('Form');

    // Mostrar La Alerta De Confirmacion
    Swal.fire({
        title: '¿Modificar Registro Existente?',
        icon: 'question',
        showCancelButton: true,
        confirmButtonText: 'Sí, Modificar Registro'
    }).then((result) => {
        if (result.isConfirmed) {
            form.submit();
        }
    });
}
