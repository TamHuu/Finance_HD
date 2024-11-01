var SaveDataBangKe = [];
var TongTien = 0;
var listDataNhanVien = []; // Khởi tạo mảng bên ngoài sự kiện click
$(document).ready(function () {
    ConfigTable();
    EventHandler();
    sendFormData();
});
function EventHandler() {
    DonViThu();
    DonViChi();
    NguoiThuTien();
    NhanVienNop();
    KhachHangNop();
    TienTe();
    MaDongTien();
    BangKeNopTien();
}
function ConfigTable() {
    // Định nghĩa ngôn ngữ dùng chung
    const commonLanguage = {
        "decimal": "",
        "emptyTable": "Không có dữ liệu trong bảng",
        "info": "Hiển thị _START_ đến _END_ trong tổng số _TOTAL_ mục",
        "infoEmpty": "Hiển thị 0 đến 0 trong tổng số 0 mục",
        "infoFiltered": "(đã lọc từ _MAX_ mục)",
        "lengthMenu": "Hiển thị _MENU_ mục",
        "loadingRecords": "Đang tải...",
        "processing": "Đang xử lý...",
        "search": "Tìm kiếm:",
        "searchPlaceholder": "Nhập từ khóa...",
        "zeroRecords": "Không tìm thấy kết quả nào",
        "paginate": {
            "first": "Đầu tiên",
            "last": "Cuối cùng",
            "next": "Kế tiếp",
            "previous": "Trước"
        }
    };

    TableChiTietBangKe = $('#TableChiTietBangKe').DataTable({
        columnDefs: [
            { className: "d-none", targets: [0, 1], orderable: false },
            { width: '200px', className: 'dt-right dt-head-center', targets: [2, 3, 4], orderable: false },
           
        ],
        searching: false,
        ordering: false,
        lengthChange: false,
        language: commonLanguage,
    });

  
}


