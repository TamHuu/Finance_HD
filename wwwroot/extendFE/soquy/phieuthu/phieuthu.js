
$(document).ready(function () {
    ConfigTable();
    getDataPhieuThu();
    DonVi();
    updateDenNgay()
});

function ConfigTable() {
    // Định nghĩa ngôn ngữ dùng chung
    const commonLanguage = {
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
    };

    TableDanhSachPhieuChiNoiBo = $('#TableDanhSachPhieuChiNoiBo').DataTable({
        columnDefs: [
            { className: "hide_column", "targets": [0] },
            { orderable: false, className: "txt-left", "targets": [1, 3, 4, 5, 6, 7, 8, 9, 12, 13, 14] },
            { orderable: false, className: "txt-right", "targets": [10, 11] },
        ],
        searching: true,
        ordering: true,
        lengthChange: false,
        language: commonLanguage,
    });

    TableDanhSachPhieuThu = $('#TableDanhSachPhieuThu').DataTable({
        columnDefs: [
            { orderable: false, className: "d-none", targets: 0, orderable: false },
            { orderable: false, width: '200px', className: "txt-left dt-head-center", "targets": [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12, 13] },
            { orderable: false, width: '150px', className: "txt-center dt-head-center", "targets": [11] },
            { orderable: false, width: '150px', className: "txt-center dt-head-center", "targets": [14, 15] },

        ],
        scrollX: true,
        searching: true,
        ordering: true,
        lengthChange: false,
        language: commonLanguage,
    });
}

