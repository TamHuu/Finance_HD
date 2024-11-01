let data
$(document).ready(function () {
    eventHandler();
    sendFormData();
});

function eventHandler() {
    DonViNhan();
    loadNganHang();
    loadTienTe();
    loadLoaiTaiKhoanNganHang();
    loadDongTienThu();
    loadDongTienChi();
}

function DonViNhan() {
    callAPI('GET', '/Branch/getListBranch', null,
        function (response) {
            if (response.success) {
                selectDonViNhan(response.data);
            } else {
                console.log("Lỗi khi lấy dữ liệu tiền tệ");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách tiền tệ:', error);
        }
    );
}
function selectDonViNhan(data) {
    var InitialSelectDonViNhan = $("#Branch");
    data.forEach(function (branch) {
        const option = $('<option>', {
            value: branch.ma,
            text: branch.ten,
            selected: branch.ma === MaChiNhanh,
        });
        InitialSelectDonViNhan.append(option);
    });
    var maDonViNhan = $("#Branch").val();
    BoPhanNhan(maDonViNhan)
}
function BoPhanNhan(Ma) {
    callAPI('GET', '/Department/getListDepartment', null,
        function (response) {
            if (response.success) {
                var result = response.data;
                var filteredBoPhanNhan = result.filter(x => x.maChiNhanh == Ma)
                console.log("mã chi nhánh thay đổi", filteredBoPhanNhan)
                selectBoPhanNhan(filteredBoPhanNhan);
            } else {
                console.log("Lỗi khi lấy dữ liệu tiền tệ");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách tiền tệ:', error);
        }
    );
}
function selectBoPhanNhan(data) {
    var InitialSelectBoPhanNhan = $("#Department");
    InitialSelectBoPhanNhan.empty(); // Xóa tất cả các tùy chọn cũ
    data.forEach(function (department) {
        const option = $('<option>', {
            value: department.maPhongBan,
            text: department.tenPhongBan,
            selected: department.maPhongBan === MaPhongBan,
        });
        InitialSelectBoPhanNhan.append(option);
    });
}


$("#Branch").on('change', function () {
    var maDonViNopChange = $(this).val();
    console.log("mã đơn vị nộp change", maDonViNopChange);
    BoPhanNhan(maDonViNopChange);
});



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
    data.forEach(function (bank) {
        const optionValue = isEdit ? maNganHang : bank.ma; 
        const option = $('<option>', {
            value: optionValue,
            text: bank.ten,
            selected: isEdit && bank.ma === maNganHang
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
            selected: isEdit && tiente.ma === maTienTe
        });
        InitialSelectTienTe.append(option);
    });
}

function loadLoaiTaiKhoanNganHang() {
    callAPI('GET', '/BankAccountType/getListBankAccountType', null,
        function (response) {
            if (response.success) {
                var result = response.data;
                selectLoaiTaiKhoanNganHang(result);
            } else {
                console.log("Lỗi khi lấy dữ liệu ");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách:', error);
        }
    );
}
function selectLoaiTaiKhoanNganHang(data) {
    var InitialSelectNganHang = $("#LoaiTaiKhoan");
    InitialSelectNganHang.empty();

    const defaultOption = $('<option>', {
        value: "",
        text: "Chọn loại tài khoản ngân hàng",
        selected: true,
        disabled: true
    });
    InitialSelectNganHang.append(defaultOption);

    // Thêm các options từ dữ liệu
    data.forEach(function (account) {
        const option = $('<option>', {
            value: account.ma,
            text: account.ten,
            selected: isEdit && account.ma === maLoaiTaiKhoan
        });
        InitialSelectNganHang.append(option);
    });
}
function loadDongTienThu() {
    callAPI('GET', '/IncomeContent/getListIncomeContent', null,
        function (response) {
            if (response.success) {
                var result = response.data;
                selectDongTienThu(result);
            } else {
                console.log("Lỗi khi lấy dữ liệu ");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách:', error);
        }
    );
}
function selectDongTienThu(data) {
    var InitialSelectDongTienThu = $("#DongTienThu");
    InitialSelectDongTienThu.empty();

    const defaultOption = $('<option>', {
        value: "",
        text: "Chọn",
        selected: true,
        disabled: true
    });
    InitialSelectDongTienThu.append(defaultOption);

    // Thêm các options từ dữ liệu
    data.forEach(function (money) {
        const option = $('<option>', {
            value: money.ma,
            text: money.ten,
            selected: isEdit && money.ma === maDongTienThu
        });
        InitialSelectDongTienThu.append(option);
    });
}
function loadDongTienChi() {
    callAPI('GET', '/IncomeContent/getListIncomeContent', null,
        function (response) {
            if (response.success) {
                var result = response.data;
                selectDongTienChi(result);
            } else {
                console.log("Lỗi khi lấy dữ liệu ");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách:', error);
        }
    );
}
function selectDongTienChi(data) {
    var InitialSelectDongTienChi = $("#DongTienChi");
    InitialSelectDongTienChi.empty();

    const defaultOption = $('<option>', {
        value: "",
        text: "Chọn",
        selected: true,
        disabled: true
    });
    InitialSelectDongTienChi.append(defaultOption);

    // Thêm các options từ dữ liệu
    data.forEach(function (money) {
        const option = $('<option>', {
            value: money.ma,
            text: money.ten,
            selected: isEdit && money.ma === maDongTienChi
        });
        InitialSelectDongTienChi.append(option);
    });
}
function sendFormData() {
    $('#btnSave').on('click', function (e) {
        e.preventDefault();

        var Ma = $("#Ma").val();
        var ChiNhanh = $("#Branch").val();
        var PhongBan = $("#Department").val();
        var NganHang = $("#Bank").val();
        var TienTe = $("#TienTe").val();
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

