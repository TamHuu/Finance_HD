let data
$(document).ready(function () {
    loadDanhSach();
    NganHang();
});

function loadDanhSach() {
    $('#btnSave').on('click', function (e) {
        e.preventDefault();

        var Ma = $("#Ma").val();
        var NganHang = $("#Bank").val();
        var Code = $("#Code").val();
        var Ten = $("#Ten").val();
        var Status = $("#Status").val();
        var formData = {
            Ma,
            Ten,
            NganHang,
            Status,
            Code
        }
        console.log("form data", formData)
        var url = Ma !== defaultUID ? '/BankAccountType/Edit' : '/BankAccountType/Add';
      
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
                        window.location.href = "/BankAccountType";
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
                console.log("Lỗi khi lấy dữ liệu ");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách:', error);
        }
    );
}
function selectNganHang(data) {
    var InitialSelectNganHang = $("#Bank");
    InitialSelectNganHang.empty();

    const defaultOption = $('<option>', {
        value: '',
        text: "Chọn...",
        selected: true
    });
    InitialSelectNganHang.append(defaultOption);
    if (data.length > 0) {
        data.forEach(function (branch) {
            const option = $('<option>', {
                value: branch.ma,
                text: branch.ten
            });
            if (branch.ma == maNganHang) {
                select.attr('selected', true);
            }
            InitialSelectNganHang.append(option);
        });
    }

  
}
