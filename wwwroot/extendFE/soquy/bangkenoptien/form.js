var SaveDataBangKe = [];
var TongTien = 0;
var listDataNhanVien = []; // Khởi tạo mảng bên ngoài sự kiện click
$(document).ready(function () {
    ConfigTable();
    EventHandler();
    sendFormData();
});
function EventHandler() {
    TienTe();
    NguoiNopTien();
    NguoiNhanTien();
    DonViNop();
    DonViNhan();
    InitForm();
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

    TableBangKeNopTien = $('#TableBangKeNopTien').DataTable({
        columnDefs: [
            { className: "d-none", targets: [0, 1], orderable: false },
            { width: '100px', className: 'dt-right dt-head-center', targets: [2, 3, 4], orderable: false },
            { width: '200px', className: 'dt-left dt-head-center', targets: [5], orderable: false },
            { targets: [5], createdCell: function (td) { $(td).attr('contenteditable', true); } },
            {
                targets: [3],
                createdCell: function (td) {
                    $(td).attr('contenteditable', true);

                    if ($(td).index() === 3) {
                        $(td).on('keypress', function (e) {
                            if (!/[0-9]/.test(String.fromCharCode(e.which))) {
                                e.preventDefault();
                            }
                        });
                    }
                }
            }
        ],
        searching: false,
        ordering: false,
        lengthChange: false,
        language: commonLanguage,
    });

    TableChiTietBangKeNhanVien = $('#TableChiTietBangKeNhanVien').DataTable({
        columnDefs: [
            { className: "d-none", targets: 0, orderable: false },
            { width: '200px', className: 'dt-left dt-head-center', targets: [1], orderable: false },
            { width: '150px', className: 'dt-right dt-head-center', targets: [2], orderable: false },
            { width: '100px', className: 'dt-right dt-head-center', targets: [3], orderable: false },
        ],
        searching: false,
        lengthChange: false,
        language: commonLanguage,
    });
}

function InitForm() {
    if (isEdit) {
        getCashDepositById()
    } else {
        TableChiTietBangKeNopTien();
        TableBangKeNhanVien();
    }
}

