$(document).ready(function () {
    var recipient = $('#recipient').val();

    setInterval(function () {
        $.ajax({
            type: "POST",
            url: "/Message/MessageReload",
            data: { "Sender": recipient },
            success: function (result) {
                $('#ConversationTab').html(result);
            },
            error: function (result) {
                console.log(result);
            }
        });
    }, 5000);
});