let data
$(document).ready(function () {
    loadChiNhanh();
    loadDanhSach();
});

function loadDanhSach() {
    $('#btnSave').on('click', function (e) {
        e.preventDefault();

        var ma = $('#Ma').val();
        var Branch = $('#Branch').val();
        var Department = $('#Department').val();
        var Code = $('#Code').val();
        var Ten = $('#Ten').val();
        var SoDienThoai = $('#SoDienThoai').val();
        var DiaChi = $('#DiaChi').val();
        var Status = $('#Status').val();
        var url = ma !== defaultUID ? '/Customer/Edit' : '/Customer/Add';

        var data = {
            Ma: ma,
            Ten: Ten,
            Code: Code,
            MaPhongBan: Department,
            MaChiNhanh: Branch,
            Status: Status,
            DiaChi: DiaChi,
            SoDienThoai: SoDienThoai,
        };

        console.table(data)

        $.ajax({
            url: url,
            type: 'post',
            data: data,
            success: function (response) {
                if (response.success) {
                    swal.fire({
                        title: 'thành công!',
                        text: response.message,
                        icon: 'success'
                    }).then(() => {
                        window.location.href = "/Customer";
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
            var DepartmentSelect = $('#Department');
            DepartmentSelect.empty();
            let listBan = DepartmentData.filter(item => item.maChiNhanh == selectedBranch);

            listBan.forEach(function (item) {
                let option = $('<option>', {
                    value: item.maPhongBan,
                    text: item.tenPhongBan,
                });

                if (item.maPhongBan === MaPB) {
                    option.attr('selected', true);
                }

                DepartmentSelect.append(option);
            });

            if (DepartmentSelect.children().length === 0) {
                DepartmentSelect.append($('<option>', {
                    value: '',
                    text: 'Không có phòng ban nào.',
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