function NguoiNhanTien() {
    callAPI('GET', '/User/getListUser', null,
        function (response) {
            if (response.success) {
                selectNguoiNhanTien(response.data);
            } else {
                console.log("Lỗi khi lấy dữ liệu tiền tệ");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách tiền tệ:', error);
        }
    );
}
function selectNguoiNhanTien(data) {
    var InitialSelectNguoiNhanTien = $("#NguoiNhanTien");
    data.forEach(function (user) {
        const option = $('<option>', {
            value: user.ma,
            text: user.fullName,
            selected: user.ma === MaNguoiNhanTien,
        });
        InitialSelectNguoiNhanTien.append(option);
    });
}
function TienTe() {
    callAPI('GET', '/Monetary/getListMonetary', null,
        function (response) {
            if (response.success) {
                selectTienTe(response.data);
                
            } else {
                console.log("Lỗi khi lấy dữ liệu tiền tệ");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách tiền tệ:', error);
        }
    );
}

function selectTienTe(data) {

    var InitialSelectTienTe = $("#TienTe");
    data.forEach(function (tiente) {
        const option = $('<option>', {
            value: tiente.ma,
            text: tiente.ten,
            selected: tiente.ma === MaTienTeVND || tiente.code === "VND",
        });
        InitialSelectTienTe.append(option);
    });
    if (!isEdit) {
    var maTienTeSelected = $("#TienTe").find("option:selected").val();
    TableChiTietBangKeNopTien(maTienTeSelected)
    }
    $("#TienTe").on('change', function () {
        var maTienTeChange = $("#TienTe").val();
        TableChiTietBangKeNopTien(maTienTeChange)
    })
}
function NguoiNopTien() {
    callAPI('GET', '/User/getListUser', null,
        function (response) {
            if (response.success) {
                selectNguoiNopTien(response.data);
            } else {
                console.log("Lỗi khi lấy dữ liệu tiền tệ");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách tiền tệ:', error);
        }
    );
}
function selectNguoiNopTien(data) {
    var InitialSelectNguoiNopTien = $("#NguoiNopTien");
    data.forEach(function (user) {
        const option = $('<option>', {
            value: user.ma,
            text: user.fullName,
            selected: user.ma === MaNguoiDangNhap,
        });
        InitialSelectNguoiNopTien.append(option);
    });

}

function DonViNop() {
    callAPI('GET', '/Branch/getListBranch', null,
        function (response) {
            if (response.success) {
                selectDonViNop(response.data);
            } else {
                console.log("Lỗi khi lấy dữ liệu tiền tệ");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách tiền tệ:', error);
        }
    );
}
function selectDonViNop(data) {
    var InitialSelectDonViNop = $("#DonViNop");
    data.forEach(function (branch) {
        const option = $('<option>', {
            value: branch.ma,
            text: branch.ten,
            selected: isEdit ? branch.ma === MaDonViNop : branch.ma === MaChiNhanhDangNhap,
        });
        InitialSelectDonViNop.append(option);
    });
    var maDonViNop = InitialSelectDonViNop.val();
    BoPhanNop(maDonViNop)
}
function BoPhanNop(Ma) {
    callAPI('GET', '/Department/getListDepartment', null,
        function (response) {
            if (response.success) {
                var result = response.data;
                var filteredBoPhanNop = result.filter(x => x.maChiNhanh == Ma)

                selectBoPhanNop(filteredBoPhanNop);
            } else {
                console.log("Lỗi khi lấy dữ liệu tiền tệ");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách tiền tệ:', error);
        }
    );
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
    var InitialSelectDonViNhan = $("#DonViNhan");
    data.forEach(function (branch) {
        const option = $('<option>', {
            value: branch.ma,
            text: branch.ten,
            selected: branch.ma === MaDonViNhan,
        });
        InitialSelectDonViNhan.append(option);
    });
    var maDonViNhan = $("#DonViNhan").val();
    BoPhanNhan(maDonViNhan)
}
function BoPhanNhan(Ma) {
    callAPI('GET', '/Department/getListDepartment', null,
        function (response) {
            if (response.success) {
                var result = response.data;
                var filteredBoPhanNhan = result.filter(x => x.maChiNhanh == Ma)
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
    var InitialSelectBoPhanNhan = $("#BoPhanNhan");
    data.forEach(function (department) {
        const option = $('<option>', {
            value: department.maPhongBan,
            text: department.tenPhongBan,
            selected: department.maPhongBan === MaBoPhanNop,
        });
        InitialSelectBoPhanNhan.append(option);
    });
}

$("#DonViNop").on('change', function () {
    var maDonViNopChange = $(this).val();
    console.log("mã đơn vị nộp change", maDonViNopChange);
    BoPhanNop(maDonViNopChange);
});
function selectBoPhanNop(data) {
    var InitialSelectBoPhanNop = $("#BoPhanNop");
    InitialSelectBoPhanNop.empty();

    if (data.length === 0) {
        const option = $('<option>', {
            value: '',
            text: "Không có bộ phận nộp",
            selected: true
        });
        InitialSelectBoPhanNop.append(option);
    } else {
        data.forEach(function (department) {
            const option = $('<option>', {
                value: department.maPhongBan,
                text: department.tenPhongBan,
                selected: isEdit ? department.maPhongBan === MaBoPhanNop : department.maPhongBan == MaPhongBanDangNhap,
            });
            InitialSelectBoPhanNop.append(option);
        });
    }
}

$("#DonViNhan").on('change', function () {
    var maDonViNhanChange = $(this).val();
    console.log("mã đơn vị nộp change", maDonViNhanChange);
    BoPhanNhan(maDonViNhanChange);
});
function selectBoPhanNhan(data) {
    var InitialSelectBoPhanNhan = $("#BoPhanNhan");
    InitialSelectBoPhanNhan.empty();

    if (data.length === 0) {
        const option = $('<option>', {
            value: '',
            text: "Không có bộ phận nhận",
            selected: true
        });
        InitialSelectBoPhanNhan.append(option);
    } else {
        data.forEach(function (department) {
            const option = $('<option>', {
                value: department.maPhongBan,
                text: department.tenPhongBan,
                selected: department.maPhongBan === MaBoPhanNhan,
            });
            InitialSelectBoPhanNhan.append(option);
        });
    }
}
function sendFormData() {
    $("#btnSave").on('click', function () {
        var url = isEdit ? "/CashDeposit/Edit" : "/CashDeposit/Add";
        updateThanhTien();

        const formData = collectFormData();
        console.log("dữ liệu truyền đi", formData);
        formData.SaveDataBangKe = SaveDataBangKe;
        formData.SaveDataNhanVien = listDataNhanVien;

        callAPI('POST', url, formData,
            function (response) {
                if (response.success) {
                    showAlert('Thành công!', response.message, 'success', 'OK', null, function () {
                        window.location.href = "/CashDeposit"; // Redirect after clicking OK
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
        MaChiNhanhNhan: $("#DonViNhan").val(),
        MaChiNhanhNop: $("#DonViNop").val(),
        MaPhongBanNhan: $("#BoPhanNhan").val(),
        MaPhongBanNop: $("#BoPhanNop").val(),
        NgayNopTien: $("#NgayNopTien").val(),
        NgayLap: $("#NgayLap").val(),
        MaHinhThuc: $("#MaHinhThuc").val(),
        MaTienTe: $("#TienTe").val(),
        TyGia: $("#TyGia").val(),
        NguoiNopTien: $("#NguoiNopTien").val(),
        NguoiNhanTien: $("#NguoiNhanTien").val(),
        MaNoiDung: $("#MaNoiDung").val(),
        GhiChu: $("#GhiChu").val(),
        DiaChi: $("#DiaChi").val(),
        SoTien: TongTien,
    };
}
function TableChiTietBangKeNopTien(MaTienTeChange) {
    callAPI('GET', '/Currency/getListCurrency', null,
        function (response) {
            if (response.success) {
                var result = response.data;
                var filterDataByTienTe = result.filter(x => x.maTienTe ==  MaTienTeChange);
                TableBangKeNopTien.clear().draw();

                filterDataByTienTe.forEach(function (item) {
                    let rowContent = [
                        `<td>${item.ma}</td>`,
                        `<td>${item.maTienTe}</td>`,
                        `<td contenteditable="true">${addCommas(item.giaTri) ?? 0}</td>`,
                        `<td>0</td>`,
                        `<td>0</td>`,
                        `<td></td>`,
                    ];

                    TableBangKeNopTien.row.add(rowContent).draw();
                });
                updateThanhTien();
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách tiền tệ:', error);
        }
    );
}
function updateThanhTien() {
    SaveDataBangKe = [];
    TongTien = 0;
    $("#TableBangKeNopTien tbody tr").each(function () {
        var maLoaiTien = $(this).find('td').eq(0).text();
        var maTienTe = $(this).find('td').eq(1).text();
        var loaiTien = parseFloat($(this).find('td').eq(2).text().replace(/,/g, '')) || 0;
        var soLuong = parseFloat($(this).find('td').eq(3).text().replace(/,/g, '')) || 0;
        var thanhTien = soLuong * loaiTien;

        $(this).find('td').eq(3).text(addCommas(soLuong));
        $(this).find('td').eq(4).text(addCommas(thanhTien));

        var ghiChu = $(this).find('td').eq(5).text();
        var listData = { maLoaiTien, maTienTe, soLuong, loaiTien, thanhTien, ghiChu };
        SaveDataBangKe.push(listData);
        TongTien += thanhTien;
        var tenTienTeSelected = $("#TienTe").find("option:selected").text();
        $("#totalAmount").text(addCommas(TongTien));
        $("#currency").text(tenTienTeSelected);
    });
}

$("#TableBangKeNopTien").on('input', 'tbody tr td:nth-child(4)', function () {
    updateThanhTien();
});

$("#btnSaveChiTietNhanVien").on('click', function () {
    var maNhanVien = $("#MaNhanVien").val();
    var soTien = $("#SoTienNhanVien").val();

    if (!maNhanVien || !soTien) {
        showAlert('Thất bại!','Vui lòng chọn nhân viên và nhập số tiền.','error','OK');
        return;
    }
    var exists = listDataNhanVien.some(function (item) {
        return item.maNhanVien === maNhanVien;
    });

    if (exists) {
        showAlert('Thất bại!', 'Mã nhân viên này đã tồn tại trong danh sách.', 'error', 'OK');
        return;
    }
    var form = { maNhanVien, soTien };
    listDataNhanVien.push(form);
    
    TableBangKeNhanVien(listDataNhanVien);
});

function TableBangKeNhanVien() {
    if (!Array.isArray(listDataNhanVien)) {
        return; 
    }

    callAPI('GET', '/User/getListUser', null, function (response) {
        if (response.success) {
            var result = response.data;
            TableChiTietBangKeNhanVien.clear();

            listDataNhanVien.forEach(function (data) {
                var filterUser = result.filter(x => x.ma == data.maNhanVien);

                if (filterUser.length === 0) {
                    console.log('Không có dữ liệu cho nhân viên này:', data.maNhanVien);
                } else {
                    filterUser.forEach(function (item) {
                        let rowContent = [
                            `<td>${item.ma}</td>`,
                            `<td>${item.fullName}</td>`,
                            `<td>${data.soTien}</td>`,
                            `<td><button class="btn btn-danger btn-sm btnDelete" data-ma="${item.ma}"><i class="fas fa-trash"></i></button></td>`
                        ];
                        TableChiTietBangKeNhanVien.row.add(rowContent);
                    });
                }
            });
        }

        TableChiTietBangKeNhanVien.draw();
    });
}


function getCashDepositById() {
    callAPI('POST', '/CashDeposit/getCashDepositById', { ma: maBangKe },
        function (response) {
            if (response.success) {
                var result = response.data;
                var dataChiTietBangKe = result.chiTietBangKe || [];
                var dataChitietNhanVien = result.nhanVien || [];

                TableBangKeNopTien.clear().draw();
                dataChiTietBangKe.forEach(function (item) {
                    let rowContent = [
                        `<td>${item.ma}</td>`,
                        `<td>${item.maLoaiTien}</td>`,
                        `<td>${item.tenLoaiTien ?? 0}</td>`,
                        `<td>${addCommas(item.soLuong)}</td>`,
                        `<td>${addCommas(item.thanhTien)}</td>`,
                        `<td>${item.ghiChu}</td>`
                    ];
                    TableBangKeNopTien.row.add(rowContent).draw();
                });
                updateThanhTien();

                TableChiTietBangKeNhanVien.clear().draw();
                dataChitietNhanVien.forEach(function (item) {
                    let rowContentNhanVien = [
                        `<td>${item.maNhanVien}</td>`,
                        `<td>${item.tenNhanVien}</td>`,
                        `<td>${addCommas(item.soTien)}</td>`,
                        `<td><button class="btn btn-danger btn-sm btnDelete" data-ma="${item.maNhanVien}"><i class="fas fa-trash"></i></button></td>`
                    ];
                    TableChiTietBangKeNhanVien.row.add(rowContentNhanVien).draw();
                });
            } else {
                console.log("Không có dữ liệu cho bảng nhân viên.");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách tiền tệ:', error);
        }
    );
}


