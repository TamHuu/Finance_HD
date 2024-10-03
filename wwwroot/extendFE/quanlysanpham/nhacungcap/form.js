$(document).ready(function () {
    $('#btnSave').on('click', function (e) {
        e.preventDefault();

        var Id = $('#Id').val();
        var name = $('#Name').val();
        var Email = $('#Email').val();
        var Phone = $('#Phone').val();
        var Address = $('#Address').val();

        var url = Id && Id !== "0" ? '/Admin/Supplier/Edit' : '/Admin/Supplier/Add';

        var data = {
            Id: Id,
            Name: name,
            Email: Email,
            Phone: Phone,
            Address: Address
        };

        $.ajax({
            url: url,
            type: 'POST',
            data: data,
            success: function (response) {
                if (response.success) {
                    Swal.fire({
                        title: 'Thành công!',
                        text: response.message,
                        icon: 'success'
                    }).then(() => {
                        window.location.href = "/Admin/Supplier";
                    });
                } else {
                    Swal.fire({
                        title: 'Thất bại!',
                        text: response.message,
                        icon: 'error'
                    });
                }
            },
            error: function (xhr, status, error) {
                Swal.fire({
                    title: 'Đã xảy ra lỗi!',
                    text: 'Vui lòng thử lại.',
                    icon: 'error'
                });
                console.error(error);
            }
        });
    });


});
