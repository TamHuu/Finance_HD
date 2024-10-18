let TableDanhSachBangKe;
let TableChiTietBangKe;
let TableChiTietNhanVien;
$(document).ready(function () {
    ConfigTable();
    DefaultDate()
    loadChiNhanhDeNghi();
    setupEventHandlers();
    loadCashDepositData();
});


function ConfigTable() {
    TableDanhSachBangKe = $('#TableDanhSachBangKe').DataTable({
        columnDefs: [
            { className: "d-none", targets: 0, orderable: false },
            { width: '200px', className: 'dt-left dt-head-center', targets: [1,2, 3, 4, 5, 6, 7, 9, 11, 12, 13, 14, 15, 16, 17], orderable: false },
            { width: '100px', className: 'text-center', targets: [10], orderable: false },
            { width: '200px', className: 'dt-right dt-head-center', targets: [8], orderable: false },
        ],
        lengthChange: false,
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

    TableChiTietBangKe = $('#TableChiTietBangKe').DataTable({
        columnDefs: [
            { className: "d-none", targets: 0, orderable: false },
            { width: '170px', className: 'dt-left dt-head-center', targets: [1,2, 3, 4], orderable: false },
        ],
        lengthChange: false,
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

    TableChiTietNhanVien = $('#TableChiTietNhanVien').DataTable({
        columnDefs: [
            { className: "d-none", targets: 0, orderable: false },
            { width: '100px', className: 'dt-left dt-head-center', targets: [1,2,3], orderable: false },
        ],
        lengthChange: false,
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
    window.open('/CashDeposit/Edit?ma=' + firstCellValue, '_blank');
}

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

            let allOption = $('<option>', {
                value: '',
                text: 'Tất cả'
            });
            branchSelect.append(allOption);

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

function setupEventHandlers() {
    $('#btnXem').on('click', function (e) {
        e.preventDefault();
        loadCashDepositData();
    });
    $('#btnXuatExcel').on('click', function (e) {
        e.preventDefault();
        ExportData('excel');
    });
    $('#btnXuatPDF').on('click', function (e) {
        e.preventDefault();
        ExportDataPDF('pdf');
    });

}
function loadCashDepositData() {
    let TuNgay = $("#TuNgay").val();
    let DenNgay = $("#DenNgay").val();
    var Branch = $('#Branch').val();
    var formdata = {
        TuNgay: TuNgay,
        DenNgay: DenNgay,
        DonViNop: Branch,
    };
    console.table(formdata);

    $.ajax({
        url: "CashDeposit/getDanhSachBangKe",
        type: 'POST',
        data: formdata,  
        success: function (response) {
            var result = response.data;
            DanhSachBangKe(result);
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
}
function ExportData(fileType) {
    let TuNgay = $("#TuNgay").val();
    let DenNgay = $("#DenNgay").val();
    var branch = $('#Branch').val();
    var formdata = {
        TuNgay: TuNgay,
        DenNgay: DenNgay,
        ChiNhanhDeNghi: branch,
        fileType: fileType
    };
    console.table(formdata);

    $.ajax({
        url: "CashDeposit/ExportToExcel",
        type: 'POST',
        data: formdata,
        xhr: function () {
            var xhr = new window.XMLHttpRequest();
            xhr.responseType = 'blob'; // Set response type to blob for file download
            return xhr;
        },
        success: function (data, status, xhr) {
            var blob = new Blob([data], { type: xhr.getResponseHeader('Content-Type') });
            var link = document.createElement('a');
            var contentDisposition = xhr.getResponseHeader('Content-Disposition');
            
            var filename = contentDisposition
                .split('filename=')[1]
                ?.replace(/['"]/g, '') 
                .split(';')[0] 
                .trim(); 

            link.href = URL.createObjectURL(blob);
            link.download = filename || 'download.xlsx';
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        },
        error: function (xhr, status, error) {
            console.error("Error occurred:", error);
        }
    });
}
function ExportDataPDF(fileType) {
    let TuNgay = $("#TuNgay").val();
    let DenNgay = $("#DenNgay").val();
    var branch = $('#Branch').val();
    var formdata = {
        TuNgay: TuNgay,
        DenNgay: DenNgay,
        ChiNhanhDeNghi: branch,
        fileType: fileType
    };
    console.table(formdata);

    $.ajax({
        url: "CashDeposit/ExportToPdf",
        type: 'POST',
        data: formdata,
        xhr: function () {
            var xhr = new window.XMLHttpRequest();
            xhr.responseType = 'blob'; // Set response type to blob for file download
            return xhr;
        },
        success: function (data, status, xhr) {
            // Create a URL for the blob
            var blob = new Blob([data], { type: xhr.getResponseHeader('Content-Type') });
            var link = document.createElement('a');
            var contentDisposition = xhr.getResponseHeader('Content-Disposition');

            // Extract the filename correctly
            var filename = contentDisposition
                .split('filename=')[1]
                ?.replace(/['"]/g, '') // Remove any surrounding quotes
                .split(';')[0] // Get the first part before any other parameters
                .trim(); // Trim whitespace

            link.href = URL.createObjectURL(blob);
            link.download = filename || 'download.xlsx'; // Fallback filename if extraction fails
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        },
        error: function (xhr, status, error) {
            console.error("Error occurred:", error);
        }
    });
}

function DanhSachBangKe(data) {
    TableDanhSachBangKe.clear().draw();
    console.table(data)
    data.forEach(function (item) {
        let rowContent = [
            `<td>${item.ma}</td>`,
            `<td>${formatDate(item.ngayLap)}</td>`,
            `<td>${formatDate(item.ngayLap)}</td>`,
            `<td>${formatDate(item.ngayLap)}</td>`,
            `<td>${formatDate(item.ngayLap)}</td>`,
            `<td>${formatDate(item.ngayLap)}</td>`,
            `<td>${formatDate(item.ngayLap)}</td>`,
            `<td>${formatDate(item.ngayLap)}</td>`,
            `<td>${formatDate(item.ngayLap)}</td>`,
            `<td>${formatDate(item.ngayLap)}</td>`,
            `<td>${formatDate(item.ngayLap)}</td>`,
            `<td>${formatDate(item.ngayLap)}</td>`,
            `<td>${formatDate(item.ngayLap)}</td>`,
            `<td>${formatDate(item.ngayLap)}</td>`,
            `<td>${formatDate(item.ngayLap)}</td>`,
            `<td>${formatDate(item.ngayLap)}</td>`,
            `<td>${formatDate(item.ngayLap)}</td>`,
            `
    <td>
        ${item.tenTrangThai === "Lập phiếu"
                ? `<button class="btn btn-success btn-sm btnApprove" data-id="${item.ma}" title="Duyệt">
                    <i class="fas fa-check"></i>
                </button>`
                : `<button class="btn btn-danger btn-sm btnRemoveApprove" data-id="${item.ma}" title="Bỏ duyệt">
                    <i class="fas fa-times"></i>
                </button>`}
        <span>|</span>
        <button class="btn btn-warning btn-sm" onclick="Edit(this);" title="Sửa">
            <i class="fas fa-edit"></i>
        </button>
        <span>|</span>
        <button class="btn btn-danger btn-sm btnDelete" data-id="${item.ma}" title="Xóa">
            <i class="fas fa-trash"></i>
        </button>
    </td>
    `
        ];


        TableDanhSachBangKe.row.add(rowContent).draw();
    });
}
$('#Table').on('click', '.btnDelete', function (e) {
    e.preventDefault();
    console.log("mã của delete", )
    var Id = $(this).data('id');
    handleDelete(Id);
});
$('#Table').on('click', '.btnApprove', function (e) {
    e.preventDefault();
    var Id = $(this).data('id');
    handleApprove(Id);
});
$('#Table').on('click', '.btnRemoveApprove', function (e) {
    e.preventDefault();
    var Id = $(this).data('id');
    handleRemoveApprove(Id);
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
                url: '/CashDeposit/Delete',
                type: 'DELETE',
                data: { Id: Id },
                success: function (response) {
                    if (response.success) {
                        Swal.fire({
                            title: 'Đã xoá!',
                            text: response.message,
                            icon: 'success'
                        }).then(() => {
                            loadCashDepositData(); // Tải lại danh sách sau khi xóa thành công
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
function handleApprove(Id) {
    console.log("id", Id);
    if (!Id || Id === "0") {
        Swal.fire({
            title: 'Lỗi!',
            text: 'Không có danh mục để duyệt.',
            icon: 'error'
        });
        return;
    }

    Swal.fire({
        title: 'Xác nhận Duyệt phiếu',
        text: 'Bạn có chắc chắn muốn duyệt phiếu này?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Có, Duyệt!',
        cancelButtonText: 'Không, Huỷ!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/CashDeposit/DuyetPhieuDeNghiChi',
                type: 'POST',
                data: { Id: Id },
                success: function (response) {
                    if (response.success) {
                        Swal.fire({
                            title: 'Đã xoá!',
                            text: response.message,
                            icon: 'success'
                        }).then(() => {
                            loadCashDepositData(); 
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
function handleRemoveApprove(Id) {
    console.log("id", Id);
    if (!Id || Id === "0") {
        Swal.fire({
            title: 'Lỗi!',
            text: 'Không có danh mục để duyệt.',
            icon: 'error'
        });
        return;
    }

    Swal.fire({
        title: 'Xác nhận bỏ duyệt phiếu',
        text: 'Bạn có chắc chắn muốn bỏ duyệt phiếu này?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Có!',
        cancelButtonText: 'Không, Huỷ!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/CashDeposit/BoDuyetPhieuDeNghiChi',
                type: 'POST',
                data: { Id: Id },
                success: function (response) {
                    if (response.success) {
                        Swal.fire({
                            title: 'Đã bỏ duyệt!',
                            text: response.message,
                            icon: 'success'
                        }).then(() => {
                            loadCashDepositData();
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

function addCommas(amount) {
    if (amount == null || isNaN(amount)) {
        return '0';
    }
    // Chuyển đổi số thành chuỗi và sử dụng phương thức replace với biểu thức chính quy
    return amount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
}

