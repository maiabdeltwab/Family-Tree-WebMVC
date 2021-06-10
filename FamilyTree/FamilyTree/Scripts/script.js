$(function () {
    $('[data-toggle="tooltip"]').tooltip();

    $('.modal').on('shown.bs.modal', function () {
        $('#myInput').trigger('focus');
    });

    setChildsLocation();
});

function setChildsLocation() {
    var divs = $('.sub-childs');

    for (var div of divs) {
        var len = $(div).children('.child--row').length;
        if (len == 0) continue;

        var max_width = $(div).parent().width();
        var row_index = 0;

        var width = 0;
        var top = 35;
        var index = 0;
        var prev_width = 0;
        var setVeticalWidth = false;

        var childs = $(div).children('.childs');

        for (var child of childs) {
            index = (width == prev_width) ? 0 : index + 1;
            var parent_id = child.id;

            var i = 0;
            [i, parent_id] = parent_id.split('p');
            var parent = $(`#${parent_id}`);
            var position = parent.position();

            if (i > row_index) {
                var x = i - row_index;
                var element = parent;

                if (element.hasClass('col-3')) {
                    width += 70;
                }

                while (x > 0) {
                    element = element.prev();
                    width += element.width();
                    x--;
                }
            }

            row_index = i;
            row_index++;

            var row = $(child).next();
            var HLine = $(child).children(':first');
            var VLine = $(child).children(':last');

            if (parent.hasClass('col-2')) {
                HLine.css({ "top": `${position.top + 189}px`, "left": `${position.left + 42}px` });
                VLine.css({ "top": `${position.top + 189}px`, "left": `${position.left + 86}px` });
            }
            else if (parent.hasClass('right--partner')) {
                HLine.css({ "top": `${position.top + 189}px`, "left": `${position.left - 180}px` });
                VLine.css({ "top": `${position.top + 189}px`, "left": `${position.left - 135}px` });
            }
            else {
                HLine.css({ "top": `${position.top + 189}px`, "left": `${position.left + 124}px` });
                VLine.css({ "top": `${position.top + 189}px`, "left": `${position.left + 169}px` });
            }
            row.css({
                "position": "relative",
                "top": `${top - 224 * index}px`,
                "margin-left": `${width}px`
            });

            if (setVeticalWidth) {
                var child_len = row.children('.sub-childs').first().children().length;

                var h = row.position().top - row.height() - child_len / 2 * 220 + 23;

                VLine.css({ 'height': `${h}px`, 'border': '1px solid #e4e4e4' });
            }

            var row_width = row.width();
            prev_width = width + row_width;
            width = prev_width + 35;

            if (width >= max_width) {
                width = -55;
                prev_width = -55;
                top -= 185;
                setVeticalWidth = true;
            }
            else if (width >= max_width - 133) {
                prev_width -= 100;
                width = prev_width;
                top -= 200;
                setVeticalWidth = true;
            } else {
                setVeticalWidth = false;
            }
        }
    }

    $(divs[0]).children('.child--row')
        .first().css('margin-left', '-224px');
}

function create() {
    $.ajax({
        type: "GET",
        url: `/Home/Create`,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (response) {
            $('#create-modal').html(response);
            $("#createModal").modal("show");
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}

function edit(id) {
    $('#detailsModal').modal('hide').removeClass('show');
    $('.modal-backdrop ').remove();

    $.ajax({
        type: "GET",
        url: `/Home/Edit/${id}`,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (response) {
            $('#create-modal').html(response);
            $("#updateModal").modal("show");
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}

function details(id) {
    $.ajax({
        type: "GET",
        url: `/Home/Details/${id}`,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (response) {
            $('#create-modal').html(response);
            $("#detailsModal").modal("show");
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}

function remove(id) {
    $('#detailsModal').modal('hide');
    $('.modal-backdrop ').remove();

    $('#personId').val(id);
    $("#deleteModal").modal("show");
}

function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#personImage').attr('src', e.target.result);
        }
        reader.readAsDataURL(input.files[0]);
    }
}

function removeImage() {
    $("#Image")[0].value = "";
    $("#RemoveImage")[0].value = "true";
    $('#personImage').attr('src', '/Content/imgs/user-vector-male.png');
}