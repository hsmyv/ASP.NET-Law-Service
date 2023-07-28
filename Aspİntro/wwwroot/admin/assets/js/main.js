$(document).ready(function () {

    $(document).on("click", ".set-default", function () {
        let PostId = parseInt($(".post-id").val());
        let ImageId = parseInt($(".set-default").attr("data-id"));

        $.ajax({
            url: "/AdminArea/Post/SetDefaultImage",
            data: {
                postId: PostId,
                imageId: ImageId
            },
            type: "POST",
            success: function (res) {
                Swal.fire({
                    position: 'top-end',
                    icon: 'success',
                    title: "Image default success",
                    showConfirmButton: false,
                    timer: 1500
                }).then(function () {
                    window.location.reload();
                })
            }
        })
    })
})