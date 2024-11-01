﻿let TableDanhSachBangKe;
let TableChiTietBangKe;
let TableChiTietNhanVien;
$(document).ready(function () {
    ConfigTable();
    DefaultDate()
    loadChiNhanhDeNghi();
    setupEventHandlers();
    loadDanhSachBangKe();
    loadChiTietBangKe();
});
function ConfigTable() {
    TableDanhSachBangKe = $('#TableDanhSachBangKe').DataTable({
        columnDefs: [
            { className: "d-none", targets: 0, orderable: false },
            { width: '200px', className: 'dt-left dt-head-center', targets: [1, 2, 3, 4, 5, 6, 7, 9, 11, 8], orderable: false },
            { width: '100px', className: 'text-center', targets: [13,], orderable: false },
            { width: '100px', className: 'dt-left dt-head-center', targets: [12,14,16], orderable: false },
            { width: '100px', className: 'text-center', targets: [17], orderable: false },
            { width: '100px', className: 'text-center', targets: [10], orderable: false },
            { width: '200px', className: 'dt-right dt-head-center', targets: [15], orderable: false },
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
            { width: '100px', className: 'dt-right dt-head-center', targets: [1, 2, 3], orderable: false },
            { width: '200px', className: 'dt-left dt-head-center', targets: [4], orderable: false },
        ],
        searching:false,
        ordering:false,
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
            { className: "d-none", targets: [0,1], orderable: false },
            { width: '300px', className: 'dt-left dt-head-center', targets: [2], orderable: false },
            { width: '200px', className: 'dt-right dt-head-center', targets: [3], orderable: false },
        ],
        searching: false,
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
    
    let TuNgay = $("#TuNgay").val();
    let DenNgay = $("#DenNgay").val();
    
    if (!TuNgay) {
        $("#TuNgay").val(sevenDaysAgo.toISOString().slice(0, 16)); 
        TuNgay = sevenDaysAgo.toISOString().slice(0, 16); 
    }
    if (!DenNgay) {
        $("#DenNgay").val(today.toISOString().slice(0, 16)); 
        DenNgay = today.toISOString().slice(0, 16); 
    }
    let TuNgayDate = new Date(TuNgay);
    let DenNgayDate = new Date(DenNgay);
    if (TuNgayDate > DenNgayDate) {
        alert("Số ngày 'Từ ngày' không được lớn hơn 'Đến ngày'");
        return; 
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
        loadDanhSachBangKe();
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
function loadDanhSachBangKe() {
    let TuNgay = $("#TuNgay").val();
    let DenNgay = $("#DenNgay").val();
    var Branch = $('#Branch').val();
    var formdata = {
        TuNgay: TuNgay,
        DenNgay: DenNgay,
        DonViNop: Branch,
    };
    $.ajax({
        url: "CashDeposit/getDanhSachBangKe",
        type: 'POST',
        data: formdata,  
        success: function (response) {
            var result = response.data;
            TableBangKe(result);
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
function loadChiTietBangKe(Ma) {

    $.ajax({
        url: "CashDeposit/getChiTietBangKe",
        type: 'POST',
        data: { Ma: Ma },
        success: function (response) {
            var result = response.data;
            TableDetailBangKe(result);
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
function loadChiTietNhanVien(Ma) {
    console.log("Ma nhan vien")
    $.ajax({
        url: "CashDeposit/getChiTietNhanVien",
        type: 'POST',
        data: { Ma: Ma },
        success: function (response) {
            var result = response.data;
            TableNhanVien(result);
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

function TableBangKe(data) {
    TableDanhSachBangKe.clear().draw();
    data.forEach(function (item) {
        let rowContent = [
            `<td>${item.ma}</td>`,
            `<td>${item.soPhieu}</td>`,
            `<td>${item.tenChiNhanhNop}</td>`,
            `<td>${item.tenChiNhanhNhan}</td>`,
            `<td>${item.tenPhongBanNhan}</td>`,
            `<td>${formatDate(item.ngayLap)}</td>`,
            `<td>${formatDate(item.ngayNopTien)}</td>`,
            `<td>${item.tenNguoiNopTien}</td>`,
            `<td>${item.diaChi}</td>`,
            `<td>${item.noiDung}</td>`,
            `<td>${item.nguoiNhanTien}</td>`,
            `<td>${item.ghiChu}</td>`,
            `<td>${item.tenTienTe}</td>`,
            `<td>${item.tyGia}</td>`,
            `<td>${item.hinhThuc}</td>`,
            `<td>${addCommas(item.soTien)}</td>`,
            `<td>${item.trangThai}</td>`,
            `
    <td>
     
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
function TableDetailBangKe(data) {
    $('#TableDanhSachBangKe').on('click', 'tr', function (e) {
        e.preventDefault();
        var totalMoney = $(this).find('td:eq(15)').text();
        var tenTienTe = $(this).find('td:eq(12)').text();

        var newRow = $('<tr></tr>');
        var newData1 = $(`<td colspan="2" style="text-align:center;"></td>`).text(`Tổng tiền (${tenTienTe})`);
        var newData3 = $('<td style="text-align:right;"></td>').text(`${totalMoney}`); 
        var newData4 = $('<td></td>').text(''); 

        newRow.append(newData1);
        newRow.append(newData3);
        newRow.append(newData4);

        $('#TableChiTietBangKe tfoot').empty().append(newRow);
    });

    TableChiTietBangKe.clear().draw(); 
    data.forEach(function (item) {
        let rowContent = [
            `<td>${item.ma}</td>`,
            `<td>${item.tenLoaiTien}</td>`,
            `<td>${addCommas(item.soLuong)}</td>`,
            `<td>${addCommas(item.thanhTien)}</td>`,
            `<td>${item.ghiChu}</td>`,
        ];
        
        TableChiTietBangKe.row.add(rowContent).draw();
    });
}

function TableNhanVien(data) {
    TableChiTietNhanVien.clear().draw();
    data.forEach(function (item) {
        let rowContent = [
            `<td>${item.ma}</td>`,
            `<td>${item.maNhanVien}</td>`,
            `<td>${item.tenNhanVien}</td>`,
            `<td>${addCommas(item.soTien)}</td>`,
        ];


        TableChiTietNhanVien.row.add(rowContent).draw();
    });
}
$('#TableDanhSachBangKe').on('click', '.btnDelete', function (e) {
    e.preventDefault();
    var Id = $(this).data('id');
    handleDelete(Id);
});

// Sử dụng event delegation
$('#TableDanhSachBangKe').on('click', 'tr', function (e) {
    e.preventDefault();

    // Lấy giá trị của ô đầu tiên (thứ 0) trong hàng được click
    var firstCellValue = $(this).find('td:eq(0)').text(); 
    loadChiTietBangKe(firstCellValue);
    loadChiTietNhanVien(firstCellValue);
});


function handleDelete(Id) {
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
                            loadDanhSachBangKe();
                            loadChiTietBangKe();
                            loadChiTietNhanVien();
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
    return amount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
}

$('#TableDanhSachBangKe tbody').on('click', 'tr', function () {
    // Remove 'selected' class from any row that has it
    $('#TableDanhSachBangKe tbody tr.selected').removeClass('selected');
    
    $(this).toggleClass('selected');
});
$('#TableChiTietBangKe tbody').on('click', 'tr', function () {
    $('#TableChiTietBangKe tbody tr.selected').removeClass('selected');
    $(this).toggleClass('selected');
});
$('#TableChiTietNhanVien tbody').on('click', 'tr', function () {
    $('#TableChiTietNhanVien tbody tr.selected').removeClass('selected');
    $(this).toggleClass('selected');
});