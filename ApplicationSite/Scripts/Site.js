// TODO: Testing as a global function.
function setupSiteModal(containerName, modalName, buttonName) {
    var url = $(buttonName).data('url');
    $.ajax({
        url: url,
        contentType: 'application/html; charset=utf-8',
        dataType: "html",
        data: "",
        async: true,
        cache: false,
        type: 'POST',
        success: function (data) {
            $(containerName).html(data);
            console.log(data);
            $(modalName).modal('show');
        }
    });
};

$.ajaxSetup({
    cache: false
});