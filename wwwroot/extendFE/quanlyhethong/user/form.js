﻿let data
$(document).ready(function () {
    loadChiNhanh();
    loadDanhSach();
});

function loadDanhSach() {
    $('#btnSave').on('click', function (e) {
        e.preventDefault();
        var chucDanh = $('#ChucDanh').val();
        var chucVu = $('#ChucVu').val();
        var ma = $('#Ma').val();
        var username = $('#Username').val();
        var msnv = $('#Msnv').val();
        var maDinhDanh = $('#MaDinhDanh').val();
        var branch = $('#Branch').val();
        var cccd = $('#CCCD').val();
        var fullName = $('#FullName').val();
        var diaChi = $('#DiaChi').val();
        var department = $('#Department').val();
        var gioiTinh = $('#GioiTinh').val();
        var ngaySinh = $('#NgaySinh').val();
        var ngayVaoLam = $('#NgayVaoLam').val();
        var soDienThoai = $('#SoDienThoai').val();
        var ngayKetThuc = $('#NgayKetThuc').val();
        var MaPhongBan = $('#Department').val();
        var Password = $('#Password').val();
        var Status = $('#Status').val();
        
        var url = ma !== defaultUID ? '/User/Edit' : '/User/Add';
        var formdata = {
            Ma: ma,
            Username: username,
            Msnv: msnv,
            MaDinhDanh: maDinhDanh,
            BranchId: branch,
            MaPhongBan: MaPhongBan,
            Password: Password,
            CCCD: cccd,
            FullName: fullName,
            DiaChi: diaChi,
            Department: department,
            GioiTinh: gioiTinh,
            NgaySinh: ngaySinh,
            NgayVaoLam: ngayVaoLam,
            SoDienThoai: soDienThoai,
            NgayKetThuc: ngayKetThuc,
            Status: Status,
        };

        console.table(formdata)

        $.ajax({
            url: url,
            type: 'POST',
            data: formdata,
            success: function (response) {
                if (response.success) {
                    swal.fire({
                        title: 'thành công!',
                        text: response.message,
                        icon: 'success'
                    }).then(() => {
                        window.location.href = "/User";
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

function loadChiNhanh() {
    $.ajax({
        url: "/Branch/getListBranch",
        type: 'GET',
        success: function (response) {
            var branchData = response.data;
            getListChiNhanh(branchData);
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

function getListChiNhanh(data) {
    var branchSelect = $('#Branch');
    branchSelect.empty();
    data.forEach(function (branch) {
        let select = $('<option>', {
            value: branch.ma,
            text: branch.ten,
        });

        if (branch.ma == maCN) {
            select.attr('selected', true);
        }
        branchSelect.append(select);
    });

    var selectedBranch = branchSelect.val();
    loadBan(selectedBranch);
    changeSelectBranch(branchSelect);
}

function changeSelectBranch(branchSelect) {
    branchSelect.on('change', function () {
        var selectedBranch = $(this).val();
        console.log("Chi nhánh đã chọn:", selectedBranch);
        loadBan(selectedBranch);
    });
}

function loadBan(selectedBranch) {
    $.ajax({
        url: "/Department/getListDepartment",
        type: 'GET',
        success: function (response) {
            var DepartmentData = response.data;
            console.log("list ban", DepartmentData)
            var DepartmentSelect = $('#Department');
            DepartmentSelect.empty();
            let listBan = DepartmentData.filter(item => item.maChiNhanh == selectedBranch);

            listBan.forEach(function (item) {
                let option = $('<option>', {
                    value: item.maPhongBan,
                    text: item.tenPhongBan,
                });

                if (item.maPhongBan === MaPhongBan) {
                    option.attr('selected', true);
                }

                DepartmentSelect.append(option);
            });

            if (DepartmentSelect.children().length === 0) {
                DepartmentSelect.append($('<option>', {
                    value: '',
                    text: 'Không có ban nào.',
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