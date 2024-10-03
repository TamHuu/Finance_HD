$(document).ready(function () {
    $('#btnSave').on('click', function (e) {
        e.preventDefault();

        var ma = $('#Ma').val();  
        var ten = $('#Name').val();         
        var code = $('#Code').val();        
        var maSoThue = $('#MaSoThue').val();
        var phapNhan = $('#PhapNhan').val(); 
        var diaChi = $('#DiaChi').val();   
     /*   var logo = $('#Logo')[0].files[0]; */  
        var coSoQuy = $('#CoSoQuy').val();
        var status = $('#Status').val();   
        var url = ma !== defaultUID ? '/Branch/Edit' : '/Branch/Add';

        var data = {
            Ma: ma,
            Ten: ten,
            Code: code,
            MaSoThue: maSoThue,
            PhapNhan: phapNhan,
            Status: status,
            DiaChi: diaChi,
            CoSoQuy: coSoQuy
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
                        window.location.href = "/Branch";
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
