
let table;
$(document).ready(function () {
    table = $('#Table').DataTable({
        columnDefs: [
            { className: "d-none", targets: 0, orderable: false },
            { width: '200px', className: 'dt-left dt-head-center', targets: [1, 2,  4, 5, 6, 9], orderable: false },
            { minWidth: '250px', width: '250px', className: 'dt-left dt-head-center', targets: [3], orderable: false },
            { minWidth: '300px', width: '250px', className: 'dt-left dt-head-center', targets: [8], orderable: false },
            { width: '150px', className: 'center', targets: [7,10], orderable: false },
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
        url: "/User/getListUser",
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
    console.table(data)
    data.forEach(function (item, index) {
        let ma = item.maUser;
        let hoten = item.fullName ?? "";
        let tendangnhap = item.userName ?? "";
        let phongban = item.tenPhongBan ?? "";
        let chucdanh = item.chucdanh ?? "";
        let chucvu = item.chucvu ?? "";
        let ldap = item.ldap == true ? "Đăng nhập" : "Đăng xuất";
        let tentrangthai = item.status;
        let donvi = item.tenChiNhanh ?? "";
        let sodienthoai = item.soDienThoai ?? 0;
        let rowContent = [
            `<td>${ma}</td>`,
            `<td>${hoten}</td>`,
            `<td>${tendangnhap}</td>`,
            `<td>${phongban}</td>`,
            `<td>${chucdanh}</td>`,
            `<td>${chucvu}</td>`,
            `<td>${ldap}</td>`,
            `<td>${tentrangthai}</td>`,
            `<td>${donvi}</td>`,
            `<td>${sodienthoai}</td>`,
        ];
        let tdChucNang = `
    <button class="btn btn-warning btn-sm" onclick="Edit(this);" title="Sửa">
        <i class="fas fa-edit"></i>
    </button>
    <span>|</span>
    <button class="btn btn-danger btn-sm btnDelete" data-id="${ma}" title="Xóa">
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
    window.open('/User/Edit?ma=' + firstCellValue, '_blank');

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
                url: '/User/Delete',
                type: 'DELETE',
                data: { Id: Id },
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

