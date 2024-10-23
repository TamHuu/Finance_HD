// Hàm CallApi
function callAPI(method, url, data, successCallback, errorCallback) {
    $.ajax({
        type: method,
        url: url,

        data: data, // Convert to JSON
        success: successCallback,
        error: errorCallback
    });
}
//// Hàm formmat số tiền 
function addCommas(amount) {
    if (amount == null || isNaN(amount)) {
        return '';
    }
    return amount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
}
// Hàm format số tiền ngay lúc nhập
function formatCurrencyInput(input) {
    let inputValue = input.value.replace(/[^0-9.]/g, '').replace(/(\..*?)\..*/g, '$1');

    if (inputValue === '') {
        input.value = '';
        return;
    }

    const numericValue = parseFloat(inputValue);
    const formattedValue = addCommas(numericValue);

    input.value = formattedValue;
}
