var TableChiTietBangKe;
$(document).ready(function () {
    fetchMonetaryList();
    fetchBranchList();
    sendFormData();
    ConfigTable();
});
function ConfigTable() {
        TableChiTietBangKe = $('#TableChiTietBangKe').DataTable({
            columnDefs: [
                { className: "d-none", targets: [0, 1], orderable: false },
                { width: '170px', className: 'dt-right dt-head-center', targets: [2, 3, 4], orderable: false },
                { width: '170px', className: 'dt-left dt-head-center', targets: [5], orderable: false },
                { targets: [ 3,5], createdCell: function (td) { $(td).attr('contenteditable', true); } }
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
            }
        });

        //TableChiTietNhanVien = $('#TableChiTietNhanVien').DataTable({
        //    columnDefs: [
        //        { width: '200px', className: 'dt-left dt-head-center', targets: [0], orderable: false },
        //        { width: '100px', className: 'dt-left dt-head-center', targets: [1], orderable: false },
        //        { width: '100px', className: 'dt-right dt-head-center', targets: [2], orderable: false },

        //    ],
        //    ordering: false,
        //    searching: false,
        //    lengthChange: false,
        //    sorting: false,
        //    paging: false,
        //    info: false,
        //    language: {
        //        "decimal": "",
        //        "emptyTable": "Không có dữ liệu trong bảng",
        //        "info": "Hiển thị _START_ đến _END_ trong tổng số _TOTAL_ mục",
        //        "infoEmpty": "Hiển thị 0 đến 0 trong tổng số 0 mục",
        //        "infoFiltered": "(đã lọc từ _MAX_ mục)",
        //        "lengthMenu": "Hiển thị _MENU_ mục",
        //        "loadingRecords": "Đang tải...",
        //        "processing": "Đang xử lý...",
        //        "search": "Tìm kiếm:",
        //        "searchPlaceholder": "Nhập từ khóa...",
        //        "zeroRecords": "Không tìm thấy kết quả nào",
        //        "paginate": {
        //            "first": "Đầu tiên",
        //            "last": "Cuối cùng",
        //            "next": "Kế tiếp",
        //            "previous": "Trước"
        //        }
        //    }
        //});
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
    data.forEach(function (monetary) {
        const option = $('<option>', {
            value: monetary.ma,
            text: monetary.ten,
            selected: monetary.ma === '0febf710-436d-40cc-95e5-e457605cd104' || monetary.code === "VND",
        });
        monetarySelect.append(option);
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
    data.forEach(function (branch) {
        const option = $('<option>', {
            value: branch.ma,
            text: branch.ten,
            selected: branch.ma === MaChiNhanhDangNhap,
        });
        BranchSelect.append(option);
    });
}

function DonViNhanSelect(data) {
    const BranchSelect = $('#DonViNhan');
    BranchSelect.empty();
    data.forEach(function (branch) {
        const option = $('<option>', {
            value: branch.ma,
            text: branch.ten,
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

                var filterData = result.filter(x => x.maChiNhanh === maDonViNhan);

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
        var Ma = $("#Ma").val();
        var NgayNopTien = $("#NgayNopTien").val();
        var NgayLap = $("#NgayLap").val();
        var MaHinhThuc = $("#HinhThuc").val();
        var MaTienTe = $("#TyGia").val();
        var NguoiNopTien = $("#NguoiNopTien").val();
        var MaChiNhanhNop = $("#DonViNop").val();
        var MaPhongBanNop = $("#BoPhanNop").val();
        var MaChiNhanhNhan = $("#DonViNhan").val();
        var MaPhongBanNhan = $("#BoPhanNhan").val();
        var KhachHang = $("#KhachHang").val();
        var MaNoiDung = $("#NoiDung").val();
        var GhiChu = $("#GhiChu").val();
        var DiaChi = $("#DiaChi").val();
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
        };
        console.table(formData);
        callAPI('POST', '/CashDeposit/Add', formData,
            function (response) {
                if (response.success) {
                    var result = response.data;
                    // Xử lý kết quả thành công ở đây nếu cần
                } else {
                    console.log("Lỗi khi lấy dữ liệu chi nhánh");
                }
            },
            function (xhr, status, error) {
                console.error('Lỗi khi lấy danh sách chi nhánh:', error);
            }
        );
    }); // Thêm dấu đóng cho sự kiện 'click'
} 
