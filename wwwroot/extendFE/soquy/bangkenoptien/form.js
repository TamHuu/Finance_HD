$(document).ready(function () {
    fetchMonetaryList();
    fetchBranchList();
});

// Tiền tệ (VND)
function fetchMonetaryList() {

    callAPI('GET', '/Monetary/getListMonetary', null,
        function (response) {
            if (response.success) {
                MonetarySelect(response.data);
            } else {
                console.log("Lỗi khi lấy dữ liệu tiền tệ");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách tiền tệ:', error);
        }
    );
}

function MonetarySelect(data) {
    const monetarySelect = $('#TienTe');
    monetarySelect.empty();
    data.forEach(function (monetary) {
        const option = $('<option>', {
            value: monetary.ma,
            text: monetary.ten,
            selected: monetary.ma === '0febf710-436d-40cc-95e5-e457605cd104' || monetary.code === "VND",
        });
        monetarySelect.append(option);
    });
}

// Chi nhánh
function fetchBranchList() {
    callAPI('GET', '/Branch/getListBranch', null,
        function (response) {
            if (response.success) {
                DonViNopSelect(response.data);
                DonViNhanSelect(response.data);

                // Gọi lại để cập nhật phòng ban nộp và nhận
                var maDonViNop = $('#DonViNop').val();
                BoPhanNopSelect(maDonViNop);
                var maDonViNhan = $('#DonViNhan').val();
                BoPhanNhanSelect(maDonViNhan);
            } else {
                console.log("Lỗi khi lấy dữ liệu chi nhánh");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách chi nhánh:', error);
        }
    );
}

function DonViNopSelect(data) {
    const BranchSelect = $('#DonViNop');
    BranchSelect.empty();
    console.log(data);
    data.forEach(function (branch) {
        const option = $('<option>', {
            value: branch.ma,
            text: branch.ten,
            selected: branch.ma === MaChiNhanhDangNhap,
        });
        BranchSelect.append(option);
    });
}

function DonViNhanSelect(data) {
    const BranchSelect = $('#DonViNhan');
    BranchSelect.empty();
    console.log(data);
    data.forEach(function (branch) {
        const option = $('<option>', {
            value: branch.ma,
            text: branch.ten,
        });
        BranchSelect.append(option);
    });
}

// Phòng ban
function BoPhanNopSelect(maDonViNop) {
    callAPI('GET', '/Department/getListDepartment', null,
        function (response) {
            if (response.success) {
                var result = response.data;
                var filterData = result.filter(x => x.maChiNhanh === maDonViNop);

                const DepartmentSelect = $('#BoPhanNop');
                DepartmentSelect.empty();

                filterData.forEach(function (department) {
                    const option = $('<option>', {
                        value: department.maPhongBan,
                        text: department.tenPhongBan,
                        selected: department.maPhongBan === MaPhongBanDangNhap, 
                    });
                    DepartmentSelect.append(option);
                });
            } else {
                console.log("Lỗi khi lấy dữ liệu bộ phận nộp");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách bộ phận nộp:', error);
        }
    );
}
function BoPhanNhanSelect(maDonViNhan) {
    callAPI('GET', '/Department/getListDepartment', null,
        function (response) {
            if (response.success) {
                var result = response.data;
                const DepartmentSelect = $('#BoPhanNhan');
                DepartmentSelect.empty();

                var filterData = result.filter(x => x.maChiNhanh === maDonViNhan);

                filterData.forEach(function (department) {
                    const option = $('<option>', {
                        value: department.maPhongBan,
                        text: department.tenPhongBan,
                    });
                    DepartmentSelect.append(option);
                });
            } else {
                console.log("Lỗi khi lấy dữ liệu bộ phận nhận");
            }
        },
        function (xhr, status, error) {
            console.error('Lỗi khi lấy danh sách bộ phận nhận:', error);
        }
    );
}
// Sự kiện onchange cho chi nhánh
$("#DonViNop").on('change', function () {
    var maDonViNop = $(this).val();
    BoPhanNopSelect(maDonViNop);
});

$("#DonViNhan").on('change', function () {
    var maDonViNhan = $(this).val();
    BoPhanNhanSelect(maDonViNhan);
});
