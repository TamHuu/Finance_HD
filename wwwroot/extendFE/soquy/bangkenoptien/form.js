﻿$(document).ready(function () {
    loadChiNhanh();
    loadDanhSach();
});

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
        console.table(formdata);

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
        console.log("Chi nhánh đã chọn:", selectedBranch);
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