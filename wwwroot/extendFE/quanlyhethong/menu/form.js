$(document).ready(function () {
    $('#btnSave').click(function () {
        let formData = {
            Ma: $('#Ma').val(),
            UsingFor: $('#cboUsingFor').val(),
            Code: $('#Code').val(),
            MenuCha: $('#MenuCha').val(),
            Name: $('#Name').val(),
            STT: $('#STT').val(),
            Url: $('#Url').val(),
            Icon: $('#Icon').val(),
            MenuCon: $('#MenuCon').val(),
            Status: $('#Status').val()
        };
        var url = isEdit ? '/Menu/Edit' : '/Menu/Add';
        console.log("dữ liệu gửi đi", formData);

        $.ajax({
            url: url,
            type: 'POST',
            data: formData,
            success: function (response) {
                if (response.success) {
                    swal.fire({
                        title: 'thành công!',
                        text: response.message,
                        icon: 'success'
                    }).then(() => {
                        window.location.href = "/Menu";
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
    MenuCha();
    MenuCon();
});
function MenuCha() {
    callAPI('GET', '/Menu/getListMenu', null,
        function (response) {
            if (response.success) {
                selectMenuCha(response.menus.parentMenus);
            } else {
                console.log("Lỗi khi lấy dữ liệu ");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách:', error);
        }
    );
}
function selectMenuCha(data) {
    var InitialSelectMenu = $("#MenuCha");
    InitialSelectMenu.empty();

    const defaultOption = $('<option>', {
        value: '',
        text: "Chọn...",
        selected: true
    });
    InitialSelectMenu.append(defaultOption);
    if (data.length > 0) {
        data.forEach(function (branch) {
            const option = $('<option>', {
                value: branch.ma,
                text: branch.ten
            });
            InitialSelectMenu.append(option);
        });
    }
}
function MenuCon() {
    callAPI('GET', '/Menu/getListMenu', null,
        function (response) {
            if (response.success) {
                selectMenuCon(response.menus.childMenus); // Gọi hàm selectMenuCon với dữ liệu nhận được
            } else {
                console.log("Lỗi khi lấy dữ liệu ");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách:', error);
        }
    );
}

function selectMenuCon(data) {
    var InitialSelectMenuCon = $("#MenuCon");
    InitialSelectMenuCon.empty(); // Xóa các tùy chọn cũ

    const defaultOption = $('<option>', {
        value: '',
        text: "Chọn...",
        selected: true // Đặt tùy chọn mặc định là "Chọn..."
    });
    InitialSelectMenuCon.append(defaultOption); // Thêm tùy chọn mặc định

    // Lặp qua từng menu trong data để lấy danh sách menu con
    if (data.length > 0) {
        data.forEach(function (branch) {
            const option = $('<option>', {
                value: branch.ma,
                text: branch.ten
            });
            InitialSelectMenuCon.append(option);
        });
    }
}
