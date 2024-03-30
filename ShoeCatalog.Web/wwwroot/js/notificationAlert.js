
const alertNotification = {
        success: function(message, title) {
            toastr.success(message, title);
        },
        warning: function (messag, title) {
            toastr.warning(messag, title);
        },
        error: function (message, title) {
            toastr.error(message, title);
        }
}

export default alertNotification;