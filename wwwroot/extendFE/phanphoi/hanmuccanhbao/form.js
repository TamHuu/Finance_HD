$(document).ready(function () {

    sendFormData();
    BoPhanNop();
});
function BoPhanNop() {
    callAPI('GET', '/Department/getListDepartment', null,
        function (response) {
            if (response.success) {
                var result = response.data;
                selectBoPhanNop(result);
            } else {
                console.log("Lỗi khi lấy dữ liệu tiền tệ");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách tiền tệ:', error);
        }
    );
}
function selectBoPhanNop(data) {
    var InitialSelectBoPhanNop = $("#Department");
    InitialSelectBoPhanNop.empty();

    if (data.length === 0) {
        const option = $('<option>', {
            value: '',
            text: "Không có bộ phận nộp",
            selected: true
        });
        InitialSelectBoPhanNop.append(option);
    } else {
        data.forEach(function (department) {
            const isSelected = isEdit && department.maPhongBan === MaBoPhan;

            const option = $('<option>', {
                value: department.maPhongBan,
                text: department.tenPhongBan,
                selected: isSelected
            });

            InitialSelectBoPhanNop.append(option);
        });
    }
}
function sendFormData() {
    $('#btnSave').on('click', function (e) {
        e.preventDefault();
        var ma = $('#Ma').val();
        var HanMuc = $('#HanMuc').val();
        var Department = $('#Department').val();
        var Status = $('#Status').val()
        var formData = {
            Ma: ma,
            MaBoPhan: Department,
            HanMuc: HanMuc,
            Status: Status,
        }
        console.table(formData)
        var url = ma !== defaultUID ? '/WarningLimit/Edit' : '/WarningLimit/Add';
        $.ajax({
            url: url,
            type: 'POST',
            data: formData,
            success: function (response) {
                if (response.success) {
                    Swal.fire({
                        title: 'Thành công!',
                        text: response.message,
                        icon: 'success'
                    }).then(() => {
                        window.location.href = "/WarningLimit";
                    });
                } else {
                    Swal.fire({
                        title: 'Thất bại!',
                        text: response.message,
                        icon: 'error'
                    });
                }
            },
            error: function (xhr, status, error) {
                Swal.fire({
                    title: 'Đã xảy ra lỗi!',
                    text: 'Vui lòng thử lại.',
                    icon: 'error'
                });
                console.error(error);
            }
        });
    });
}
