/* Category modal js */


function Create() {
    $.get("/Admin/Category/Create",
        function (result) {
            $("#myModal").modal();
            $("#myModalLabel").html("گروه جدید");
            $("#myModalBody").html(result);
            debugger;
        });
}

function Edit(id) {
    $.ajax({
        url: "/Admin/Category/Edit/",
        data: { id }
    }).done(function (result) {
        $("#myModal").modal();
        $("#myModalLabel").html("ویرایش گروه");
        $("#myModalBody").html(result);
    });
}

function Delete(id) {
    $.ajax({
        url: "/Admin/Category/Delete/",
        data: { id }
    }).done(function (result) {
        $("#myModal").modal();
        $("#myModalLabel").html("حذف گروه");
        $("#myModalBody").html(result);
    });
}

/* Movie modal js */

function DeleteMovie(id) {
    $.ajax({
        url: "/Admin/Movies/Delete/",
        data: { id }
    }).done(function (result) {
        $("#myModal").modal();
        $("#myModalLabel").html("حذف گروه");
        $("#myModalBody").html(result);
    });
}

function SetLinkToMovie(id) {
    $.ajax({
        url: "/Admin/Movies/LinkToMovies",
        data: { id }
    }).done(function (result) {
        $("#myModal").modal();
        $("#myModalLabel").html("افزودن لینک به فیلم");
        $("#myModalBody").html(result);
    });
}

/* Links js */

function DeleteLink(id) {
    $.ajax({
        url: "/Admin/MovieLinks/Delete/",
        data: { id }
    }).done(function (result) {
        $("#myModal").modal();
        $("#myModalLabel").html("حذف لینک");
        $("#myModalBody").html(result);
    });
}

// Call the 'DownloadCount' method.
function count() {
    $.ajax({
        url: "/Movies/DownloadCount",
        type: "POST"
    }).done(function () {
    });
}

function ChangePage(pageId) {
    $("#pageId").val(pageId);
    $("#filterForm").submit();
}