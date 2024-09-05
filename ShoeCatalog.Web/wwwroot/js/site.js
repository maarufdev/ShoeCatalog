// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(window).on('load', function () {
    // Hide the spinner when the page finishes loading
    $('.spinner-container').fadeOut();
});

const apiService = function (url, method, data) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: url,
            method: method,
            data: data,
            success: function (response) {
                resolve(response)
            },
            error: function (xhr, status, error) {
                reject(error)
            }
        })
    })
}

function registerEvent(target, type, callback) {
    $(target).off(type).on(type, function (e) {
        callback(e);
    });
}