
$(document).ready(function () {
    $(".username").focus(function () {
        $(".user-icon").css("left", "-48px");
    });
    $(".username").blur(function () {
        $(".user-icon").css("left", "0px");
    });

    $(".password").focus(function () {
        $(".pass-icon").css("left", "-48px");
    });
    $(".password").blur(function () {
        $(".pass-icon").css("left", "0px");
    });

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
