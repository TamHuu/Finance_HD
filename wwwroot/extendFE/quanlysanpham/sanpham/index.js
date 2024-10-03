
let table;
$(document).ready(function () {
    table = $('#Table').DataTable({
        columnDefs: [
            { className: "d-none", targets: 0 },
            { width: '300px', className: 'text-left', targets: [1], minWidth: '300px' },
            { width: '250px', className: 'text-left', targets: [ 10, 11, 12], minWidth: '250px' },
            { width: '150px', className: 'text-center', targets: [2], minWidth: '150px' },
            { width: '150px', className: 'text-right', targets: [3, 4], minWidth: '150px' },
            { width: '150px', className: 'text-center', targets: [5, 6, 7, 9], minWidth: '150px' },
            { width: '150px', className: 'text-center', targets: [8], minWidth: '200px' },
            { width: '100px', className: 'text-center', targets: [13, 14], minWidth: '100px' },
            { width: '230px', className: 'text-left', targets: [1, 10, 11, 12], minWidth: '200px' },
        ],
       
        scrollY: '900px', 
        scrollCollapse: true,
        scrollX:true,
        paging: true,
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
        url: "/Admin/Product/getListProduct",
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
        let tensanpham = product.tenSanPham ?? "";
        let hinhanh = product.hinhAnh ?? "";
        let gia = product.gia ?? 0;
        let giakhuyenmai = product.giaKhuyenMai ?? 0;
        let thueVAT = product.thueVAT ?? 0;
        let soluong = product.soLuong ?? 0;
        let baohanh = product.baoHanh ?? 0;
        let hot = formatDate(product.hot) ?? "";
        let luotxemsanpham = product.luotxemsanpham ?? 0;
        let tendanhmuc = product.tenDanhMuc ?? "";
        let tenthuonghieu = product.tenThuongHieu ?? "";
        let tennhacungcap = product.tenNhaCungCap ?? "";
        let trangthai = product.trangThai;
        const obj = {
            tensanpham,
            hinhanh,
            gia,
            giakhuyenmai,
            thueVAT,
            soluong,
            baohanh,
            hot,
            luotxemsanpham,
            tendanhmuc,
            tenthuonghieu,
            tennhacungcap,
            trangthai
        }

        let rowContent = [
            `<td>${product.maSanPham}</td>`,
            `<td><a style="cursor:pointer;font-size: 1.1rem;" class="btn btn-link btn-sm btnThem" onclick="Read(this);" title="Xem">${tensanpham}</a></td>`,
            `<td style="overflow: hidden;text-align: center;"><img src="${hinhanh}" alt="Hình ảnh" class="img-square" style=" width: 80px;height: 80px;object-fit: cover; "></td>`,
            `<td>${addCommas(gia)}</td>`,
            `<td>${addCommas(giakhuyenmai)}</td>`,
            `<td>${thueVAT}</td>`,
            `<td>${soluong}</td>`,
            `<td>${baohanh}</td>`,
            `<td>${hot}</td>`,
            `<td>${luotxemsanpham}</td>`,
            `<td>${tendanhmuc}</td>`,
            `<td>${tenthuonghieu}</td>`,
            `<td>${tennhacungcap}</td>`,
            `<td>${trangthai}</td>`,

        ];

        let tdChucNang = `
    <button class="btn btn-warning btn-sm" onclick="Edit(this);" title="Sửa">
        <i class="fas fa-edit"></i>
    </button>
    <span>|</span>
    <button class="btn btn-danger btn-sm btnDelete" data-id="${product.maSanPham}" title="Xóa">
                <i class="fas fa-trash"></i>
    </button>

    `;
        rowContent.push(tdChucNang);
        table.row.add(rowContent);
    });

    table.draw();
    table.columns.adjust();
}

function Edit(row) {
    var rowData = table.row($(row).parents('tr')).data();
    var firstCellValue = $(row).parents('tr').find('td:eq(0)').text().trim();
    window.open('/Admin/Product/Edit?ma=' + firstCellValue, '_blank');

}


$('#Table').on('click', '.btnDelete', function (e) {
    e.preventDefault();
    var cateId = $(this).data('id');
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
                url: '/Admin/Product/Delete',
                data: { id: cateId },
                method: "DELETE",
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

function addCommas(number) {
    const numString = number.toString();
    
    const [integerPart, decimalPart] = numString.split(".");
    
    const formattedInteger = integerPart.replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    
    const formattedNumber = decimalPart ? `${formattedInteger}.${decimalPart}` : formattedInteger;

    return `${formattedNumber} VND`;
}

function Read(row) {
    var firstCellValue = $(row).parents('tr').find('td:eq(0)').text().trim();
    if (firstCellValue === "0") {
        Swal.fire({
            title: 'Lỗi!',
            text: 'Mã sản phẩm không hợp lệ.',
            icon: 'error'
        });
        return;
    }
    window.location.href = '/Admin/Product/getProductById?ma=' + firstCellValue;
}

