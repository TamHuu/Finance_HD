let data
$(document).ready(function () {
    sendFormData();
    eventHandler();
});

function eventHandler() {
    loadChiNhanh();
    loadNganHang();
    loadTienTe();
}

function loadChiNhanh() {
    callAPI('GET', '/Branch/getListBranch', null,
        function (response) {
            if (response.success) {
                var result = response.data;
                var maChiNhanh = result.filter(x => x.Ma);
                loadPhongBan(maChiNhanh)
                selectChiNhanh(result);
            } else {
                console.log("Lỗi khi lấy dữ liệu ");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách:', error);
        }
    );
}
function selectChiNhanh(data) {
    var InitialSelectChiNhanh = $("#Branch");
    InitialSelectChiNhanh.empty();

    const defaultOption = $('<option>', {
        value: "",
        text: "Chọn chi nhánh",
        selected: true,
        disabled: true
    });
    InitialSelectChiNhanh.append(defaultOption);

    // Thêm các options từ dữ liệu
    data.forEach(function (branch) {
        const option = $('<option>', {
            value: branch.ma,
            text: branch.ten,
        });
        InitialSelectChiNhanh.append(option);
    });
}
function loadPhongBan(Ma) {
    callAPI('GET', '/Department/getListDepartment', null,
        function (response) {
            if (response.success) {
                var result = response.data;
                var filterPhongBanTheoChiNhanh = result.filter(x => x.maChiNhanh == Ma)
                selectPhongBan(filterPhongBanTheoChiNhanh);
            } else {
                console.log("Lỗi khi lấy dữ liệu ");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách:', error);
        }
    );
}
function selectPhongBan(data) {
    var InitialSelectPhongBan = $("#Department");
    InitialSelectPhongBan.empty();

    const defaultOption = $('<option>', {
        value: "",
        text: "Chọn phòng ban",
        selected: true,
        disabled: true
    });
    InitialSelectPhongBan.append(defaultOption);

    // Thêm các options từ dữ liệu
    data.forEach(function (department) {
        const option = $('<option>', {
            value: department.maPhongBan,
            text: department.tenPhongBan,
            selected: department.maPhongBan,
        });
        InitialSelectPhongBan.append(option);
    });
}

$("#Branch").on('change', function () {
    var maChiNhanhChange = $("#Branch").val();
    console.log("mã chi nhánh change", maChiNhanhChange)
    loadPhongBan(maChiNhanhChange)
})

function loadNganHang() {
    callAPI('GET', '/Bank/getListBank', null,
        function (response) {
            if (response.success) {
                var result = response.data;
                selectNganHang(result);
            } else {
                console.log("Lỗi khi lấy dữ liệu ");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách:', error);
        }
    );
}
function selectNganHang(data) {
    var InitialSelectNganHang = $("#Bank");
    InitialSelectNganHang.empty();

    const defaultOption = $('<option>', {
        value: "",
        text: "Chọn ngân hàng",
        selected: true,
        disabled: true
    });
    InitialSelectNganHang.append(defaultOption);

    // Thêm các options từ dữ liệu
    data.forEach(function (branch) {
        const option = $('<option>', {
            value: branch.ma,
            text: branch.ten,
        });
        InitialSelectNganHang.append(option);
    });
}

function loadTienTe() {
    callAPI('GET', '/Monetary/getListMonetary', null,
        function (response) {
            if (response.success) {
                var result = response.data;
                selectTienTe(result);
            } else {
                console.log("Lỗi khi lấy dữ liệu ");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách:', error);
        }
    );
}
function selectTienTe(data) {
    var InitialSelectTienTe = $("#TienTe");
    InitialSelectTienTe.empty();

    const defaultOption = $('<option>', {
        value: "",
        text: "Chọn tiền tệ",
        selected: true,
        disabled: true
    });
    InitialSelectTienTe.append(defaultOption);

    // Thêm các options từ dữ liệu
    data.forEach(function (tiente) {
        const option = $('<option>', {
            value: tiente.ma,
            text: tiente.ten,
        });
        InitialSelectTienTe.append(option);
    });
}
function sendFormData() {
    $('#btnSave').on('click', function (e) {
        e.preventDefault();

        var Ma = $("#Ma").val();
        var ChiNhanh = $("#Branch").val();
        var PhongBan = $("#Department").val();
        var NganHang = $("#Bank").val();
        var TienTe = $("#Montery").val();
        var SoTaiKhoan = $("#SoTaiKhoan").val();
        var DienGiai = $("#DienGiai").val();
        var DongTienThu = $("#DongTienThu").val();
        var DongTienChi = $("#DongTienChi").val();
        var LoaiTaiKhoan = $("#LoaiTaiKhoan").val();
        var Status = $("#Status").val();
        var formData = {
            Ma,
            ChiNhanh,
            PhongBan,
            NganHang,
            TienTe,
            SoTaiKhoan,
            DienGiai,
            DongTienThu,
            DongTienChi,
            LoaiTaiKhoan,
            Status,
        }
        console.log("form data", formData)
        var url = Ma !== defaultUID ? '/BankAccount/Edit' : '/BankAccount/Add';

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
                        window.location.href = "/BankAccount";
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

