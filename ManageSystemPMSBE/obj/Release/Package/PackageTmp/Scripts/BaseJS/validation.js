$("input").click(function () {
    $("#form_data").each(function () {
        var inputs = $(this).find(':input[required]');
        for (var i = 0; i < inputs.length; i++) {
            inputs[i].classList.remove("border-warning");
            if ($(inputs[i].parentElement).find('span.required-field').length > 0)
                $(inputs[i].parentElement).find('span.required-field')[0].textContent = "(*)";
        }
        var selects = $(this).find('select[required]');
        for (i = 0; i < selects.length; i++) {
            selects[i].classList.remove("border-warning");
            if ($(selects[i].parentElement).find('span.required-field').length > 0)
                $(selects[i].parentElement).find('span.required-field')[0].textContent = "(*)";
        }
    });
});
$("select").click(function () {
    $("#form_data").each(function () {
        var inputs = $(this).find(':input[required]');
        for (var i = 0; i < inputs.length; i++) {
            inputs[i].classList.remove("border-warning");
            if ($(inputs[i].parentElement).find('span.required-field').length > 0)
                $(inputs[i].parentElement).find('span.required-field')[0].textContent = "(*)";
        }
        var selects = $(this).find('select[required]');
        for (i = 0; i < selects.length; i++) {
            selects[i].classList.remove("border-warning");
            if ($(selects[i].parentElement).find('span.required-field').length > 0)
                $(selects[i].parentElement).find('span.required-field')[0].textContent = "(*)";
        }
    });
});