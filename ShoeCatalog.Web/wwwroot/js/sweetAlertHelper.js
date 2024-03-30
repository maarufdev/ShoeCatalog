

const sweetAlertHelper = {
    warningDelete: function (item) {
        return Swal.fire({
            title: 'Are you sure?',
            text: `You won't be able to revert ${item}!`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        })
    },
    success: function (title) {
        return Swal.fire({
            title: title,
            icon: 'success'
        })
    },
    error: function (title) {
        return Swal.fire({
            title, title,
            icon: 'error'
        })
    }
}

export default sweetAlertHelper;