let TableChiTietBangKe;
let TableChiTietNhanVien;
$(document).ready(function () {
    loadChiNhanh();
    loadDanhSach();
    ConfigTable();
    loadChiTietBangKe();
});
function ConfigTable() {
    TableChiTietBangKe = $('#TableChiTietBangKe').DataTable({
        columnDefs: [
            { className: "d-none", targets: 0, orderable: false },
            { width: '170px', className: 'dt-right dt-head-center', targets: [1, 2, 3], orderable: false },
            { width: '170px', className: 'dt-left dt-head-center', targets: [4], orderable: false },
        ],
        sort:false,
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

    TableChiTietNhanVien = $('#TableChiTietNhanVien').DataTable({
        columnDefs: [
            { className: "d-none", targets: 0, orderable: false },
            { width: '170px', className: 'dt-left dt-head-center', targets: [1, 2, 3], orderable: false },
        ],
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
                    value: item.maPhongBan ,
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
    console.table(data)
    TableChiTietBangKe.clear().draw();
    data.forEach(function (item) {
            let rowContent = [
                `<td>${item.ma}</td>`,
                `<td>${item.giaTri}</td>`,
                `<td>0</td>`,     
                `<td>0</td>`,      
                `<td></td>`     
            ];
            
            TableChiTietBangKe.row.add(rowContent).draw();
        }
    );
}
