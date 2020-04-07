$(function () {

    loadUserRole();

    $(document).on('click', '#searchUser', function () {
        loadUserRole();
    });

    $(document).on('keyup', '#first-name', function () {
        loadUserRole();
    });
    $(document).on('keyup', '#last-name', function () {
        loadUserRole();
    });
    $(document).on('keyup', '#email', function () {
        loadUserRole();
    });
    $(document).on('change', '#show-all-users', function () {
        loadUserRole();
    });

    $(document).on('click', '#show-all-label', function () {
        $('#show-all-users').click();
        loadUserRole();
    });

    $("#adminuser").change(function () {

        var isadmin = $(this).is(":checked");
        var isbasic = $("#basicuser").is(":checked")

        if (isadmin) {
            $("#adminmsg").show();
        }
        else {
            $("#adminmsg").hide();
        }

        if (isadmin && !isbasic) {
            $("#basicuser").click();
        }

        if (!isadmin && isbasic) {
            $("#basicuser").click();
        }


    });

    // Assign Role to User
    function loadUserRole() {

        console.log("loading..")

        $("#user-table").show();
        var firstName = $("#first-name").val();
        var lastName = $("#last-name").val();
        var email = $("#email").val();
        var all = $('#show-all-users').is(":checked") ? "y" : "n";
        var loadingHtml = $('#show-all-users').is(":checked") ? "showing all users : " : "showing users who need access : ";

        var Url = '/admin/Userwithrole?firstname=' + firstName + '&lastname=' + lastName + '&email=' + email + '&all=' + all;
        $.ajax({
            type: "POST",
            url: Url,
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                $("#user-table-msg").html(loadingHtml + result.length);
                if (result.length > 0) {
                    $('#user-pagination').pagination({
                        dataSource: result,
                        pageSize: 10,
                        showGoInput: true,
                        showGoButton: true,
                        callback: function (data, pagination) {
                            var userTableHtml = '<table class="table"><tr><th>First Name</th><th>Middle Name</th><th>Last Name</th><th>Email</th><th>Role</th><th>Update</th></tr>';
                            data.forEach(function (item) {
                                var dataHtml = ' data-userid="' + item.userId + '" data-username="' + item.firstName + '"  data-rolename="' + item.role + '"';
                                var btnHtml = ' <button class="btn btn-default edit-user" ' + dataHtml + ' >Map</button>'
                                userTableHtml += '<tr><td>' + item.firstName + '</td><td>' + item.middleName + '</td><td>' + item.lastName + '</td><td>' + item.email + '</td><td>' + item.role + '</td><td>' + btnHtml + '</td></tr>';
                            })
                            userTableHtml += '</table>';
                            $("#user-pagination").prev().html(userTableHtml);

                        }
                    });
                }
                else {
                    $("#user-table").hide();
                }
            },
            error: function (errorMsg) {
                $("#user-table").hide();
            }
        });
    }


    $("#roleForUserAutoComplete").autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/Admin/SearchRole?search=" + $("#roleForUserAutoComplete").val(),
                data: "{ 'searchText' : '" + $("#roleForUserAutoComplete").val() + "'}",
                dataFilter: function (data) { return data; },
                dataType: "json",
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.roleName,
                            value: item.roleName,
                            task: item.roleId
                        }
                    }))
                },
                error: function (result) {
                    alert("Error");
                }
            });
        },
        select: function (event, ui) {
            $("#roleForUserAutoComplete").val(ui.item.label);
            $("#roleForUser").val(ui.item.task);
        }
    });

    $(document).on('click', ".edit-user", function () {
        $("#userId").val($(this).data("userid"));
        $("#currentRole").html('Current Role : <span style="text-decoration:underline;">' + $(this).data("rolename") + '</span>');
        $("#user-name").html($(this).data('username'));
        $('#lastUpdatedUser').html('');

        var isadmin = $("#adminuser").is(":checked");
        var isbasic = $("#basicuser").is(":checked");

        if ($(this).data("rolename") == "Admin") {
            if (!isadmin) {
                $("#adminuser").prop('checked', true);
            }
            if (!isbasic) {
                $("#basicuser").prop('checked', true);
            }
        }
        else if ($(this).data("rolename") == "Basic") {
            if (isadmin) {
                $("#adminuser").prop('checked', false);
            }
            if (!isbasic) {
                $("#basicuser").prop('checked', true);
            }
        }
        else {
            if (isadmin) {
                $("#adminuser").prop('checked', false);
            }
            if (isbasic) {
                $("#basicuser").prop('checked', false);
            }
        }

        $("#userModal").modal();
    });

    $(document).on('click', "#saveUserRole", function () {
        var userId = $("#userId").val();
        var isadmin = $("#adminuser").is(":checked");
        var isbasic = $("#basicuser").is(":checked")


        var mapid = isbasic ? "1" : "-1";

        var workFlowUserMap = {
            MapId: mapid,
            UserIdSystem: userId,
            IsAdmin: isadmin,
            UpdateByName: "0",
            UpdateById: "0",
            UpdateDate: new Date()
        }

        $.ajax({
            type: "POST",
            url: "/Admin/ManageUserRole/",
            data: JSON.stringify(workFlowUserMap),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.key == "y") {
                    $('#lastUpdatedUser').html('Updated Successfully <span style="color:green" class="glyphicon glyphicon-ok-circle"> </span>');
                    $('#saveUseRole').removeAttr('disabled');
                    loadUserRole();
                }
                else {
                    $('#lastUpdatedUser').html('Updated Failed <span style="color:red" class="glyphicon glyphicon-remove-circle"> </span>');
                    $('#saveUseRole').removeAttr('disabled');
                }
            },
            error: function (errorMsg) {
                $('#lastUpdatedUser').html('<span class="alert alert-warning">' + errorMsg.statusText + '</span> ');
            }
        });
    });

})


