
$(document).ready(function () {

    TooltipImages();

    function TooltipImages() {
        $(".gridImages").tooltip({
            track: true,
            delay: 0,
            showURL: false,
            fade: 100,
            bodyHandler: function () {
                return $($(this).next().html());
            },
            showURL: false
        });
    }
});
