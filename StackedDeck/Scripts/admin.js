$(document).ready(function () {
    var maxPages;
    $.ajax({
        type: "POST",
        url: "/Admin/getMaxUsers",
        data: {"search": $('#search').val()},
        success: function (result) {
            maxPages = parseInt(result);
        },
        error: function (result) {
            console.log(result);
        }
    }).done(function () {
        $('#admin-pagination').twbsPagination({
            totalPages: maxPages,
            visiblePages: 5,
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
            url: "/Admin/SearchUsers",
            data: {"page": page, "search": $('#search').val()},
            success: function (result) {
                resp = result;
            },
            error: function (result) {
                console.log(result);
            }
        }).done(function () {
            $('#UserTable').html(resp);
        });
    }
    $("#uploadBtn").click(function () {
        var data = new FormData();
        var adNameInput = $('#adInput').val();
        var files = $("#uploadedFile").get(0).files;
        data.append("adName", adNameInput);
        if (files.length > 0) {
            data.append("uploadedFile", files[0]);
        }
        var ajaxRequest = $.ajax({
            type: "POST",
            url: "/Admin/Upload",
            xhr: function () {
                var xhr = $.ajaxSettings.xhr();
                if (xhr.upload) {
                    xhr.upload.addEventListener('progress', function (evt) {
                        var percent = (evt.loaded / evt.total) * 100;
                        $(".progress").removeClass("hidden");
                        $(".progress-bar").removeClass("hidden").width(percent + "%");
                    }, false);
                }
                return xhr;
            },
            contentType: false,
            processData: false,
            data: data,
            complete: function (result) {
                setTimeout(function () {
                    $("#adsPartial").html(result.responseText);
                    $('#imagePreview').attr('src', "a").addClass("hidden");
                    $("#adNameInput").addClass("hidden");
                    $("#adInput").val("");
                    $('#uploadBtn').prop('disabled', true);
                    $(".progress-bar").width("0%").addClass("hidden");
                    $(".progress").addClass("hidden");
                }, 500);
            },
            error: function (response) {
                console.log(response.responseText);
            }
        });
    });

    window.removePagination = function () {
        $('#admin-pagination').twbsPagination('destroy');
        $.ajax({
            type: "POST",
            url: "/Admin/getMaxUsers",
            data: { "search": $('#search').val() },
            success: function (result) {
                maxPages = (result == "0") ? 1 : parseInt(result);
            },
            error: function (result) {
                console.log(result);
            }
        }).done(function () {
            $('#admin-pagination').twbsPagination({
                totalPages: maxPages,
                visiblePages: 5,
                initiateStartPageClick: false,
                onPageClick: function (event, page) {
                    loadpage(page);
                }
            })
        })
    }

    $('#UserInfoModal').on('show.bs.modal', function (event) {
        var anchor = $(event.relatedTarget);
        var un = anchor.data('un');
        var fn = anchor.data('fn');
        var ln = anchor.data('ln');
        var q = anchor.data('q');
        var a = anchor.data('a');
        var e = anchor.data('e');
        var ph = anchor.data('ph');
        var po = anchor.data('po');
        var add = anchor.data('add');
        var c = anchor.data('c');
        var prov = anchor.data('prov');
        var cr = anchor.data('cr');
        var rd = anchor.data('rd');

        var modal = $(this);
        modal.find('.modal-title').text('User Information for: ' + un);
        modal.find('.modal-body #un').val(un);
        modal.find('.modal-body #fn').val(fn);
        modal.find('.modal-body #ln').val(ln);
        modal.find('.modal-body #q').val(q);
        modal.find('.modal-body #a').val(a);
        modal.find('.modal-body #e').val(e);
        modal.find('.modal-body #ph').val(ph);
        modal.find('.modal-body #po').val(po);
        modal.find('.modal-body #add').val(add);
        modal.find('.modal-body #CountryDropDown').val(c);
        modal.find('.modal-body #ProvinceDropDown').val(prov);
        modal.find('.modal-body #cr').text(cr);
        modal.find('.modal-body #rd').text(rd);
        modal.find('.modal-footer #BanUser').val(un);

        if ($('#CountryDropDown').val() == 'Canada') {
            $('#ProvinceInput').removeClass('hidden');
        }
        else {
            $('#ProvinceInput').addClass('hidden');
            $('#ProvinceDropDown').val('');
        }
    })

    $('#DeleteModal').on('show.bs.modal', function (event) {
        var anchor = $(event.relatedTarget);
        var id = anchor.data('id');
        var title = anchor.data('title') + "?";
        var modal = $(this);
        modal.find('.modal-title').text('Delete ' + title);
        if (title != "Report?") {
            modal.find('.modal-body #DeleteAd').val(id);
            $('#ReportForm').hide();
            $('#AdForm').show();
        }
        else {
            modal.find('.modal-body #DeleteReport').val(id);
            $('#AdForm').hide();
            $('#ReportForm').show();
        }
    })

    window.hidemodal = function () {
        $('#DeleteModal').modal('hide');
    }

    $('#AdminNav li a[data-toggle="tab"]').click(function () {
        event.preventDefault();
        var $anchor = $(this);

        var view = $anchor.data('v');
        var data = $anchor.data('d');

        $.ajax({
            type: "POST",
            url: "/Admin/Partials",
            xhr: function () {
                var xhr = $.ajaxSettings.xhr();
                if (xhr.upload) {
                    xhr.upload.addEventListener('progress', function (evt) {
                        var percent = (evt.loaded / evt.total) * 100;
                        $("#ReportsProgress").removeClass("hidden");
                        $("#ReportsProgressBar").removeClass("hidden").width(percent + "%");
                    }, false);
                }
                return xhr;
            },
            data: { "V":view, "D":data },
            success: function (result) {
                $('#' + view).html(result);
                $('#' + data + 'ProgressBar').width("0%").addClass("hidden");
                $('#' + data + 'Progress').addClass("hidden");
            },
            error: function (result) {
                console.log(result);
            }
        })
    });
});

