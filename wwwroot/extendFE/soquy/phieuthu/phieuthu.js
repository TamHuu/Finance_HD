
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
            { className: "txt-left", "targets": [1, 3, 4, 5, 6, 7, 8, 9, 12, 13, 14] },
            { className: "txt-right", "targets": [10, 11] },
        ],
        searching: true,
        ordering: true,
        lengthChange: false,
        language: commonLanguage,
    });

    TableDanhSachPhieuThu = $('#TableDanhSachPhieuThu').DataTable({
        columnDefs: [
            { className: "d-none", targets: 0, orderable: false },
            { width: '200px', className: "txt-left dt-head-center", "targets": [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12, 13] },
            { width: '150px', className: "txt-center dt-head-center", "targets": [11] },
            { width: '150px', className: "txt-center dt-head-center", "targets": [14, 15] },
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
        let rowContent = [
            `<td>${item.ma}</td>`,
            `<td>${item.soPhieu}</td>`,
            `<td>${formatDate(item.ngayLapPhieu)}</td>`,
            `<td>${item.tenNguoiLapPhieu}</td>`,
            `<td>${item.tenChiNhanhThu}</td>`,
            `<td>${item.tenPhongBanThu}</td>`,
            `<td>${item.tenChiNhanhChi}</td>`,
            `<td>${item.tenPhongBanChi}</td>`,
            `<td>${item.tenNoiDungThuChi}</td>`,
            `<td>${item.tenTienTe}</td>`,
            `<td>${item.tyGia}</td>`,
            `<td>${addCommas(item.soTien)}</td>`,
            `<td>${item.hinhThuc}</td>`,
            `<td>${item.ghiChu || ""}</td>`,
            `<td>${item.trangThai}</td>`,
            `
    <td>
        ${item.trangThai === "Lập phiếu"
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