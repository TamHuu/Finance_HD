var TableChiTietBangKe;
var TableChiTietNhanVien
var DataNhanVien = [];
var totalSum = 0;
$(document).ready(function () {
    fetchMonetaryList();
    fetchUserList();
    fetchBranchList();
    sendFormData();
    ConfigTable();
    getCashDepositById();
    initialize();
});
function ConfigTable() {
    TableChiTietBangKe = $('#TableChiTietBangKe').DataTable({
        columnDefs: [
            { className: "d-none", targets: [0, 1], orderable: false },
            { width: '170px', className: 'dt-right dt-head-center', targets: [2, 3, 4], orderable: false },
            { width: '170px', className: 'dt-left dt-head-center', targets: [5], orderable: false },
            { targets: [3, 5], createdCell: function (td) { $(td).attr('contenteditable', true); } }
        ],
        ordering: false,
        searching: false,
        paging: false,
        lengthChange: false,
        info: false,
        language: {
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
        },

    });
    TableChiTietNhanVien = $('#TableChiTietNhanVien').DataTable({
        columnDefs: [
            { className: "d-none", targets: 3, orderable: false },
            { width: '300px', className: 'dt-left dt-head-center', targets: [0,1], orderable: false },
            { width: '100px', className: 'dt-left dt-head-center', targets: [2], orderable: false },
        ],
        searching: false,
        lengthChange: false,
        language: {
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
        }
    });
}



