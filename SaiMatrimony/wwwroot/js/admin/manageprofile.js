$(function () {

    loadProfiles();

    $(document).on('click', '#searchprofile', function () {
        loadUserRole();
    });

    $(document).on('keyup', '#profile-first-name', function () {
        loadUserRole();
    });
    $(document).on('keyup', '#profile-last-name', function () {
        loadUserRole();
    });
    $(document).on('keyup', '#profile-email', function () {
        loadUserRole();
    });
    $(document).on('change', '#show-all-profiles', function () {
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
    function loadProfiles() {

        $("#profile-table").show();
        var firstName = $("#profile-first-name").val();
        var lastName = $("#profile-last-name").val();
        var email = $("#profile-email").val();
        var all = $('#show-all-profiles').is(":checked") ? "y" : "n";
        var loadingHtml = $('#show-all-profiles').is(":checked") ? "showing all profiles : " : "showing profiles who need approval : ";

        var Url = '/admin/Profile?firstname=' + firstName + '&lastname=' + lastName + '&email=' + email + '&all=' + all;
        $.ajax({
            type: "POST",
            url: Url,
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                $("#profile-table-msg").html(loadingHtml + result.length);
                if (result.length > 0) {
                    $('#profile-pagination').pagination({
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
                            $("#profile-pagination").prev().html(userTableHtml);

                        }
                    });
                }
                else {
                    $("#profile-table").hide();
                }
            },
            error: function (errorMsg) {
                $("#profile-table").hide();
            }
        });
    }
    
    $(document).on('click', ".edit-profile", function () {        
        $("#profileid").val($(this).data('profileid'));
        $("#profileapproval").val($(this).data('approval'));

        if ($(this).data('approval') == "y") {
            $('#lastUpdatedProfile').html('You are about to approve the profile' + $(this).data('profilename'));
        } else {
            $('#lastUpdatedProfile').html('You are about to reject the profile' + $(this).data('profilename'));
        }        
        
        $("#profileModal").modal();
    });

    $(document).on('click', ".saveApprovalProfile", function () {
        var profile = $("#profileid").val();
        var approved = $("#profileapproval").val();
        $.ajax({
            type: "POST",
            url: "/Admin/ManageProfile?profileid=" + profile + "&approved=" + approved,            
            success: function (result) {
                if (result.key == "y") {
                    $('#lastUpdatedProfile').html('Updated Successfully <span style="color:green" class="glyphicon glyphicon-ok-circle"> </span>');
                    $('#saveProfile').removeAttr('disabled');
                    loadUserRole();
                }
                else {
                    $('#lastUpdatedUser').html('Updated Failed <span style="color:red" class="glyphicon glyphicon-remove-circle"> </span>');
                    $('#saveProfile').removeAttr('disabled');
                }
            },
            error: function (errorMsg) {
                $('#saveProfile').html('<span class="alert alert-warning">' + errorMsg.statusText + '</span> ');
            }
        });
    });

})


