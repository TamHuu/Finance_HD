let data
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
        var ChucDanh = $("#ChucDanh").val();
        var ChucVu = $("#ChucVu").val();
        
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
            ChucDanh: ChucDanh,
            ChucVu: ChucVu
        };

        console.log(formdata)

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
    callAPI('GET', '/Branch/getListBranch', null,
        function (response) {
            if (response.success) {
                selectBranch(response.data);
            } else {
                console.log("Lỗi khi lấy dữ liệu ");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách:', error);
        }
    );
}
function selectBranch(data) {
    var InitialSelectBranch = $("#Branch");
    InitialSelectBranch.empty();

    data.forEach(function (branch) {
        const option = $('<option>', {
            value: branch.ma,
            text: branch.ten
        });
        
        if (branch.ma == maChiNhanh) {
            option.attr('selected', true);
        }

        InitialSelectBranch.append(option);
    });
    
    var maBranch = InitialSelectBranch.val();
    Department(maBranch);
    InitialSelectBranch.on('change', function () {
        var selectedBranch = $(this).val();
        Department(selectedBranch);
    });
}

function Department(Ma) {
    callAPI('GET', '/Department/getListDepartment', null,
        function (response) {
            if (response.success) {
                var result = response.data;
                var filteredDepartment = result.filter(x => x.maChiNhanh == Ma)
                selectDepartment(filteredDepartment);
            } else {
                console.log("Lỗi khi lấy dữ liệu tiền tệ");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách tiền tệ:', error);
        }
    );
}

$("#Branch").on('change', function () {
    var maBranchChange = $(this).val();
    Department(maBranchChange);
});
function selectDepartment(data) {
    var InitialSelectDepartment = $("#Department");
    InitialSelectDepartment.empty();

    if (data.length === 0) {
        const option = $('<option>', {
            value: '',
            text: "Chọn...",
            selected: true
        });
        InitialSelectDepartment.append(option);
    } else {
        data.forEach(function (department) {
            const option = $('<option>', {
                value: department.maPhongBan,
                text: department.tenPhongBan,
                selected: department.maPhongBan,
            });
            InitialSelectDepartment.append(option);
        });
    }
}