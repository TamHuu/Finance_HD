$(document).ready(function () {
    $('#btnSave').on('click', function (e) {
        e.preventDefault();

        var cateId = $('#CateId').val();
        var name = $('#Name').val();
        var seoTitle = $('#SeoTitle').val();
        var sort = $('#Sort').val();
        var status = $('#Status').val();

        var url = cateId && cateId !== "0" ? '/Admin/ProductComment/Edit' : '/Admin/ProductComment/Add';

        var data = {
            CateId: cateId,
            Name: name,
            SeoTitle: seoTitle,
            Sort: sort,
            Status: status
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
                        window.location.href = "/Admin/ProductComment";
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
