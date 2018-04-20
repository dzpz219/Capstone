$(document).ready(function () {
    $('#NewMessageModal').on('show.bs.modal', function (event) {
        var anchor = $(event.relatedTarget);
        var recipient = anchor.data('sender');
        var subject = anchor.data('subject');
        var content = anchor.data('content');
        var modal = $(this);
        if (recipient == undefined) {
            modal.find('.modal-title').text('New Message');
            modal.find('.modal-body #recipient-name').val('');
            modal.find('.modal-body #subject').val('');
            modal.find('.modal-body #content').val('');
            modal.find('.modal-body #message').val('');
            modal.find('.modal-body #content-input').hide();
        }
        else {
            modal.find('.modal-title').text('Reply to ' + recipient);
            modal.find('.modal-body #recipient-name').val(recipient);
            modal.find('.modal-body #subject').val(subject);
            modal.find('.modal-body #content').text(content);
            modal.find('.modal-body #content-input').show();
            modal.find('.modal-body #message').val('');
        }
    })

    $('#SentMail').click(function () {
        $.ajax({
            type: "GET",
            url: "/Message/SentMessages",
            success: function (result) {
                $('#SentMailTable').html(result);
            },
            error: function (result) {
                console.log(result);
            }
        });
    })

    $('#InboxMail').click(function () {
        $.ajax({
            type: "GET",
            url: "/Message/Messages",
            success: function (result) {
                $('#InboxTable').html(result);
            },
            error: function (result) {
                console.log(result);
            }
        });
    })

    window.clearMessage = function () {
        $('#NewMessageInput').val('');
    }
});