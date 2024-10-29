
$(document).ready(function () {
    ConfigTable();
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
            searching: false,
            ordering: false,
            lengthChange: false,
            language: commonLanguage,
        });

        TableDanhSachPhieuThu = $('#TableDanhSachPhieuThu').DataTable({
            columnDefs: [
                { className: "d-none", targets: 0, orderable: false },
                { className: "txt-left", "targets": [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22] },
               
            ],
            scrollX:true,
            searching: false,
            lengthChange: false,
            language: commonLanguage,
        });
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
