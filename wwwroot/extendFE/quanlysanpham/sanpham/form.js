$(document).ready(function () {
    var currentImageUrl = $('#currentImage').attr('src'); // Lưu URL hình ảnh hiện tại
    console.log("lưu giá trị cũ", currentImageUrl)
    $('#btnSave').on('click', function (e) {
        e.preventDefault();
        uploadImage(currentImageUrl); // Truyền giá trị cũ vào hàm uploadImage
    });

    $('#image').on('change', previewImage);
});

function uploadImage(currentImageUrl) {
    var image = $('#image')[0].files[0];

    // Kiểm tra xem có file hay không
    if (image) {
        var formData = new FormData();
        formData.append('Image', image);

        $.ajax({
            url: '/Admin/Product/UploadImage',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                console.log("Dữ liệu trả về của file ảnh", response);
                if (response.success) {
                    AddProduct(response.imageUrl); // Chỉ gọi AddProduct nếu upload thành công
                } else {
                    showAlert('Thất bại!', response.message, 'error');
                }
            },
            error: function (xhr, status, error) {
                showAlert('Đã xảy ra lỗi!', 'Vui lòng thử lại.', 'error');
                console.error(error);
            }
        });
    } else {
        // Nếu không có file, gọi AddProduct với currentImageUrl
        AddProduct(currentImageUrl);
    }
}

function AddProduct(imageUrl) {
    var id = getIdFromUrl();
    var productData = getProductData(imageUrl, id);

    var url = productData.Id && productData.Id !== "0" ? '/Admin/Product/Edit' : '/Admin/Product/Add';

    $.ajax({
        url: url,
        type: 'POST',
        data: productData,
        success: function (response) {
            if (response.success) {
                showAlert('Thành công!', response.message, 'success').then(() => {
                    window.location.href = "/Admin/product";
                });
            } else {
                showAlert('Thất bại!', response.message, 'error');
            }
        },
        error: function (xhr, status, error) {
            showAlert('Đã xảy ra lỗi!', 'Vui lòng thử lại.', 'error');
            console.error(error);
        }
    });
}

function getProductData(imageUrl, id) {
    return {
        Id: id,
        Name: $('#Name').val(),
        SeoTitle: $('#seoTitle').val(),
        Category: $('#category').val(),
        Brand: $('#brand').val(),
        Supplier: $('#supplier').val(),
        Price: $('#price').val(),
        PromotionPrice: $('#promotionPrice').val(),
        Vat: $('#vat').val(),
        Quantity: $('#quantity').val(),
        Warranty: $('#warranty').val(),
        HotDate: $('#hotDate').val(),
        Views: $('#views').val(),
        Status: $('#status').val(),
        CreatedDate: $('#createdDate').val(),
        Description: $('#description').val(),
        Detail: $('#detail').val(),
        Image: imageUrl // Sử dụng imageUrl từ hàm uploadImage
    };
}

function showAlert(title, text, icon) {
    return Swal.fire({ title: title, text: text, icon: icon });
}

function previewImage(event) {
    const currentImage = document.getElementById('currentImage');
    const file = event.target.files[0];

    if (file) {
        const objectUrl = URL.createObjectURL(file);
        currentImage.src = objectUrl; // Cập nhật hình ảnh xem trước
    }
}

function getIdFromUrl() {
    const urlParams = new URLSearchParams(window.location.search);
    const ma = urlParams.get('ma');
    return ma;
}