// Tiền tệ (VND)
function fetchMonetaryList() {

    callAPI('GET', '/Monetary/getListMonetary', null,
        function (response) {
            if (response.success) {
                MonetarySelect(response.data);
            } else {
                console.log("Lỗi khi lấy dữ liệu tiền tệ");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách tiền tệ:', error);
        }
    );
}

function MonetarySelect(data) {
    const monetarySelect = $('#TienTe');
    monetarySelect.empty();
    console.log("Mã tiền tệ", MaTienTe)
    // Sử dụng giá trị MaTienTe nếu đang ở chế độ chỉnh sửa
    var maTienTe = isEdit ? MaTienTe : '0febf710-436d-40cc-95e5-e457605cd104'; // Mặc định VND hoặc giá trị từ Model
    data.forEach(function (monetary) {
        const option = $('<option>', {
            value: monetary.ma,
            text: monetary.ten,
            selected: monetary.ma === maTienTe || monetary.code === "VND",
        });
        monetarySelect.append(option);
    });
    var selectedMaTienTe = monetarySelect.val();

    if (!isEdit) {
    TableBangKe(selectedMaTienTe);
    }
}
$("#TienTe").on('change', function () {
    var maTienTeChange = $("#TienTe").val();
    TableBangKe(maTienTeChange);
})
// Người nộp tiền
function fetchUserList() {

    callAPI('GET', '/User/getListUser', null,
        function (response) {
            if (response.success) {
                UserSelect(response.data);
            } else {
                console.log("Lỗi khi lấy dữ liệu tiền tệ");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách tiền tệ:', error);
        }
    );
}

function UserSelect(data) {
    const userSelect = $('#NguoiNopTien');
    userSelect.empty();
    console.log("Mã tiền tệ", MaNguoiNopTien)
    var maNguoiNopTien = isEdit ? MaNguoiNopTien : MaNguoiDangNhap;
    data.forEach(function (user) {
        const option = $('<option>', {
            value: user.ma,
            text: user.fullName,
            selected: user.ma === maNguoiNopTien,
        });
        userSelect.append(option);
    });
}


// Chi nhánh
function fetchBranchList() {
    callAPI('GET', '/Branch/getListBranch', null,
        function (response) {
            if (response.success) {
                DonViNopSelect(response.data);
                DonViNhanSelect(response.data);

                // Gọi lại để cập nhật phòng ban nộp và nhận
                var maDonViNop = $('#DonViNop').val();
                BoPhanNopSelect(maDonViNop);
                var maDonViNhan = $('#DonViNhan').val();
                BoPhanNhanSelect(maDonViNhan);
            } else {
                console.log("Lỗi khi lấy dữ liệu chi nhánh");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách chi nhánh:', error);
        }
    );
}

function DonViNopSelect(data) {
    const BranchSelect = $('#DonViNop');
    BranchSelect.empty();
    var maDonViNop = isEdit ? MaDonViNop : MaChiNhanhDangNhap;
    data.forEach(function (branch) {
        const option = $('<option>', {
            value: branch.ma,
            text: branch.ten,
            selected: branch.ma === maDonViNop,
        });
        BranchSelect.append(option);
    });
}

function DonViNhanSelect(data) {
    const BranchSelect = $('#DonViNhan');
    BranchSelect.empty();

    var maDonViNhan = isEdit ? MaDonViNhan : '';

    if (!maDonViNhan) {
        const defaultOption = $('<option>', {
            value: '',
            text: 'Chọn...',
            selected: true
        });
        BranchSelect.append(defaultOption);
    }

    // Thêm các tùy chọn khác từ data
    data.forEach(function (branch) {
        const option = $('<option>', {
            value: branch.ma,
            text: branch.ten,
            selected: branch.ma === maDonViNhan,
        });
        BranchSelect.append(option);
    });
}


// Phòng ban
function BoPhanNopSelect(maDonViNop) {
    callAPI('GET', '/Department/getListDepartment', null,
        function (response) {
            if (response.success) {
                var result = response.data;
                var filterData = result.filter(x => x.maChiNhanh === maDonViNop);

                const DepartmentSelect = $('#BoPhanNop');
                DepartmentSelect.empty();

                filterData.forEach(function (department) {
                    const option = $('<option>', {
                        value: department.maPhongBan,
                        text: department.tenPhongBan,
                        selected: department.maPhongBan === MaPhongBanDangNhap,
                    });
                    DepartmentSelect.append(option);
                });
            } else {
                console.log("Lỗi khi lấy dữ liệu bộ phận nộp");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách bộ phận nộp:', error);
        }
    );
}
function BoPhanNhanSelect(maDonViNhan) {
    callAPI('GET', '/Department/getListDepartment', null,
        function (response) {
            if (response.success) {
                var result = response.data;
                const DepartmentSelect = $('#BoPhanNhan');
                DepartmentSelect.empty();

                if (!maDonViNhan) {
                    const defaultOption = $('<option>', {
                        value: '',
                        text: 'Chọn...',
                        selected: true
                    });
                    DepartmentSelect.append(defaultOption);
                }

                var filterData = result.filter(x => x.maChiNhanh === maDonViNhan);

                // Thêm các tùy chọn từ filterData
                filterData.forEach(function (department) {
                    const option = $('<option>', {
                        value: department.maPhongBan,
                        text: department.tenPhongBan,
                    });
                    DepartmentSelect.append(option);
                });
            } else {
                console.log("Lỗi khi lấy dữ liệu bộ phận nhận");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách bộ phận nhận:', error);
        }
    );
}

// Sự kiện onchange phòng ban theo chi nhánh
$("#DonViNop").on('change', function () {
    var maDonViNop = $(this).val();
    BoPhanNopSelect(maDonViNop);
});

$("#DonViNhan").on('change', function () {
    var maDonViNhan = $(this).val();
    BoPhanNhanSelect(maDonViNhan);
});

// Gửi dữ liệu sang controller
function sendFormData() {
    $("#btnSave").on('click', function () {
        var DataChiTietBangKe = [];
        TableChiTietBangKe.rows().every(function () {
            var row = $(this.node());

            var maLoaiTien = row.find('td:eq(0)').text();
            var loaiTien = parseFloat(row.find('td:eq(2)').text());
            var soLuong = parseInt(row.find('td:eq(3)').text());
            var thanhTien = parseInt(row.find('td:eq(4)').text());
            var ghiChu = row.find('td:eq(5)').text();

            var DataRow = { maLoaiTien, loaiTien, soLuong, thanhTien, ghiChu };
            DataChiTietBangKe.push(DataRow);
        });

        console.log("dữ liệu", DataChiTietBangKe);
        var Ma = $("#Ma").val();
        var NgayNopTien = $("#NgayNopTien").val();
        var NgayLap = $("#NgayLap").val();
        var MaHinhThuc = $("#HinhThuc").val();
        var MaTienTe = $("#TienTe").val();
        var NguoiNopTien = $("#NguoiNopTien").val();
        var MaChiNhanhNop = $("#DonViNop").val();
        var MaPhongBanNop = $("#BoPhanNop").val();
        var MaChiNhanhNhan = $("#DonViNhan").val();
        var MaPhongBanNhan = $("#BoPhanNhan").val();
        var KhachHang = $("#KhachHang").val();
        var MaNoiDung = $("#NoiDung").val();
        var GhiChu = $("#GhiChu").val();
        var DiaChi = $("#DiaChi").val();
        var TyGia = $("#TyGia").val();
        var SoTien = totalSum;
        var formData = {
            Ma,
            NgayNopTien,
            NgayLap,
            MaHinhThuc,
            MaTienTe,
            NguoiNopTien,
            MaChiNhanhNop,
            MaPhongBanNop,
            MaChiNhanhNhan,
            MaPhongBanNhan,
            KhachHang,
            MaNoiDung,
            GhiChu,
            DiaChi,
            DataChiTietBangKe,
            DataNhanVien,
            TyGia,
            SoTien,
        };
        callAPI('POST', '/CashDeposit/Add', formData,
            function (response) {
                if (response.success) {
                    showAlert('Thành công!', response.message, 'success', 'OK', null, function () {
                        window.location.href = "/CashDeposit"; // Chuyển trang sau khi nhấn OK
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
// Xử lý Chi tiết bảng kê
function TableBangKe(MaTienTe) {
    callAPI('GET', '/Currency/getListCurrency', null,
        function (response) {
            if (response.success) {
                var result = response.data;
                var filteredResult = result.filter(x => x.maTienTe === MaTienTe);

                // Làm sạch bảng trước khi thêm dữ liệu mới
                TableChiTietBangKe.clear().draw();

                if (filteredResult.length > 0) {
                    // Thêm các dòng dữ liệu mới vào bảng
                    filteredResult.forEach(function (item) {
                        let rowContent = [
                            `<td>${item.ma}</td>`,
                            `<td>${item.maTienTe}</td>`,
                            `<td>${addCommas(item.giaTri) ?? 0}</td>`,
                            `<td contenteditable="true">0</td>`,
                            `<td>0</td>`,
                            `<td contenteditable="true"></td>`,  // Ghi chú có thể chỉnh sửa
                        ];

                        TableChiTietBangKe.row.add(rowContent).draw();
                    });

                    handleTableRows(); // Gọi hàm xử lý hàng

                    // Cập nhật dòng tổng tiền ban đầu
                    var tenTienTeHienThi = filteredResult[0].tenTienTe;
                    updateTotalRow(tenTienTeHienThi);

                } else {
                    // Nếu không có kết quả
                    var tenTienTe = $("#TienTe option:selected").text();
                    updateTotalRow(tenTienTe);
                }
            } else {
                console.log("Lỗi khi lấy dữ liệu tiền tệ");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách tiền tệ:', error);
        }
    );
}

function updateTotalRow(tenTienTe) {
    var newRow = $('<tr></tr>');
    var newData1 = $(`<td colspan="2" style="text-align:center;"></td>`).text(`Tổng tiền (${tenTienTe})`);
    var newData3 = $('<td style="text-align:right;"></td>').text('0');
    var newData4 = $('<td></td>').text('');

    newRow.append(newData1);
    newRow.append(newData3);
    newRow.append(newData4);

    $('#TableChiTietBangKe tfoot').empty().append(newRow);
}

// Hàm xử lý sự kiện chỉnh sửa các hàng trong bảng
function handleTableRows() {
    TableChiTietBangKe.rows().every(function () {
        var row = $(this.node());
        var maBangKe = row.find('td:eq(0)').text();
        var maTienTe = row.find('td:eq(1)').text();
        var loaiTien = parseFloat(row.find('td:eq(2)').text().replace(/,/g, ''));
        var soLuong = parseInt(row.find('td:eq(3)').text().replace(/,/g, ''), 10);
        var thanhTien = loaiTien * soLuong;
        var ghiChu = row.find('td:eq(5)').text();

        row.find('td:eq(4)').text(thanhTien.toLocaleString());

        var allData = {
            maBangKe,
            maTienTe,
            loaiTien,
            soLuong,
            thanhTien,
            ghiChu
        };
        // Bắt sự kiện khi ô được chỉnh sửa cho các cột số lượng và thành tiền
        row.find('td:eq(3), td:eq(4)').on('input', function () {
            var updatedValue = $(this).text();
            var columnIndex = $(this).index();

            if (columnIndex === 3) { // Số lượng
                const newSoLuong = parseInt(updatedValue.replace(/,/g, ''), 10);
                if (!isNaN(newSoLuong) && newSoLuong !== soLuong) {
                    soLuong = newSoLuong;
                    thanhTien = loaiTien * soLuong;
                    row.find('td:eq(3)').text(soLuong.toLocaleString());
                    row.find('td:eq(4)').text(thanhTien.toLocaleString());

                    // Cập nhật đối tượng allData
                    allData.soLuong = soLuong;
                    allData.thanhTien = thanhTien;
                }

            } else if (columnIndex === 4) { // Thành tiền
                const newThanhTien = parseFloat(updatedValue.replace(/,/g, ''));
                if (!isNaN(newThanhTien) && newThanhTien !== thanhTien) {
                    thanhTien = newThanhTien;
                    allData.thanhTien = thanhTien;
                }
            }

            updateTotalSum();
        });

        row.find('td:eq(5)').on('blur', function () {
            ghiChu = $(this).text();
            allData.ghiChu = ghiChu;
        });

        // Ràng buộc sự kiện keydown cho ô số lượng và thành tiền
        row.find('td:eq(3), td:eq(4)').on('keydown', function (e) {
            if (!((e.key >= '0' && e.key <= '9') || e.key === 'Backspace' || e.key === 'Tab' || e.key === 'Enter' || e.key === 'Escape' || e.key === ',' || e.key === '.')) {
                e.preventDefault();
            }
        });
    });
}

function updateTotalSum() {
    TableChiTietBangKe.rows().every(function () {
        var row = $(this.node());
        var thanhTien = parseFloat(row.find('td:eq(4)').text().replace(/,/g, ''));
        if (!isNaN(thanhTien)) {
            totalSum += thanhTien;
        }
    });
    $('#TableChiTietBangKe tfoot td:eq(1)').text(totalSum.toLocaleString());
}



// Xủ lý bảng nhân viên

$("#btnSaveChiTietNhanVien").on('click', function () {
    // Lấy giá trị từ input
    var MaNhanVien = $("#MaNhanVien").val();
    var SoTien = $("#SoTienNhanVien").val();

    if (!SoTien) {
        showAlert('Lỗi!', 'Vui lòng nhập số tiền.', 'error');
        return;
    }

    var exists = DataNhanVien.some(function (data) {
        return data.MaNhanVien === MaNhanVien;
    });

    if (exists) {
        showAlert('Lỗi!', 'Nhân viên này đã tồn tại trong danh sách.', 'error');
        return;
    }

    var allData = { MaNhanVien, SoTien };
    DataNhanVien.push(allData);

    callAPI('GET', '/User/getListUser', null,
        function (response) {

            var result = response.data;
            var filteredData = result.filter(x => x.ma == MaNhanVien);
            filteredData.forEach(function (item) {
                let rowContent = [
                    `<td>${item.ma}</td>`,
                    `<td>${item.fullName}</td>`,
                    `<td>${SoTien}</td>`,
                    `<td><button class="btn btn-danger btn-sm btnDelete" data-ma="${item.ma}"><i class="fas fa-trash"></i></button></td>`
                ];
                TableChiTietNhanVien.row.add(rowContent).draw();
            });
            $("#SoTienNhanVien").val('');
        },
    );
});
$('#TableChiTietNhanVien tbody').on('click', '.btnDelete', function () {
    // Lấy mã nhân viên từ data attribute
    var maNhanVien = $(this).data('ma');

    // Xóa hàng khỏi DataNhanVien
    DataNhanVien = DataNhanVien.filter(function (data) {
        return data.MaNhanVien !== maNhanVien; // Giữ lại các nhân viên không phải là nhân viên bị xóa
    });

    // Xóa hàng khỏi bảng
    TableChiTietNhanVien.row($(this).parents('tr')).remove().draw();
});



function initialize(MaTienTe) {
    // Kiểm tra xem dữ liệu đã được lưu trong local storage hay chưa
    var storedData = localStorage.getItem('dataKey');
    if (storedData) {
        // Nếu có dữ liệu đã lưu, hãy khôi phục nó
        DataNhanVien = JSON.parse(storedData);
        // Tải lại bảng từ dữ liệu đã lưu
        loadTableFromData(DataNhanVien);
    } else if (isEdit) {
        getCashDepositById();
    } else {
        TableBangKe(MaTienTe);
        TableNhanVien(MaTienTe);
    }
}
function getCashDepositById() {
    callAPI('POST', '/CashDeposit/getCashDepositById', { ma: maBangKe },
        function (response) {
            if (response.success) {
                var result = response.data;

                // Xóa dữ liệu cũ trong bảng Chi Tiết Bảng Kê
                TableChiTietBangKe.clear().draw();
                result.chiTietBangKe.forEach(function (item) {
                    let rowContent = [
                        item.ma,
                        item.maLoaiTien,
                        addCommas(item.tenLoaiTien),
                        item.soLuong,
                        addCommas(item.thanhTien),
                        item.ghiChu,
                    ];

                    // Thêm hàng mới vào bảng Chi Tiết Bảng Kê
                    TableChiTietBangKe.row.add(rowContent);
                });
                TableChiTietBangKe.draw();

                // Xóa dữ liệu cũ trong bảng Chi Tiết Nhân Viên
                TableChiTietNhanVien.clear().draw();
                result.nhanVien.forEach(function (item) {
                    let rowContent = [
                        item.maNhanVien,
                        item.tenNhanVien,
                        addCommas(item.soTien),
                        ""
                    ];
                    
                    TableChiTietNhanVien.row.add(rowContent);
                });
                TableChiTietNhanVien.draw(); 

            } else {
                // Xử lý lỗi nếu không thành công
                console.error("Lỗi khi lấy dữ liệu:", response.message);
            }
        },
        function (error) {
            // Xử lý lỗi mạng hoặc các lỗi khác
            console.error("Lỗi mạng:", error);
        }
    );
}
