$(function () {
    kendoUI.window_widget = function () {
        var e = $("#kUI_window");
        var t = $("#kUI_undo");
        if (e.length) {
            var a = e,
                n = t.bind("click", function () {
                    a.data("kendoWindow").open(), n.hide()
                }),
                o = function () {
                    n.show();
                };
            a.data("kendoWindow") || a.kendoWindow({
                width: "1260px",
                title: "Thông tin chi tiết",
                actions: ["Pin", "Minimize", "Maximize", "Close"],
                close: o,
                position: {
                    top: 10,
                    left: "5%"
                }
            });
        }
    };
});