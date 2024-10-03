
let table;
$(document).ready(function () {
    table = $('#productCategoryTable').DataTable({
        columnDefs: [
            { width: '50px', className: 'text-center', targets: [1] },
            { className: "d-none", "targets": 0 },
            { width: '100px', className: 'text-center', targets: [7] },
            { width: '600px', "orderable": false, "targets": [2, 3], className: 'text-left' },
            { "orderable": false, "targets": [ 4, 5, 6, 7], className: 'text-center' }
        ],
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

    loadDanhSach();
});
function loadDanhSach() {
    $.ajax({
        url: "/Admin/ProductCategory/GetCategories",
        method: "GET",
        success: function (response) {
            if (response && response.data) {
                drawDanhSach(response.data);
            } else {
                console.error("Dữ liệu không hợp lệ.");
            }
        },
        error: function () {
            console.error("Có lỗi xảy ra khi tải danh sách.");
        }
    });
}
function drawDanhSach(data) {
    table.clear().draw();
    data.forEach(function (product, index) {
        let tentrangthai = product.trangThai == true ? "Hoạt động" : "Hết hoạt động";
        let ngay = formatDate(product.ngayTao);
        console.log("ngày", product.ngayTao)
        let rowContent = [
            `<td>${product.ma}</td>`,
            `<td>${index + 1}</td>`,
            `<td>${product.tenDanhMuc}</td>`,
            `<td>${product.tieuDeSeo}</td>`,
            `<td>${tentrangthai}</td>`,
            `<td>${product.sapXep}</td>`,
            `<td>${ngay}</td>`
        ];
        let tdChucNang = `
    <button class="btn btn-warning btn-sm" onclick="Edit(this);" title="Sửa">
        <i class="fas fa-edit"></i>
    </button>
    <span>|</span>
    <button class="btn btn-danger btn-sm btnDelete" data-id="${product.ma}" title="Xóa">
                <i class="fas fa-trash"></i>
    </button>

    `;
        rowContent.push(tdChucNang);
        table.row.add(rowContent).draw();
    });
}

function Edit(row) {
    var rowData = table.row($(row).parents('tr')).data();
    var firstCellValue = $(row).parents('tr').find('td:eq(0)').text().trim();
    window.open('/Admin/ProductCategory/Edit?ma=' + firstCellValue, '_blank');

}


$('#productCategoryTable').on('click', '.btnDelete', function (e) {
    e.preventDefault();
    var cateId = $(this).data('id');
    console.log("id", cateId);
    handleDelete(cateId);
});


function handleDelete(cateId) {
    if (!cateId || cateId === "0") {
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
                url: '/Admin/ProductCategory/Delete',  
                type: 'DELETE',
                data: { id: cateId },
                success: function (response) {
                    if (response.success) {
                        Swal.fire({
                            title: 'Đã xoá!',
                            text: response.message,
                            icon: 'success'
                        }).then(() => {
                            loadDanhSach(); 
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
        }
    });
}

function formatDate(date) {

    const d = new Date(date);
    const day = String(d.getDate()).padStart(2, '0');
    const month = String(d.getMonth() + 1).padStart(2, '0');
    const year = d.getFullYear();

    return `${day}/${month}/${year}`;
}