function DonViThu() {
    callAPI('GET', '/Branch/getListBranch', null,
        function (response) {
            if (response.success) {
                selectDonViThu(response.data);
            } else {
                console.log("Lỗi khi lấy dữ liệu ");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách:', error);
        }
    );
}
function selectDonViThu(data) {
    var InitialSelectDonViThu = $("#DonViThu");
    InitialSelectDonViThu.empty(); 
    
    const defaultOption = $('<option>', {
        value: '',
        text: "Chọn...",
        selected: true
    });
    InitialSelectDonViThu.append(defaultOption);
    if (data.length > 0) {
        data.forEach(function (branch) {
            const option = $('<option>', {
                value: branch.ma,
                text: branch.ten
            });
            InitialSelectDonViThu.append(option);
        });
    }

    var maDonViThu = $("#DonViThu").val();
    BoPhanThu(maDonViThu);
}
function BoPhanThu(Ma) {
    callAPI('GET', '/Department/getListDepartment', null,
        function (response) {
            if (response.success) {
                var result = response.data;
                var filteredBoPhanThu = result.filter(x => x.maChiNhanh == Ma)
                selectBoPhanThu(filteredBoPhanThu);
            } else {
                console.log("Lỗi khi lấy dữ liệu tiền tệ");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách tiền tệ:', error);
        }
    );
}

$("#DonViThu").on('change', function () {
    var maDonViThuChange = $(this).val();
    BoPhanThu(maDonViThuChange);
});
function selectBoPhanThu(data) {
    var InitialSelectBoPhanThu = $("#BoPhanThu");
    InitialSelectBoPhanThu.empty();

    if (data.length === 0) {
        const option = $('<option>', {
            value: '',
            text: "Chọn...",
            selected: true
        });
        InitialSelectBoPhanThu.append(option);
    } else {
        data.forEach(function (department) {
            const option = $('<option>', {
                value: department.maPhongBan,
                text: department.tenPhongBan,
                selected: department.maPhongBan,
            });
            InitialSelectBoPhanThu.append(option);
        });
    }
}
function DonViChi() {
    callAPI('GET', '/Branch/getListBranch', null,
        function (response) {
            if (response.success) {
                selectDonViChi(response.data);
            } else {
                console.log("Lỗi khi lấy dữ liệu ");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách:', error);
        }
    );
}
function selectDonViChi(data) {
    var InitialSelectDonViChi = $("#DonViChi");
    InitialSelectDonViChi.empty();

    const defaultOption = $('<option>', {
        value: '',
        text: "Chọn...",
        selected: true
    });
    InitialSelectDonViChi.append(defaultOption);
    if (data.length > 0) {
        data.forEach(function (branch) {
            const option = $('<option>', {
                value: branch.ma,
                text: branch.ten
            });
            InitialSelectDonViChi.append(option);
        });
    }

    var maDonViChi = $("#DonViChi").val();
    BoPhanChi(maDonViChi);
}

function BoPhanChi(Ma) {
    callAPI('GET', '/Department/getListDepartment', null,
        function (response) {
            if (response.success) {
                var result = response.data;
                var filteredBoPhanThu = result.filter(x => x.maChiNhanh == Ma)
                selectBoPhanChi(filteredBoPhanThu);
            } else {
                console.log("Lỗi khi lấy dữ liệu tiền tệ");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách tiền tệ:', error);
        }
    );
}

$("#DonViChi").on('change', function () {
    var maDonViChiChange = $(this).val();
    BoPhanChi(maDonViChiChange);
});
function selectBoPhanChi(data) {
    var InitialSelectBoPhanChi = $("#BoPhanChi");
    InitialSelectBoPhanChi.empty();

    if (data.length === 0) {
        const option = $('<option>', {
            value: '',
            text: "Chọn...",
            selected: true
        });
        InitialSelectBoPhanChi.append(option);
    } else {
        data.forEach(function (department) {
            const option = $('<option>', {
                value: department.maPhongBan,
                text: department.tenPhongBan,
                selected: department.maPhongBan,
            });
            InitialSelectBoPhanChi.append(option);
        });
    }
}

function NguoiThuTien() {
    callAPI('GET', '/User/getListUser', null,
        function (response) {
            if (response.success) {
                selectNguoiThuTien(response.data);
            } else {
                console.log("Lỗi khi lấy dữ liệu ");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách:', error);
        }
    );
}
function selectNguoiThuTien(data) {
    var InitialSelectNguoiThuTien = $("#NguoiThuTien");
    InitialSelectNguoiThuTien.empty();

    const defaultOption = $('<option>', {
        value: '',
        text: "Chọn...",
        selected: true
    });
    InitialSelectNguoiThuTien.append(defaultOption);
    if (data.length > 0) {
        data.forEach(function (branch) {
            const option = $('<option>', {
                value: branch.ma,
                text: branch.fullName
            });
            InitialSelectNguoiThuTien.append(option);
        });
    }

   
}

function NhanVienNop() {
    callAPI('GET', '/User/getListUser', null,
        function (response) {
            if (response.success) {
                selectNhanVienNop(response.data);
            } else {
                console.log("Lỗi khi lấy dữ liệu ");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách:', error);
        }
    );
}
function selectNhanVienNop(data) {
    var InitialSelectNhanVienNop = $("#NhanVienNop");
    InitialSelectNhanVienNop.empty();

    const defaultOption = $('<option>', {
        value: '',
        text: "Chọn...",
        selected: true
    });
    InitialSelectNhanVienNop.append(defaultOption);
    if (data.length > 0) {
        data.forEach(function (branch) {
            const option = $('<option>', {
                value: branch.ma,
                text: branch.fullName
            });
            InitialSelectNhanVienNop.append(option);
        });
    }


}

function KhachHangNop() {
    callAPI('GET', '/Customer/getListCustomer', null,
        function (response) {
            if (response.success) {
                selectKhachHangNop(response.data);
            } else {
                console.log("Lỗi khi lấy dữ liệu ");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách:', error);
        }
    );
}
function selectKhachHangNop(data) {
    var InitialSelectKhachHangNop = $("#KhachHangNop");
    InitialSelectKhachHangNop.empty();

    const defaultOption = $('<option>', {
        value: '',
        text: "Chọn...",
        selected: true
    });
    InitialSelectKhachHangNop.append(defaultOption);
    if (data.length > 0) {
        data.forEach(function (branch) {
            const option = $('<option>', {
                value: branch.ma,
                text: branch.ten
            });
            InitialSelectKhachHangNop.append(option);
        });
    }


}

function TienTe() {
    callAPI('GET', '/Monetary/getListMonetary', null,
        function (response) {
            if (response.success) {
                selectTienTe(response.data);
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
        value: '',
        text: "Chọn...",
        selected: true
    });
    InitialSelectTienTe.append(defaultOption);
    if (data.length > 0) {
        data.forEach(function (branch) {
            const option = $('<option>', {
                value: branch.ma,
                text: branch.ten,
                selected: branch.ma ==='0febf710-436d-40cc-95e5-e457605cd104'
            });
            InitialSelectTienTe.append(option);
        });
    }


}

function MaDongTien() {
    callAPI('GET', '/IncomeContent/getListIncomeContent', null,
        function (response) {
            if (response.success) {
                selectMaDongTien(response.data);
            } else {
                console.log("Lỗi khi lấy dữ liệu ");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách:', error);
        }
    );
}
function selectMaDongTien(data) {
    var InitialSelectMaDongTien = $("#MaDongTien");
    InitialSelectMaDongTien.empty();

    const defaultOption = $('<option>', {
        value: '',
        text: "Chọn...",
        selected: true
    });
    InitialSelectMaDongTien.append(defaultOption);
    if (data.length > 0) {
        data.forEach(function (branch) {
            const option = $('<option>', {
                value: branch.ma,
                text: branch.ten
            });
            InitialSelectMaDongTien.append(option);
        });
    }


}
function BangKeNopTien() {
    callAPI('POST', '/CashDeposit/getDanhSachBangKe', null,
        function (response) {
            if (response.success) {
                selectBangKeNopTien(response.data);
            } else {
                console.log("Lỗi khi lấy dữ liệu ");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách:', error);
        }
    );
}
function selectBangKeNopTien(data) {
    var InitialSelectBangKe = $("#BangKe");
    InitialSelectBangKe.empty();

    const defaultOption = $('<option>', {
        value: '',
        text: "Chọn...",
        selected: true
    });
    InitialSelectBangKe.append(defaultOption);
    if (data.length > 0) {
        data.forEach(function (bangke) {
            const option = $('<option>', {
                value: bangke.ma,
                text: bangke.soPhieu,
            });
            InitialSelectBangKe.append(option);
        });
    }
    InitialSelectBangKe.on('change', function () {
        var maBangKe = $(this).val();
        if (maBangKe) {
            TableDetailBangKe(maBangKe); // Gọi hàm với mã bảng kê đã chọn
        }
    });
}
function TableDetailBangKe(maBangKe) {
    console.log("mã bảng kê", maBangKe);

    $('#TableChiTietBangKe').on('click', 'tr', function (e) {
        e.preventDefault();
        // Xử lý click trên hàng ở đây nếu cần
    });

    callAPI('POST', '/CashDeposit/getCashDepositById', { ma: maBangKe },
        function (response) {
            if (response.success) {
                var dataChiTietBangKe = response.data.chiTietBangKe;
                var dataBangKeNopTien = response.data.cashDeposit;
                var tenTienTe = dataBangKeNopTien[0].tenTienTe;
                var soTien = dataBangKeNopTien[0].soTien;
                TableChiTietBangKe.clear().draw();
                dataChiTietBangKe.forEach(function (item) {
                    let rowContent = [
                        `<td>${item.ma}</td>`,
                        `<td>${item.maLoaiTien}</td>`,
                        `<td>${item.tenLoaiTien}</td>`,
                        `<td>${addCommas(item.soLuong)}</td>`,
                        `<td>${addCommas(item.thanhTien)}</td>`,
                        `<td>${item.ghiChu}</td>`,
                    ];

                    TableChiTietBangKe.row.add(rowContent).draw();
                    $('#totalAmount').text(addCommas(soTien))
                    $('#currency').text(tenTienTe)
                    
                });
            } else {
                console.log("Lỗi khi lấy dữ liệu ");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách:', error);
        }
    );
}


// Gửi dữ liệu form
function sendFormData() {
    $("#btnSave").on('click', function () {
        var url = isEdit ? "/Receipt/Edit" : "/Receipt/Add";
        var formData = collectFormData();
        console.log("dữ liệu truyền đi", formData)
        callAPI('POST', url, formData,
            function (response) {
                if (response.success) {
                    showAlert('Thành công!', response.message, 'success', 'OK', null, function () {
                        window.location.href = "/Receipt"; // Redirect after clicking OK
                    });
                } else {
                    console.log("Lỗi khi lấy dữ liệu chi nhánh");
                }
            },
            function (xhr, status, error) {
                console.error('Lỗi khi lấy danh sách chi nhánh:', error);
            }
        );
    });
}
function collectFormData() {
    return {
        Ma: $("#Ma").val(),
        DonViThu: $("#DonViThu").val(),
        DonViChi: $("#DonViChi").val(),
        BoPhanThu: $("#BoPhanThu").val(),
        BoPhanChi: $("#BoPhanChi").val(),
        NgayLap: $("#NgayLap").val(),
        NguoiThuTien: $("#NguoiThuTien").val(),
        NhanVienNop: $("#NhanVienNop").val(),
        KhachHangNop: $("#KhachHangNop").val(),
        BangKe: $("#BangKe").val(),
        TienTe: $("#TienTe").val(),
        TyGia: $("#TyGia").val(),
        SoPhieuChi: $("#SoPhieuChi").val(),
        SoTien: $("#SoTien").val(),
        HinhThuc: $("#HinhThuc").val(),
        SoChungTu: $("#SoHoSoKemTheo").val(),
        GhiChu: $("#GhiChu").val(),
        MaDongTien: $("#MaDongTien").val(),

    };
}






