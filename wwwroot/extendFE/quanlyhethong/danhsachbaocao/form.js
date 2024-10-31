$(document).ready(function () {
    $('#btnSave').on('click', function (e) {
        e.preventDefault();

        var ma = $('#Ma').val();
        var name = $('#Name').val();
        var code = $('#Code').val();
        var menu = $('#Menu').val();
        var status = $('#Status').val();
        var url = ma !== defaultUID ? '/Report/Edit' : '/Report/Add';

        var data = {
            Ma: ma,
            Name: name,
            Code: code,
            Menu: menu,
            Status: status,
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
                        window.location.href = "/Report";
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
    MenuCha();

});

function MenuCha() {
    callAPI('GET', '/Menu/getListMenu', null,
        function (response) {
            if (response.success) {
                selectMenu(response.menus.childMenus);
            } else {
                console.log("Lỗi khi lấy dữ liệu ");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách:', error);
        }
    );
}

function selectMenu(data) {
    var InitialSelectMenu = $("#Menu");
    InitialSelectMenu.empty();

    if (data.length > 0) {
        data.forEach(function (branch) {
            const optionValue = isEdit ? maDanhSachBaoCao : branch.ma; // Kiểm tra `isEdit`
            const option = $('<option>', {
                value: optionValue,  // Gán giá trị dựa trên chế độ `isEdit`
                text: branch.ten,
                selected: isEdit && branch.ma === maDanhSachBaoCao // Đánh dấu `selected` nếu là chế độ edit và `ma` khớp với `maDanhSachBaoCao`
            });
            InitialSelectMenu.append(option);
        });
    }
}
