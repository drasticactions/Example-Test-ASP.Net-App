// TODO: Testing as a global function.
function setupSiteModal(containerName, modalName) {
    var url = $(modalName).data('url');

    $.get(url, function(data) {
        $(containerName).html(data);

        $(modalName).modal('show');
    });
};