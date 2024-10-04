let data
$(document).ready(function () {
    loadChiNhanh();
    loadDanhSach();
});

function loadDanhSach() {
    $('#btnSave').on('click', function (e) {
        e.preventDefault();
       
        var ma = $('#Ma').val();
        var code = $('#Code').val();
        var branch = $('#Branch').val();
        var MaBan = $('#Division').val();
        var ten = $('#Ten').val();
        var ban = $('#Ban').is(':checked'); 
        var coSoQuy = $('#CoSoQuy').is(':checked'); 
        var status = $('#Status').val();
        var url = ma !== defaultUID ? '/Department/Edit' : '/Department/Add';
        var formdata = {
            Ma: ma,
            MaChiNhanh: branch,
            Code: code,
            Ten: ten,
            CoSoQuy: coSoQuy,
            MaBan: MaBan,
            Ban: ban,
            Status: status
        };
        console.table(formdata)

        $.ajax({
            url: url,
            type: 'post',
            data: formdata,
            success: function (response) {
                if (response.success) {
                    swal.fire({
                        title: 'thành công!',
                        text: response.message,
                        icon: 'success'
                    }).then(() => {
                        window.location.href = "/Department";
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

function loadChiNhanh() {
    $.ajax({
        url: "/Branch/getListBranch",
        type: 'GET',
        success: function (response) {
            var branchData = response.data;
            getListChiNhanh(branchData);
        },
        error: function (error) {
            Swal.fire({
                title: 'Đã xảy ra lỗi!',
                text: 'Vui lòng thử lại.',
                icon: 'error'
            });
            console.error('Lỗi:', error);
        }
    });
}

function getListChiNhanh(data) {
    var branchSelect = $('#Branch');
    branchSelect.empty();

    data.forEach(function (branch) {
        branchSelect.append($('<option>', {
            value: branch.ma,
            text: branch.ten
        }));
    });

    branchSelect.on('change', function () {
        var selectedBranch = $(this).val();
        loadBan(selectedBranch);
    });
}

function loadBan(selectedBranch) {
    $.ajax({
        url: "/Division/getListDivision", 
        type: 'GET',
        success: function (response) {
            var DivisionData = response.data;
            var DivisionSelect = $('#Division');
            DivisionSelect.empty();
            let listBan = DivisionData.filter(item => item.maChiNhanh == selectedBranch);
            listBan.forEach(function (item) {
                DivisionSelect.append($('<option>', {
                    value: item.ma,
                    text: item.ten
                }));
            });
        },
        error: function (error) {
            Swal.fire({
                title: 'Đã xảy ra lỗi!',
                text: 'Vui lòng thử lại.',
                icon: 'error'
            });
            console.error('Lỗi:', error);
        }
    });
}
