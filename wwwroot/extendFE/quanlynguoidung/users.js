$(document).ready(function () {
    $('#table').DataTable({
        searching: true,
        paging: true,
        ordering: false,
        columnDefs: [
            { targets: 0, width: '20px', className: 'text-center' },
            { targets: [1, 2, 3], width: '200px', className: 'text-left' },
            { targets: [4], width: '200px', className: 'text-center' },
        ],
        language: {
            decimal: "",
            emptyTable: "Không có dữ liệu trong bảng",
            info: "Hiển thị _START_ đến _END_ trong tổng số _TOTAL_ mục",
            infoEmpty: "Hiển thị 0 đến 0 trong tổng số 0 mục",
            infoFiltered: "(đã lọc từ _MAX_ mục)",
            lengthMenu: "Hiển thị _MENU_ mục",
            loadingRecords: "Đang tải...",
            processing: "Đang xử lý...",
            search: "Tìm kiếm:",
            searchPlaceholder: "Nhập từ khóa...",
            zeroRecords: "Không tìm thấy kết quả nào",
            paginate: {
                first: "Đầu tiên",
                last: "Cuối cùng",
                next: "Kế tiếp",
                previous: "Trước"
            }
        }
    });

    // Sửa User
    $(document).on('click', '.btnSua', function () {
        const id = $(this).data('id');
        console.log("id",id)
        $('#btnSaveChanges').data('id', id).show();
        $.ajax({
            url: "/admin/Users/UpdateUser",
            type: "POST",
            data: { id: id },
            success: function (response) {
                if (response.success) {
                    const user = response.data;
                    $('#model .modal-title').text("Sửa User");
                    $('#model .modal-body').html(`
                        <div class="form-group">
                            <label for="userName">Tên người dùng</label>
                            <input class="form-control" type="text" id="userName" value="${user.userName}" />
                            <label for="password">Mật khẩu</label>
                            <input class="form-control" type="text" id="password" value="${user.password}" />
                            <label for="email">Email</label>
                            <input class="form-control" type="email" id="email" value="${user.email}" />
                        </div>
                    `);
                    $('#model').modal('show');
                } else {
                    alert(response.message);
                }
            },
            error: function () {
                alert("Đã xảy ra lỗi trong khi lấy thông tin User.");
            }
        });
    });

    // Xóa User
    $(document).on('click', '.btnXoa', function () {
        const id = $(this).data('id');
        $('#btnConfirmDelete').data('id', id);
        $('#modelDelete').modal('show');
    });

    $(document).on('click', '#btnConfirmDelete', function () {
        const id = $(this).data('id');
        $.ajax({
            url: "/admin/Users/DeleteUser",
            type: "DELETE",
            data: { id: id },
            success: function (response) {
                if (response.success) {
                    alert("Xóa User thành công!");
                    $('#modelDelete').modal('hide');
                    location.reload();
                } else {
                    alert(response.message);
                }
            },
            error: function () {
                alert("Đã xảy ra lỗi trong khi xóa User.");
            }
        });
    });

    // Thêm User
    $(document).on('click', '.btnThem', function () {
        $('#btnSaveChanges').data('id', null).show();
        $('#model .modal-title').text("Thêm User");
        $('#model .modal-body').html(`
    <div class="form-group">
     
        <label for="userName">Tên người dùng</label>
        <input class="form-control" type="text" id="userName" />
        
        <label for="password">Mật khẩu</label>
        <input class="form-control" type="password" id="password" />
        
        <label for="email">Email</label>
        <input class="form-control" type="email" id="email" />
        
        <label for="address">Địa chỉ</label>
        <input class="form-control" type="text" id="address" />
        
        <label for="phone">Số điện thoại</label>
        <input class="form-control" type="text" id="phone" />
    </div>
`);

        $('#model').modal('show');
    });

 
    $('#btnSaveChanges').click(function () {
        const id = $(this).data('id');
        const UserName = $('#userName').val();
        const Password = $('#password').val();
        const Email = $('#email').val();
        const Address = $('#address').val();
        const Phone = $('#phone').val();
        // Kiểm tra đầu vào
        if (!UserName || !Password || !Email) {
            alert("Vui lòng điền đầy đủ thông tin.");
            return;
        }

        const url = id ? "/admin/Users/UpdateUser" : "/admin/Users/AddUser";
        const type = id ? "POST" : "POST";
        const data = id ? { id, UserName, Password, Email, Address, Phone } : { UserName, Password, Email, Address, Phone };

        $.ajax({
            url: url,
            type: type,
            data: data,
            success: function (response) {
                if (response.success) {
                    alert(id ? "Cập nhật User thành công!" : "Thêm User thành công!");
                    $('#model').modal('hide');
                    location.reload();
                } else {
                    alert(response.message);
                }
            },
            error: function () {
                alert("Đã xảy ra lỗi trong khi " + (id ? "cập nhật" : "thêm") + " User.");
            }
        });
    });
});
