let table;

$(document).ready(function () {
    ConfigTable();  
    DefaultDate()
    loadChiNhanhDeNghi();
    XemDanhSach();
});



function ConfigTable() {
    table = $('#Table').DataTable({
        columnDefs: [
            { className: "d-none", targets: 0, orderable: false },
            { width: '200px', className: 'dt-left dt-head-center', targets: [2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19], orderable: false }
        ],
        scrollX: true,
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



function Edit(row) {
    var firstCellValue = $(row).parents('tr').find('td:eq(0)').text().trim();
    window.open('/ExpenseRequest/Edit?ma=' + firstCellValue, '_blank');
}


$('#Table').on('click', '.btnDelete', function (e) {
    e.preventDefault();
    var Id = $(this).data('id');
    handleDelete(Id);
});


function handleDelete(Id) {
    console.log("id", Id);
    if (!Id || Id === "0") {
        Swal.fire({
            title: 'Lỗi!',
            text: 'Không có danh mục để xoá.',
            icon: 'error'
        });
        return;
    }

    Swal.fire({
        title: 'Xác nhận xoá',
        text: 'Bạn có chắc chắn muốn xoá danh mục này?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Có, xoá nó!',
        cancelButtonText: 'Không, huỷ!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/ExpenseRequest/Delete',
                type: 'DELETE',
                data: { Id: Id },
                success: function (response) {
                    if (response.success) {
                        Swal.fire({
                            title: 'Đã xoá!',
                            text: response.message,
                            icon: 'success'
                        }).then(() => {
                            loadDanhSach(); // Tải lại danh sách sau khi xóa thành công
                        });
                    } else {
                        Swal.fire({
                            title: 'Thất bại!',
                            text: response.message,
                            icon: 'error'
                        });
                    }
                },
                error: function () {
                    Swal.fire({
                        title: 'Đã xảy ra lỗi!',
                        text: 'Vui lòng thử lại.',
                        icon: 'error'
                    });
                }
            });
        }
    });
}

// Hàm định dạng ngày tháng
function formatDate(date) {
    const d = new Date(date);
    const day = String(d.getDate()).padStart(2, '0');
    const month = String(d.getMonth() + 1).padStart(2, '0');
    const year = d.getFullYear();

    return `${day}/${month}/${year}`;
}

function DefaultDate() {
    var today = new Date();
    var sevenDaysAgo = new Date();
    sevenDaysAgo.setDate(today.getDate() - 7);

    // Lấy giá trị của "Từ ngày" và "Đến ngày"
    let TuNgay = $("#TuNgay").val();
    let DenNgay = $("#DenNgay").val();

    // Nếu "Từ ngày" và "Đến ngày" chưa được nhập, đặt giá trị mặc định
    if (!TuNgay) {
        $("#TuNgay").val(sevenDaysAgo.toISOString().slice(0, 16)); // 7 ngày trước
        TuNgay = sevenDaysAgo.toISOString().slice(0, 16); // Cập nhật giá trị mới
    }
    if (!DenNgay) {
        $("#DenNgay").val(today.toISOString().slice(0, 16)); // Ngày hôm nay
        DenNgay = today.toISOString().slice(0, 16); // Cập nhật giá trị mới
    }

    // Chuyển "Từ ngày" và "Đến ngày" thành đối tượng Date để so sánh
    let TuNgayDate = new Date(TuNgay);
    let DenNgayDate = new Date(DenNgay);

    // Kiểm tra nếu "Từ ngày" lớn hơn "Đến ngày"
    if (TuNgayDate > DenNgayDate) {
        alert("Số ngày 'Từ ngày' không được lớn hơn 'Đến ngày'");
        return; // Kết thúc hàm nếu điều kiện không hợp lệ
    }
}

function loadChiNhanhDeNghi() {
    $.ajax({
        url: "/Branch/getListBranch",
        method: "GET",
        success: function (response) {
            var data = response.data;
            var branchSelect = $('#Branch');
            branchSelect.empty();
            data.forEach(function (branch) {
                let select = $('<option>', {
                    value: branch.ma,
                    text: branch.ten,
                });
                branchSelect.append(select); 
            });
        },
        error: function () {
            console.error("Có lỗi xảy ra khi tải danh sách.");
        }
    });
}

function XemDanhSach() {
    $('#btnXem').on('click', function (e) {
        e.preventDefault();

        let TuNgay = $("#TuNgay").val();
        let DenNgay = $("#DenNgay").val();
        var branch = $('#Branch').val();
        var formdata = {
            TuNgay: TuNgay,
            DenNgay: DenNgay,
            ChiNhanhDeNghi: branch,
        };
        console.table(formdata)

        $.ajax({
            url: "ExpenseRequest/getListExpenseRequest",
            type: 'POST',
            data: formdata,
            success: function (response) {
                var result = response.data;
                drawDanhSach(result)
               
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
function drawDanhSach(data) {
    table.clear().draw();
    console.log("data", data);

    data.forEach(function (item) {
        let rowContent = [
            `<td>${item.ma}</td>`,
            `<td>${formatDate(item.ngayLap)}</td>`,
            `<td>${item.soPhieu}</td>`,
            `<td>${item.tenChiNhanhDeNghi}</td>`,
            `<td>${item.tenPhongBanDeNghi}</td>`,
            `<td>${item.nguoiDuyet}</td>`,
            `<td>${item.tenChiNhanhChi}</td>`,
            `<td>${item.tenPhongBanChi}</td>`,
            `<td>${item.soTien}</td>`,
            `<td>${item.tenTienTe}</td>`,
            `<td>${item.tyGia}</td>`,
            `<td>${formatDate(item.ngayYeuCauNhan)}</td>`,
            `<td>${item.noiDung}</td>`,
            `<td>${item.hinhThuc}</td>`,
            `<td>${item.ghiChu || ""}</td>`,
            `<td>${item.status ? "Hoạt động" : "Hết hoạt động"}</td>`,
            `<td>${item.nguoiChi}</td>`,
            `<td>${formatDate(item.ngayChi)}</td>`,
            `<td>${item.soTienChi}</td>`,
            `
                <button class="btn btn-warning btn-sm" onclick="Edit(this);" title="Sửa">
                    <i class="fas fa-edit"></i>
                </button>
                <span>|</span>
                <button class="btn btn-danger btn-sm btnDelete" data-id="${item.ma}" title="Xóa">
                    <i class="fas fa-trash"></i>
                </button>
            `
        ];

        table.row.add(rowContent).draw();
    });
}
