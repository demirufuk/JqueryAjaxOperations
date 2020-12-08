
function ShowImagePreview(imageUploader, previewImage) {
    if (imageUploader.files && imageUploader.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(previewImage).attr('src', e.target.result);
        };
        reader.readAsDataURL(imageUploader.files[0]);
    }
}

function jQueryAjaxPost(form) {
    $.validator.unobtrusive.parse(form);
    if ($(form).valid()) {
        var ajaxConfig =
        {
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (response) {
                if (response.success) {

                    $("#firstTab").html(response.html);
                    refreshAddNewTab($(form).attr('data-resetUrl'), true);
                }
                else {
                    //error
                }

            }
        };

        if ($(form).attr('enctype') == "mutipart/form-data") {
            ajaxConfig["contentType"] = false,
                ajaxConfig["processData"] = false;
        }

        $.ajax(ajaxConfig);
    }
    return false;
}

function refreshAddNewTab(resetUrl, showViewTab) {
    $.ajax({
        type: 'GET',
        url: resetUrl,
        success: function (response) {
            $('#secondTab').html(response);
            $('ul.nav.nav-tabs a:eq(1)').html('Add New');
            if (showViewTab) {
                $('ul.nav.nav-tabs a:eq(0)').tab('show');
            }
        }
    });
}

function Edit(url) {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (response) {
            $('#secondTab').html(response);
            $('ul.nav.nav-tabs a:eq(1)').html('Edit');
            $('ul.nav.nav-tabs a:eq(1)').tab('show');
        }
    });
}

function Delete(url) {
    if (confirm('Silmek istediğinize emin misiniz?') == true) {

        $.ajax({
            type: 'POST',
            url: url,
            success: function (response) {
                if (response.success) {
                    $('#firstTab').html(response.html);
                }
            }
        });
    }
}

