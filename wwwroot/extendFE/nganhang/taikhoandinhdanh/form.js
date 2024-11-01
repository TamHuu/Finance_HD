let data
$(document).ready(function () {
    loadDanhSach();
    NganHang();
    TaiKhoanNganHang();
});

function loadDanhSach() {
    $('#btnSave').on('click', function (e) {
        e.preventDefault();

        var Ma = $("#Ma").val();
        var Code = $("#Code").val();
        var NganHang = $("#Bank").val();
        var TaiKhoan = $("#BankAccount").val();
        var Ten = $("#Ten").val();
        var Status = $("#Status").val();
        var formData = {
            Ma,
            Ten,
            NganHang,
            TaiKhoan,
            Status,
            Code
        }
        console.log("form data", formData)
        var url = Ma !== defaultUID ? '/IdentifiedAccount/Edit' : '/IdentifiedAccount/Add';
      
        console.table(formData)

        $.ajax({
            url: url,
            type: 'post',
            data: formData,
            success: function (response) {
                if (response.success) {
                    swal.fire({
                        title: 'thành công!',
                        text: response.message,
                        icon: 'success'
                    }).then(() => {
                        window.location.href = "/Bank";
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

function NganHang() {
    callAPI('GET', '/Bank/getListBank', null,
        function (response) {
            if (response.success) {
                selectNganHang(response.data);
            } else {
                console.log("Lỗi khi lấy dữ liệu tiền tệ");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách tiền tệ:', error);
        }
    );
}
function selectNganHang(data) {
    var InitialSelectNganHang = $("#Bank");
    data.forEach(function (branch) {
        const option = $('<option>', {
            value: branch.ma,
            text: branch.ten,
            selected: branch.ma === maNganHang,
        });
        InitialSelectNganHang.append(option);
    });
}

function TaiKhoanNganHang() {
    callAPI('GET', '/BankAccount/getListBankAccount', null,
        function (response) {
            if (response.success) {
                selectTaiKhoanNganHang(response.data);
            } else {
                console.log("Lỗi khi lấy dữ liệu tiền tệ");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách tiền tệ:', error);
        }
    );
}
function selectTaiKhoanNganHang(data) {
    var InitialSelectTaiKhoanNganHang = $("#BankAccount");
    data.forEach(function (branch) {
        const option = $('<option>', {
            value: branch.ma,
            text: `${branch.tenLoaiTaiKhoan} - ${branch.soTaiKhoan} - ${branch.tenTienTe}`,
            selected: branch.ma === maTaiKhoanNganHang,
        });
        InitialSelectTaiKhoanNganHang.append(option);
    });
}