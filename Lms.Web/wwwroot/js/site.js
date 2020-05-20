// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
ShowAlert = function (action, options) {
    var settings = $.extend({
        title: "",
        text: "",
        confirmButtonText: "Tamam",
        cancelButtonText: "İptal",
        showCancel: false,
        defaultError: false,
    }, options);
    switch (action) {
        case "error":
            swal({
                title: "Hata",
                text: settings.defaultError ? "İşlem Sırasında Hata Oluştu" : settings.text,
                type: "error",
                confirmButtonClass: "btn btn-primary",
                cancelButtonClass: "btn btn-light",
                confirmButtonText: settings.confirmButtonText,
                position: settings.position
            });
            break;
        case "warning":
            swal({
                title: settings.title,
                text: settings.text,
                type: action,
                showCancelButton: settings.showCancel,
                confirmButtonClass: "btn btn-danger",
                cancelButtonClass: "btn btn-light",
                confirmButtonText: settings.confirmButtonText,
                cancelButtonText: settings.cancelButtonText,
                position: settings.position
            }).then(result => {
                if (result.value != null && result.value == true) {
                    if (settings.then) {
                        settings.then();
                        return;
                    }
                    return result.value;
                }

                if (settings.cancel)
                    settings.cancel();
            });
            break;
        case "success":
        case "question":
            Swal.fire({
                title: settings.title,
                text: settings.text,
                type: action,
                icon: 'success',
                showCancelButton: settings.showCancel,
                confirmButtonClass: "btn btn-primary",
                cancelButtonClass: "btn btn-light",
                confirmButtonText: settings.confirmButtonText,
                cancelButtonText: settings.cancelButtonText,
                position: settings.position
            }).then(result => {
                if (result.value != null && result.value == true) {
                    if (settings.then) {
                        settings.then();
                        return;
                    }
                }
                if (settings.cancel)
                    settings.cancel();
            });
            break;
        case "delete":
            swal({
                title: "Silme",
                text: "Silmek",
                type: "warning",
                showCancelButton: true,
                confirmButtonClass: "btn-danger",
                confirmButtonText: "Evet, Sil!",
                cancelButtonText: "Hayır, iptal!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        $.ajax({
                            url: "/Kategori/KategoriSil",
                            type: 'DELETE',
                            error: function () {
                                alert('Something is wrong');
                            },
                            success: function (data) {
                                $("#" + id).remove();
                                swal("success", "Silme işlemi Başarılı!");
                            }
                        });
                    } else {
                        swal("Cancelled", "Your imaginary file is safe :)", "error");
                    }
                });
            break;
        default:
    }
}


