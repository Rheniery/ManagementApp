$(document).ready(function () {
    $("form").submit(function (event) {
        var valid = true;
        var selectedMember = $('#MemberType option:selected').val();
        if (selectedMember == "2") {
            if ($("#Date_StartTerm").val() == "") {
                $("#spanStartDate").css("display", "block");
                valid = false;
            } else if ($("#Date_EndTerm").val() == "") {
                $("#spanEndDate").css("display", "block");
                valid = false;
            }
        } else if (selectedMember == "1") {
            var countRole = $('#RoleId option:selected').val();
            var countTags = $('#listTags option:selected').length;
            var selectedTag = $('#listTags option:selected').val();
            console.log(countRole);
            console.log(countTags);
            if (countRole == "0") {
                $("#spanRole").css("display", "block");
                valid = false;
            } else if (countTags == "0") {
                $("#spanTags").css("display", "block");
                valid = false;
            } else if (countTags == "1" || selectedTag == "0") {
                $("#spanTags").css("display", "block");
                valid = false;
            }
        }

        if (!valid)
            event.preventDefault();
    });

    $('#MemberType').change(function () {
        var selectedId = $(this).children("option:selected").val();
        if (selectedId == "1") {
            $("#divEmployee").css("display", "block");
            $("#divContractor").css("display", "none");
            $("#Date_StartTerm").val('');
            $("#Date_EndTerm").val('');
        } else if (selectedId == "2") {
            $("#divEmployee").css("display", "none");
            $("#divContractor").css("display", "block");
            $("#RoleId").val('');
            $("#listTags").val('');
        } else {
            $("#divEmployee").css("display", "none");
            $("#divContractor").css("display", "none");
            $("#RoleId").val('');
            $("#Date_StartTerm").val('');
            $("#Date_EndTerm").val('');
        }
    });
    $('input[name=Date_StartTerm]').click(function () {
        $("#spanStartDate").css("display", "none");
    });
    $('input[name=Date_EndTerm]').click(function () {
        $("#spanEndDate").css("display", "none");
    });
    $('input[name=RoleId]').click(function () {
        $("#spanRole").css("display", "none");
    });
    $('input[name=listTags]').click(function () {
        $("#spanTags").css("display", "none");
    });

    //Load listTags - multiselect
    var listTagsLoaded = $("#listTagsLoaded").val().split(",");
    $('select[name=listTags]').selectpicker('val', listTagsLoaded);
    $('select[name=listTags]').selectpicker('refresh')
});