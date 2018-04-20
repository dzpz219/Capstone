$(document).ready(function () {
    window.clearThreadModal = function() {
        $('#NewThreadModal').modal('toggle');
        $('#ThreadTopicTextBox').val("");
        $('#ThreadContentTextArea').val("");
        $('#threads-pagination').twbsPagination('show', 1)
    }
    window.clearPostTextArea = function() {
        $('#NewPostContent').val("");
    }
    var maxPages;
    $.ajax({
        type: "POST",
        url: "/Discussion/getMaxThreads",
        data: {"search": $('#search').val()},
        success: function (result) {
            maxPages = parseInt(result);
        },
        error: function (result) {
            console.log(result);
        }
    }).done(function () {
        $('#threads-pagination').twbsPagination({
            totalPages: maxPages,
            visiblePages: 10,
            initiateStartPageClick: false,
            onPageClick: function (event, page) {
                loadpage(page);
            }
        })
    })
    
    var resp = "";
    function loadpage(page) {
        $.ajax({
            type: "POST",
            url: "/Discussion/SearchThreads",
            data: {"page": page, "search": $('#search').val()},
            success: function (result) {
                resp = result;
            },
            error: function (result) {
                console.log(result);
            }
        }).done(function () {
            $('#ThreadsTable').html(resp);
        });
    }

    window.removePagination = function () {
        $('#threads-pagination').twbsPagination('destroy');
        $.ajax({
            type: "POST",
            url: "/Discussion/getMaxThreads",
            data: { "search": $('#search').val() },
            success: function (result) {
                maxPages = (result == "0") ? 1 : parseInt(result);
            },
            error: function (result) {
                console.log(result);
            }
        }).done(function () {
            $('#threads-pagination').twbsPagination({
                totalPages: maxPages,
                visiblePages: 10,
                initiateStartPageClick: false,
                onPageClick: function (event, page) {
                    loadpage(page);
                }
            })
        })
    }

    $('#posts-pagination').twbsPagination({
        totalPages: $('#MaxPages').val(),
        visiblePages: 3,
        initiateStartPageClick: false,
        onPageClick: function (event, page) {
            loadposts(page);
        }
    })

    function loadposts(page) {
        $.ajax({
            type: "GET",
            url: "/Discussion/Posts",
            data: { "Page": page, "ThreadId": $('#ThreadId').val(), "Topic": $('#Topic').text(), "Author": $('#Author').val() },
            success: function (result) {
                resp = result;
            },
            error: function (result) {
                console.log(result);
            }
        }).done(function () {
            $('#PostsPartial').html(resp);
        });
    }
});

