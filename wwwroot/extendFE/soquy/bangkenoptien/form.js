let TableChiTietBangKe;
let TableChiTietNhanVien;
$(document).ready(function () {
    loadChiNhanh();
    loadDanhSach();
    ConfigTable();
    loadChiTietBangKe();
    TableChiTietBangKe.on('draw', function () {
        $('#TableChiTietBangKe tbody tr td:nth-child(3)')
            .attr('contenteditable', 'true')
            .addClass('editable')
            .on('keypress', function (e) {
                let charCode = e.which ? e.which : e.keyCode;
                if (charCode === 8 || charCode === 9 || charCode === 46) {
                    return true;
                }

                if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                    return false;
                }
            })
            .on('input', function () {
                let currentRow = $(this).closest('tr');
                let columnLoaiTien = parseFloat(currentRow.find('td:nth-child(2)').text()) || 0;
                let columnSoLuong = parseFloat($(this).text()) || 0;
                let columnThanhTien = columnLoaiTien * columnSoLuong;

                currentRow.find('td:nth-child(4)').text(addCommas(columnThanhTien));
            });

        $('#TableChiTietBangKe tbody tr td:nth-child(5)')
            .attr('contenteditable', 'true')
            .addClass('editable')
            .on('input', function () {
                let enteredText = $(this).text();
                console.log('Giá trị ô thứ 5:', enteredText);
            });
    });

});
function ConfigTable() {
    TableChiTietBangKe = $('#TableChiTietBangKe').DataTable({
        columnDefs: [
            { className: "d-none", targets: 0, orderable: false },
            { width: '170px', className: 'dt-right dt-head-center', targets: [1, 2, 3], orderable: false },
            { width: '170px', className: 'dt-left dt-head-center', targets: [4], orderable: false },
        ],
        searching: false,
        sort: false,
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

    TableChiTietNhanVien = $('#TableChiTietNhanVien').DataTable({
        columnDefs: [
            { width: '250px', className: 'dt-left dt-head-center', targets: [0], orderable: false },
            { width: '150px', className: 'dt-left dt-head-center', targets: [1], orderable: false },
            { width: '100px', className: 'dt-right dt-head-center', targets: [2], orderable: false },
        ],
        searching: false,
        lengthChange: false,
        sort: false,
        paging: false,
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
}

function loadDanhSach() {
    $('#btnSave').on('click', function (e) {
        e.preventDefault();

        var ma = $('#Ma').val();
        var ngayNopTien = $('#dtpNgayNopTien').val();
        var ngayLap = $('#dtpNgayLap').val();
        var hinhThuc = $('#HinhThuc').val();
        var tienTe = $('#TienTe').val();
        var tyGia = $('#TyGia').val();
        var nguoiNopTien = $('#NguoiNopTien').val();
        var donViNop = $('#DonViNop').val();
        var boPhanNop = $('#BoPhanNop').val();
        var donViNhan = $('#DonViNhan').val();
        var boPhanNhan = $('#BoPhanNhan').val();
        var tenNguoiNopTien = $('#TenNguoiNopTien').val();
        var noiDung = $('#NoiDung').val();
        var ghiChu = $('#GhiChu').val();
        var diaChi = $('#DiaChi').val();
        var url = ma !== defaultUID ? '/CashDeposit/Edit' : '/CashDeposit/Add';
        let formdata = {
            ma: ma,
            ngayNopTien: ngayNopTien,
            ngayLap: ngayLap,
            hinhThuc: hinhThuc,
            tienTe: tienTe,
            tyGia: tyGia,
            nguoiNopTien: nguoiNopTien,
            donViNop: donViNop,
            boPhanNop: boPhanNop,
            donViNhan: donViNhan,
            boPhanNhan: boPhanNhan,
            tenNguoiNopTien: tenNguoiNopTien,
            noiDung: noiDung,
            ghiChu: ghiChu,
            diaChi: diaChi,
        };

        $.ajax({
            url: url,
            type: 'post',
            data: formdata,
            success: function (response) {
                if (response.success) {
                    swal.fire({
                        title: 'Thành công!',
                        text: response.message,
                        icon: 'success'
                    }).then(() => {
                        window.location.href = "/CashDeposit";
                    });
                } else {
                    swal.fire({
                        title: 'Thất bại!',
                        text: response.message,
                        icon: 'error'
                    });
                }
            },
            error: function (xhr, status, error) {
                swal.fire({
                    title: 'Đã xảy ra lỗi!',
                    text: 'Vui lòng thử lại.',
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
            DonViNop(branchData);
            DonViNhan(branchData);
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

function DonViNop(data) {
    var branchSelect = $('#DonViNop');
    branchSelect.empty();
    data.forEach(function (branch) {
        let select = $('<option>', {
            value: branch.ma,
            text: branch.ten,
        });

        if (branch.ma == MaDonViNop) {
            select.attr('selected', true);
        }
        branchSelect.append(select);
    });

    var selectedBranch = branchSelect.val();
    loadBan(selectedBranch, '#BoPhanNop', MaBoPhanNop);
    changeSelectBranch(branchSelect, '#BoPhanNop');
}

function DonViNhan(data) {
    var branchSelect = $('#DonViNhan');
    branchSelect.empty();
    data.forEach(function (branch) {
        let select = $('<option>', {
            value: branch.ma,
            text: branch.ten,
        });

        if (branch.ma == MaDonViNhan) {
            select.attr('selected', true);
        }
        branchSelect.append(select);
    });

    var selectedBranch = branchSelect.val();
    loadBan(selectedBranch, '#BoPhanNhan', MaBoPhanNhan);
    changeSelectBranch(branchSelect, '#BoPhanNhan');
}

function changeSelectBranch(branchSelect, DepartmentSelectId) {
    branchSelect.on('change', function () {
        var selectedBranch = $(this).val();
        loadBan(selectedBranch, DepartmentSelectId);
    });
}

function loadBan(selectedBranch, DepartmentSelectId, selectedDepartment = '') {
    $.ajax({
        url: "/Department/getListDepartment",
        type: 'GET',
        success: function (response) {
            var DepartmentData = response.data;
            var DepartmentSelect = $(DepartmentSelectId);
            DepartmentSelect.empty();
            let listBan = DepartmentData.filter(item => item.maChiNhanh == selectedBranch);

            listBan.forEach(function (item) {
                let option = $('<option>', {
                    value: item.maPhongBan,
                    text: item.tenPhongBan,
                });

                if (item.maPhongBan === selectedDepartment) {
                    option.attr('selected', true);
                }

                DepartmentSelect.append(option);
            });

            if (DepartmentSelect.children().length === 0) {
                DepartmentSelect.append($('<option>', {
                    value: '',
                    text: '',
                }));
            }
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
function loadChiTietBangKe() {
    $.ajax({
        url: "/Currency/getListCurrency",
        type: 'GET',
        success: function (response) {
            var result = response.data;
            ChiTietBangKe(result);
        },
        error: function (xhr, status, error) {
            swal.fire({
                title: 'Đã xảy ra lỗi!',
                text: 'Vui lòng thử lại.',
                icon: 'error'
            });
            console.error(error);
        }
    });
}

function ChiTietBangKe(data) {
    TableChiTietBangKe.clear();
    data.forEach(function (item) {
        let rowContent = [
            item.ma,
            item.giaTri,
            '0',
            '0',
            ''
        ];

        TableChiTietBangKe.row.add(rowContent);
    });

    TableChiTietBangKe.draw();
}
$('#btnSaveChiTietNhanVien').on('click', function (e) {
    e.preventDefault();
    ChiTietNhanVien();
});
function ChiTietNhanVien() {
    TableChiTietNhanVien.clear();
    var maNhanVien = $("#MaNhanVien").val();
    var sotien = $("#SoTienNhanVien").val();

    $.ajax({
        url: "/User/getListUser",
        type: 'GET',
        success: function (response) {
            var result = response.data;

            // Kiểm tra từng item trong result
            result.forEach(function (item) {
                if (item.ma == maNhanVien) {
                    let rowContent = [
                        item.ma,
                        item.fullName,
                        addCommas(sotien),

                    ];
                    TableChiTietNhanVien.row.add(rowContent);
                }
            });

            TableChiTietNhanVien.draw();
        },
        error: function (xhr, status, error) {
            swal.fire({
                title: 'Đã xảy ra lỗi!',
                text: 'Vui lòng thử lại.',
                icon: 'error'
            });
            console.error(error);
        }
    });
}



function addCommas(amount) {
    if (amount == null || isNaN(amount)) {
        return '0';
    }
    return amount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
}

