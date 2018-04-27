(function (nm, $) {
    nm.init = function () {
        $.ajaxSetup({
            cache: false,
            error: function (xhr) {
                $('#modal').removeClass('loading');
                $('#dvIndex').html(xhr.responseText);
            }
        });    
    };
})(window.WorkerAgent = window.WorkerAgent || {}, jQuery);

$(document).ajaxStart(function () {
    $('#modal').addClass('loading');
}).ajaxStop(function () {
    $('#modal').removeClass('loading');
});