function DonVi() {
    callAPI('GET', '/Branch/getListBranch', null,
        function (response) {
            if (response.success) {
                selectDonVi(response.data);
            } else {
                console.log("Lỗi khi lấy dữ liệu ");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách:', error);
        }
    );
}
function selectDonVi(data) {
    var InitialSelectDonVi = $("#Branch");
    InitialSelectDonVi.empty();

    const defaultOption = $('<option>', {
        value: '',
        text: "Tất cả",
        selected: true
    });
    InitialSelectDonVi.append(defaultOption);
    if (data.length > 0) {
        data.forEach(function (branch) {
            const option = $('<option>', {
                value: branch.ma,
                text: branch.ten
            });
            InitialSelectDonVi.append(option);
        });
    }
}

$('#TableDanhSachPhieuChiNoiBo tbody').on('click', 'tr', function () {
    // Remove 'selected' class from any row that has it
    $('#TableDanhSachPhieuChiNoiBo tbody tr.selected').removeClass('selected');

    $(this).toggleClass('selected');
});
$('#TableDanhSachPhieuThu tbody').on('click', 'tr', function () {
    $('#TableDanhSachPhieuThu tbody tr.selected').removeClass('selected');
    $(this).toggleClass('selected');
});
function getDataPhieuThu() {
    let TuNgay = $("#TuNgay").val();
    let DenNgay = $("#DenNgay").val();
    var branch = $('#Branch').val();
    var formdata = {
        TuNgay: TuNgay,
        DenNgay: DenNgay,
        ChiNhanhDeNghi: branch,
    };
    console.table(formdata);

    $.ajax({
        url: "Receipt/getListReceipt",
        type: 'POST',
        data: formdata,
        success: function (response) {
            var result = response.data;
            drawDanhSach(result);
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

function drawDanhSach(data) {
    TableDanhSachPhieuThu.clear().draw();
    console.table(data)
    data.forEach(function (item) {
        const ma = item.ma || "";
        const soPhieu = item.soPhieu || "";
        const ngayLapPhieu = item.ngayLapPhieu ? formatDate(item.ngayLapPhieu) : "";
        const tenNguoiLapPhieu = item.tenNguoiLapPhieu || "";
        const tenChiNhanhThu = item.tenChiNhanhThu || "";
        const tenPhongBanThu = item.tenPhongBanThu || "";
        const tenChiNhanhChi = item.tenChiNhanhChi || "";
        const tenPhongBanChi = item.tenPhongBanChi || "";
        const tenNoiDungThuChi = item.tenNoiDungThuChi || "";
        const tenTienTe = item.tenTienTe || "";
        const tyGia = item.tyGia || "";
        const soTien = item.soTien ? addCommas(item.soTien) : "";
        const hinhThuc = item.hinhThuc || "";
        const ghiChu = item.ghiChu || "";
        const trangThai = item.trangThai || "";
        let rowContent = [
            `<td>${ma}</td>`,
            `<td>${soPhieu}</td>`,
            `<td>${ngayLapPhieu}</td>`,
            `<td>${tenNguoiLapPhieu}</td>`,
            `<td>${tenChiNhanhThu}</td>`,
            `<td>${tenPhongBanThu}</td>`,
            `<td>${tenChiNhanhChi}</td>`,
            `<td>${tenPhongBanChi}</td>`,
            `<td>${tenNoiDungThuChi}</td>`,
            `<td>${tenTienTe}</td>`,
            `<td>${tyGia}</td>`,
            `<td>${soTien}</td>`,
            `<td>${hinhThuc}</td>`,
            `<td>${ghiChu}</td>`,
            `<td>${trangThai}</td>`,
            `
    <td>
        ${item.trangThai === "Tạm nộp"
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


        TableDanhSachPhieuThu.row.add(rowContent).draw();
    });
}
function Edit(row) {
    var firstCellValue = $(row).parents('tr').find('td:eq(0)').text().trim();
    window.open('/Receipt/Edit?ma=' + firstCellValue, '_blank');
}
$('#TableDanhSachPhieuThu').on('click', '.btnDelete', function (e) {
    e.preventDefault();
    var Id = $(this).data('id');
    handleDelete(Id);
});
$('#TableDanhSachPhieuThu').on('click', '.btnApprove', function (e) {
    e.preventDefault();
    var Id = $(this).data('id');
    handleApprove(Id);
});
$('#TableDanhSachPhieuThu').on('click', '.btnRemoveApprove', function (e) {
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
                url: '/Receipt/Delete',
                type: 'DELETE',
                data: { Id: Id },
                success: function (response) {
                    if (response.success) {
                        Swal.fire({
                            title: 'Đã xoá!',
                            text: response.message,
                            icon: 'success'
                        }).then(() => {
                            getDataPhieuThu(); // Tải lại danh sách sau khi xóa thành công
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
                url: '/Receipt/DuyetPhieuThu',
                type: 'POST',
                data: { Id: Id },
                success: function (response) {
                    if (response.success) {
                        Swal.fire({
                            title: 'Đã xoá!',
                            text: response.message,
                            icon: 'success'
                        }).then(() => {
                            getDataPhieuThu();
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
                url: '/Receipt/BoDuyetDuyetPhieuThu',
                type: 'POST',
                data: { Id: Id },
                success: function (response) {
                    if (response.success) {
                        Swal.fire({
                            title: 'Đã bỏ duyệt!',
                            text: response.message,
                            icon: 'success'
                        }).then(() => {
                            getDataPhieuThu();
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

function formatDate(date) {
    const d = new Date(date);
    const day = String(d.getDate()).padStart(2, '0');
    const month = String(d.getMonth() + 1).padStart(2, '0');
    const year = d.getFullYear();

    return `${day}/${month}/${year}`;
}
function updateDenNgay() {
    const tuNgay = document.getElementById("TuNgay").value;
    if (tuNgay) {
        const tuNgayDate = new Date(tuNgay);
        const denNgayDate = new Date(tuNgayDate);
        denNgayDate.setDate(tuNgayDate.getDate() + 10); // Cộng thêm 10 ngày

        const denNgayInput = document.getElementById("DenNgay");
        denNgayInput.value = denNgayDate.toISOString().slice(0, 16); // Định dạng thành yyyy-MM-ddTHH:mm
    }
}