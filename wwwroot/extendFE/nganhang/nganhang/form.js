let data
$(document).ready(function () {
    loadDanhSach();
});

function loadDanhSach() {
    $('#btnSave').on('click', function (e) {
        e.preventDefault();

        var Ma = $("#Ma").val();
        var Ten = $("#Ten").val();
        var CodeVa = $("#CodeVa").val();
        var Code = $("#Code").val();
        var MaxLengthVa = $("#MaxLengthVa").val();
        var Status = $("#Status").val();
        var formData = {
            Ma,
            Ten,
            CodeVa,
            MaxLengthVa,
            Status,
            Code
        }
        console.log("form data", formData)
        var url = Ma !== defaultUID ? '/Bank/Edit' : '/Bank/Add';
      
        console.table(formData)

        $.ajax({
            url: url,
            type: 'post',
            data: formData,
            success: function (response) {
                if (response.success) {
                    swal.fire({
                        title: 'thành công!',
                        text: response.message,
                        icon: 'success'
                    }).then(() => {
                        window.location.href = "/Bank";
                    });
                } else {
                    swal.fire({
                        title: 'thất bại!',
                        text: response.message,
                        icon: 'error'
                    });
                }
            },
            error: function (xhr, status, error) {
                swal.fire({
                    title: 'đã xảy ra lỗi!',
                    text: 'vui lòng thử lại.',
                    icon: 'error'
                });
                console.error(error);
            }
        });
    });


}

