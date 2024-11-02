$(document).ready(function () {
    loadChiNhanh();
    loadDanhSach();
});

function loadDanhSach() {
    $('#btnSave').on('click', function (e) {
        e.preventDefault();

        var ma = $("#Ma").val();
        var ngayLap = $("#dtpNgayLap").val();
        var ngayNhanTien = $("#dtpNgayNhanTien").val();
        var chiNhanhDeNghi = $("#ChiNhanhDeNghi").val();
        var phongBanDeNghi = $("#PhongBanDeNghi").val();
        var chiNhanhChi = $("#ChiNhanhChi").val();
        var phongBanChi = $("#PhongBanChi").val();
        var noiDung = $("#NoiDung").val();
        var tienTe = $("#TienTe").val();
        var tyGia = $("#TyGia").val();
        var soTien = $("#SoTien").val();
        var hinhThuc = $("#HinhThuc").val();
        var trangThai = $("#TrangThai").val();
        var ghiChu = $("#GhiChu").val();
        var url = ma !== defaultUID ? '/ExpenseRequest/Edit' : '/ExpenseRequest/Add';
        let formdata ={
            ma: ma,
            ngayLap: ngayLap,
            ngayNhanTien: ngayNhanTien,
            chiNhanhDeNghi: chiNhanhDeNghi,
            phongBanDeNghi: phongBanDeNghi,
            chiNhanhChi: chiNhanhChi,
            phongBanChi: phongBanChi,
            noiDung: noiDung,
            tienTe: tienTe,
            tyGia: tyGia,
            soTien: soTien,
            hinhThuc: hinhThuc,
            ghiChu: ghiChu,
            trangThai: trangThai,
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
                        window.location.href = "/ExpenseRequest";
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
            ChiNhanhDeNghi(branchData);
            ChiNhanhChi(branchData);
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

function ChiNhanhDeNghi(data) {
    var branchSelect = $('#ChiNhanhDeNghi');
    branchSelect.empty();
    data.forEach(function (branch) {
        let select = $('<option>', {
            value: branch.ma,
            text: branch.ten,
        });

        if (branch.ma == maChiNhanhDeNghi) {
            select.attr('selected', true);
        }
        branchSelect.append(select);
    });

    var selectedBranch = branchSelect.val();
    loadBan(selectedBranch, '#PhongBanDeNghi', MaPhongBanDeNghi);
    changeSelectBranch(branchSelect, '#PhongBanDeNghi');
}

function ChiNhanhChi(data) {
    var branchSelect = $('#ChiNhanhChi');
    branchSelect.empty();
    data.forEach(function (branch) {
        let select = $('<option>', {
            value: branch.ma,
            text: branch.ten,
        });

        if (branch.ma == maChiNhanhChi) {
            select.attr('selected', true);
        }
        branchSelect.append(select);
    });

    var selectedBranch = branchSelect.val();
    loadBan(selectedBranch, '#PhongBanChi', maPhongBanChi);
    changeSelectBranch(branchSelect, '#PhongBanChi');
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
$("#SoTien").on('input', function () {
    var soTien = $("#SoTien").val();
    formatCurrencyInput(soTien)
})
