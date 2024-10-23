function callAPI(method, url, data, successCallback, errorCallback) {
    $.ajax({
        type: method,
        url: url,

        data: data, // Convert to JSON
        success: successCallback,
        error: errorCallback
    });
}
