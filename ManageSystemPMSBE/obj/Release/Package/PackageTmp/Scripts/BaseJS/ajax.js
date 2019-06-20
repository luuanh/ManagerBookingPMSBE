function Post(url, data, done, fail = null, headers = {}, dataType = "json", always = null) {
    var ajaxPost = $.ajax({
        type: "POST",
        url: url,
        data: data,
        headers: headers,
        dataType: dataType
    }).done(done);
    if (fail !== null)
        ajaxPost.fail(fail);
    if (always !== null)
        ajaxPost.always(always);
}
function Get(url, done, fail = null, headers = {}, dataType = "json", always = null) {
    var ajaxGet = $.ajax({
        type: "GET",
        url: url,
        headers: headers,
        dataType: dataType
    }).done(done);
    if (fail !== null)
        ajaxGet.fail(fail);
    if (always !== null)
        ajaxGet.always(always);
}

function Put(url, data, done, fail = null, headers = {}, dataType = "json", always = null) {
    var ajaxPut = $.ajax({
        type: "Put",
        url: url,
        data: data,
        headers: headers,
        dataType: dataType
    }).done(done);
    if (fail !== null)
        ajaxPut.fail(fail);
    if (always !== null)
        ajaxPut.always(always);
}

function Delete(url, done, fail = null, headers = {}, dataType = "json", always = null) {
    var ajaxDelete = $.ajax({
        type: "Delete",
        url: url,
        headers: headers,
        dataType: dataType
    }).done(done);
    if (fail !== null)
        ajaxDelete.fail(fail);
    if (always !== null)
        ajaxDelete.always(always);
}