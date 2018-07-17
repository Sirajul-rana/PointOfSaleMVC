$(document).ready(function () {
    var sideslider = $('[data-toggle=collapse-side]');
    var sel = sideslider.attr('data-target');
    var sel2 = sideslider.attr('data-target-2');
    sideslider.click(function () {
        $(sel).toggleClass('in');
        $(sel2).toggleClass('out');
    });

    /*
     * Category Setup page code start here
     */


    //function readUrl(input) {
    //    if (input.files && input.files[0]) {
    //        var reader = new FileReader();

    //        reader.onload = function (e) {
    //            $('#preview').attr('src', e.target.result);
    //        }

    //        reader.readAsDataURL(input.files[0]);
    //    }
    //}

    //$("#CategoryImage").change(function () {
    //    readUrl(this);
    //});
    function getCategories() {
        $.get("/Setup/GetParentCategories/", function (data) {
            var markup = "<option value=''>Select Category</option>";
            for (var x = 0; x < data.length; x++) {
                markup += "<option value=" + data[x].Value + ">" + data[x].Text + "</option>";
            }
            $("#RootCategoryId").html(markup).show();

        });
    }

    function loadCategoryTable() {
        $.get("/Setup/GetAllCategories/", function (data) {
            var tblHtml = "";
            $.each(data, function (i, val) {
                tblHtml += "<tr><td></td><td>" + val.CategoryType + "</td>";
                tblHtml += "<td>" + val.RootCategory.CategoryName + "</td>";
                tblHtml += "<td>" + val.CategoryName + "</td>";
                tblHtml += "<td>" + val.CategoryCode + "</td>";
                tblHtml += "<td>" + val.CategoryDescription + "</td>";
                tblHtml += '<td><button type="button" id="btnEdit" class="btn btn-primary btnEdit"><i class="fa fa-edit"></i></button>';
                tblHtml += '<button type="button" class="btn btn-danger btnDelete" id="btnDelete"><i class="fa fa-remove"></i></button></td></tr>';
            });

            $("#categoryTableBody").html(tblHtml);
        });
    }




    //$("#tableBody").click(function() {
    //    loadCategoryTable();
    //});

    $("#childCategory").change(function () {
        $("#RootCategoryId").removeAttr("disabled");
        $("#CategoryName").val("");
        $("#CategoryCode").val("");
        $("#CategoryDescription").val("");
        $('#rootCategory').attr('checked', false);
        $('#childCategory').attr('checked', true);
        getCategories();

    });

    $("#rootCategory").change(function () {
        $("#RootCategoryId").find('option:not(:first)').remove();
        $("#CategoryName").val("");
        $("#CategoryCode").val("");
        $("#CategoryDescription").val("");
        $('#rootCategory').attr('checked', true);
        $('#childCategory').attr('checked', false);
        $("#RootCategoryId").prop("disabled", "disabled");
    });

    $("#CategoryName").change(function () {
        var code = ($("#CategoryName").val()).slice(0, 3);
        $("#CategoryCode").val(code.toLocaleUpperCase());
    });
    $("#cancelCategoryButton").click(function () {
        $("#rootCategory").prop("checked", "checked");
        $("#RootCategoryId").prop("disabled", "disabled");
        $("#RootCategoryId").find('option:not(:first)').remove();
        $("#CategoryName").val("");
        $("#CategoryCode").val("");
        $("#CategoryDescription").val("");
        alertify.error("All Cleared");
    });

    //Validation For course assigning Page
    $("#saveCategoryForm").validate({
        rules: {
            RootCategoryId: {
                required: true
            },
            CategoryName: {
                required: true
            },
            CategoryCode: {
                required: true
            },
            CategoryDescription: {
                required: true
            }
        },
        messages: {
            RootCategoryId: {
                required: "Please select an option"
            },
            CategoryName: {
                required: "Please enter category name"
            },
            CategoryCode: {
                required: "Please enter category code"
            },
            CategoryDescription: {
                required: "Please enter category description"
            }
        }
    });


    $("#saveCategoryButton").click(function () {
        if ($("#saveCategoryForm").valid()) {
            var categoryName = $("#CategoryName").val();
            var categoryCode = $("#CategoryCode").val();
            var categoryDes = $("#CategoryDescription").val();
            var rootCategoryId = $("#RootCategoryId").val();

            if ($("#rootCategory").attr("checked") === "checked") {
                $.post("/Setup/SaveCategory/",
                    {
                        CategoryName: categoryName,
                        CategoryCode: categoryCode,
                        CategoryDescription: categoryDes,
                        CategoryType: "root"
                    },
                    function (data, status) {
                        if (status === "success") {
                            $("#rootCategory").prop("checked", "checked");
                            $("#RootCategoryId").prop("disabled", "disabled");
                            $("#RootCategoryId").find('option:not(:first)').remove();
                            $("#CategoryName").val("");
                            $("#CategoryCode").val("");
                            $("#CategoryDescription").val("");
                            $('#rootCategory').attr('checked', true);
                            $('#childCategory').attr('checked', false);
                            loadCategoryTable();
                            alertify.success("Data: " + data + "\nStatus: " + status);
                        } else {
                            loadCategoryTable();
                            alertify.error("Data: " + data + "\nStatus: " + status);
                        }

                    });
            }

            if ($("#childCategory").attr("checked") === "checked") {
                $.post("/Setup/SaveCategory/",
                    {
                        RootCategoryId: rootCategoryId,
                        CategoryName: categoryName,
                        CategoryCode: categoryCode,
                        CategoryDescription: categoryDes,
                        CategoryType: "child"
                    },
                    function (data, status) {
                        if (status === "success") {
                            $("#rootCategory").prop("checked", "checked");
                            $("#RootCategoryId").prop("disabled", "disabled");
                            $("#RootCategoryId").find('option:not(:first)').remove();
                            $("#CategoryName").val("");
                            $("#CategoryCode").val("");
                            $("#CategoryDescription").val("");
                            $('#rootCategory').attr('checked', true);
                            $('#childCategory').attr('checked', false);
                            loadCategoryTable();
                            alertify.success("Data: " + data + "\nStatus: " + status);
                        } else {
                            loadCategoryTable();
                            alertify.error("Data: " + data + "\nStatus: " + status);
                        }

                    });
            }

        }
    });
    /*
     * Category Setup page code ends here
     */

    /*
     * Item Setup page code starts here
     */
    function loadItemTable() {
        $.get("/Setup/GetAllItems/", function (data) {
            var tblHtml = "";
            $.each(data, function (i, val) {
                tblHtml += "<tr><td></td><td>" + val.Category.CategoryName + "</td>";
                tblHtml += "<td>" + val.ItemName + "</td>";
                tblHtml += "<td>" + val.Category.CategoryCode + "-" + val.ItemCode + "</td>";
                tblHtml += "<td>" + val.ItemDescription + "</td>";
                tblHtml += "<td>" + val.CostPrice + " tk</td>";
                tblHtml += "<td>" + val.SalePrice + " tk</td>";
                tblHtml += '<td><button type="button" id="btnEdit" class="btn btn-primary"><i class="fa fa-edit"></i></button>';
                tblHtml += '<button type="button" class="btn btn-danger" id="btnDelete"><i class="fa fa-remove"></i></button></td></tr>';
            });

            $("#itemTableBody").html(tblHtml);
        });
    }
    $("#itemCancelButton").click(function () {
        $("#CategoryId").prop('selectedIndex', 0);
        $("#ItemName").val("");
        $("#ItemCode").val("");
        $("#ItemDescription").val("");
        $("#CostPrice").val("");
        $("#SalePrice").val("");
        alertify.error("All Cleared");
    });

    $("#CategoryId").change(function () {
        if ($("#CategoryId").val() === "") {
            $("#CategoryCode").val("");
            $("#ItemCode").val("");
        } else {
            var code = ($("#CategoryId option:selected").html()).slice(0, 3);
            var categoryId = $("#CategoryId").val();
            $("#CategoryCode").val(code.toLocaleUpperCase());
            $.post("/Setup/GetItemCode/",
                {
                    CategoryId: categoryId
                },
                function (data, status) {
                    if (status === "success") {
                        $("#ItemCode").val(data);
                    } else {
                        loadCategoryTable();
                        alertify.error("Data: " + data + "\nStatus: " + status);
                    }

                });

        }


    });

    $("#saveItemForm").validate({
        rules: {
            CategoryId: {
                required: true
            },
            ItemName: {
                required: true
            },
            ItemCode: {
                required: true
            },
            ItemDescription: {
                required: true
            },
            CostPrice: {
                required: true,
                number: true
            },
            SalePrice: {
                required: true,
                number: true
            }
        },
        messages: {
            CategoryId: {
                required: "Please select an option"
            },
            ItemName: {
                required: "Please enter item name"
            },
            ItemCode: {
                required: "Please enter item code"
            },
            ItemDescription: {
                required: "Please enter item description"
            },
            CostPrice: {
                required: "Please enter item purchase price",
                number: "Please enter a valid price"
            },
            SalePrice: {
                required: "Please enter a valid price",
                number: true
            }
        }
    });

    $("#itemSaveButton").click(function () {
        if ($("#saveItemForm").valid()) {
            var itemName = $("#ItemName").val();
            var itemCode = $("#ItemCode").val();
            var itemDescription = $("#ItemDescription").val();
            var costPrice = $("#CostPrice").val();
            var salePrice = $("#SalePrice").val();
            var categoryId = $("#CategoryId").val();

            $.post("/Setup/ItemSetup/",
                {
                    ItemName: itemName,
                    ItemCode: itemCode,
                    ItemDescription: itemDescription,
                    CostPrice: costPrice,
                    SalePrice: salePrice,
                    CategoryId: categoryId
                },
                function (data, status) {
                    if (status === "success") {
                        $("#CategoryId").prop('selectedIndex', 0);
                        $("#ItemName").val("");
                        $("#ItemCode").val("");
                        $("#ItemDescription").val("");
                        $("#CostPrice").val("");
                        $("#SalePrice").val("");
                        loadItemTable();
                        alertify.success("Data: " + data + "\nStatus: " + status);
                    } else {
                        loadItemTable();
                        alertify.error("Data: " + data + "\nStatus: " + status);
                    }

                });

        }
    });
    /*
     * Item Setup page code ends here
     */

    /*
     * Expense Setup page code starts here
     */
    function getExpenseCategory() {
        $.get("/Setup/GetParentExpenseCategories/", function (data) {
            var markup = "<option value=''>Select Category</option>";
            for (var x = 0; x < data.length; x++) {
                markup += "<option value=" + data[x].Value + ">" + data[x].Text + "</option>";
            }
            $("#RootExpenseCategoryId").html(markup).show();

        });
    }
    $("#ExpenseName").change(function () {
        var code = ($("#ExpenseName").val()).slice(0, 3);
        $("#ExpenseCode").val(code.toLocaleUpperCase());
    });
    $("#childExpenseCategory").change(function () {
        $("#RootExpenseCategoryId").removeAttr("disabled");
        $("#ExpenseName").val("");
        $("#ExpenseCode").val("");
        $("#ExpenseDescription").val("");
        $('#rootExpenseCategory').attr('checked', false);
        $('#childExpenseCategory').attr('checked', true);
        getExpenseCategory();
    });

    $("#rootExpenseCategory").change(function () {
        $("#RootExpenseCategoryId").find('option:not(:first)').remove();
        $("#ExpenseName").val("");
        $("#ExpenseCode").val("");
        $("#ExpenseDescription").val("");
        $('#rootExpenseCategory').attr('checked', true);
        $('#childExpenseCategory').attr('checked', false);
        $("#RootExpenseCategoryId").prop("disabled", "disabled");
    });

    $("#saveExpenseForm").validate({
        rules: {
            RootExpenseCategoryId: {
                required: true
            },
            ExpenseName: {
                required: true
            },
            ExpenseCode: {
                required: true
            },
            ExpenseDescription: {
                required: true
            }
        },
        messages: {
            RootExpenseCategoryId: {
                required: "Please select an option"
            },
            ExpenseName: {
                required: "Please enter expanse category name"
            },
            ExpenseCode: {
                required: "Please enter expense category code"
            },
            ExpenseDescription: {
                required: "Please enter expense category description"
            }
        }
    });


    $("#saveExpenseButton").click(function () {
        if ($("#saveExpenseForm").valid()) {
            var expenseName = $("#ExpenseName").val();
            var expenseCode = $("#ExpenseCode").val();
            var expenseDescription = $("#ExpenseDescription").val();
            var rootExpenseCategoryId = $("#RootExpenseCategoryId").val();

            if ($("#rootCategory").attr("checked") === "checked") {
                $.post("/Setup/SaveExpenseCategory/",
                    {
                        ExpenseName: expenseName,
                        ExpenseCode: expenseCode,
                        ExpenseDescription: expenseDescription
                    },
                    function (data, status) {
                        if (status === "success") {
                            $("#rootExpenseCategory").prop("checked", "checked");
                            $("#RootExpenseCategoryId").prop("disabled", "disabled");
                            $("#RootExpenseCategoryId").find('option:not(:first)').remove();
                            $("#ExpenseName").val("");
                            $("#ExpenseCode").val("");
                            $("#ExpenseDescription").val("");
                            $('#rootExpenseCategory').attr('checked', true);
                            $('#childExpenseCategory').attr('checked', false);
                            //loadCategoryTable();
                            alertify.success("Data: " + data + "\nStatus: " + status);
                        } else {
                            //loadCategoryTable();
                            alertify.error("Data: " + data + "\nStatus: " + status);
                        }

                    });
            }

            if ($("#childExpenseCategory").attr("checked") === "checked") {
                $.post("/Setup/SaveExpenseCategory/",
                    {
                        RootExpenseCategoryId: rootExpenseCategoryId,
                        ExpenseName: ExpenseName,
                        expenseCode: expenseCode,
                        expenseDescription: expenseDescription
                    },
                    function (data, status) {
                        if (status === "success") {
                            $("#rootExpenseCategory").prop("checked", "checked");
                            $("#RootExpenseCategoryId").prop("disabled", "disabled");
                            $("#RootExpenseCategoryId").find('option:not(:first)').remove();
                            $("#ExpenseName").val("");
                            $("#ExpenseCode").val("");
                            $("#ExpenseDescription").val("");
                            $('#rootExpenseCategory').attr('checked', true);
                            $('#childExpenseCategory').attr('checked', false);
                            //loadCategoryTable();
                            alertify.success("Data: " + data + "\nStatus: " + status);
                        } else {
                            //loadCategoryTable();
                            alertify.error("Data: " + data + "\nStatus: " + status);
                        }

                    });
            }

        }
    });
    /*
     * Expense Setup page code ends here
     */

    /*
     * Organization setup code starts here
     */
    function loadOrganizationTable() {
        $.get("/Setup/GetOrganizations/", function (data) {
            var tblHtml = "";
            $.each(data, function (i, val) {
                tblHtml += "<tr><td></td><td>" + val.OrganizationName + "</td>";
                tblHtml += "<td>" + val.OrganizationCode + "</td>";
                tblHtml += "<td>" + val.OrganizationContactNo + "</td>";
                tblHtml += "<td>" + val.OrganizationAddress + "</td>";
                tblHtml += '<td><button type="button" id="btnEdit" class="btn btn-primary btnEdit"><i class="fa fa-edit"></i></button>';
                tblHtml += '<button type="button" class="btn btn-danger btnDelete" id="btnDelete"><i class="fa fa-remove"></i></button></td></tr>';
            });

            $("#organizationTableBody").html(tblHtml);
        });
    }

    $("#OrganizationName").change(function () {
        var code = ($("#OrganizationName").val()).slice(0, 3);
        $("#OrganizationCode").val(code.toLocaleUpperCase());
    });

    $("#saveOrganizationForm").validate({
        rules: {
            OrganizationName: {
                required: true
            },
            OrganizationCode: {
                required: true
            },
            OrganizationContactNo: {
                required: true
            },
            OrganizationAddress: {
                required: true
            }
        },
        messages: {
            OrganizationName: {
                required: "Please enter organization name"
            },
            OrganizationCode: {
                required: "Please enter organization code"
            },
            OrganizationContactNo: {
                required: "Please enter organization contact no"
            },
            OrganizationAddress: {
                required: "Please enter organization address"
            }
        }
    });

    $("#saveOrganizationButton").click(function () {
        if ($("#saveOrganizationForm").valid()) {
            var organizationName = $("#OrganizationName").val();
            var organizationCode = $("#OrganizationCode").val();
            var organizationContactNo = $("#OrganizationContactNo").val();
            var organizationAddress = $("#OrganizationAddress").val();

            $.post("/Setup/SaveOrganization/",
                {
                    OrganizationName: organizationName,
                    OrganizationCode: organizationCode,
                    OrganizationContactNo: organizationContactNo,
                    OrganizationAddress: organizationAddress
                },
                function (data, status) {
                    if (status === "success") {
                        $("#OrganizationName").val("");
                        $("#OrganizationCode").val("");
                        $("#OrganizationContactNo").val("");
                        $("#OrganizationAddress").val("");
                        alertify.success("Data: " + data + "\nStatus: " + status);
                        loadOrganizationTable();
                    } else {
                        loadOrganizationTable();
                        alertify.error("Data: " + data + "\nStatus: " + status);
                    }

                });

        }
    });

    $("#cancelOrganizationButton").click(function () {
        $("#OrganizationName").val("");
        $("#OrganizationCode").val("");
        $("#OrganizationContactNo").val("");
        $("#OrganizationAddress").val("");
        alertify.error("All Cleared");
    });
    /*
     * Organization setup code ends here
     */


    /*
     * Outlet/ Branch setup code starts here
     */
    $("#OrganizationId").change(function () {
        if ($("#OrganizationId").val() === "") {
            $("#OrganizationCode").val("");
        } else {
            var organizationId = $("#OrganizationId").val();
            $.post("/Setup/GetOrganizationCode/",
                {
                    OrganizationId: organizationId
                },
                function (data, status) {
                    if (status === "success") {
                        $("#BranchCode").val(data);
                    } else {
                        loadCategoryTable();
                        alertify.error("Data: " + data + "\nStatus: " + status);
                    }

                });

        }


    });

    $("#saveBranchForm").validate({
        rules: {
            OrganizationId: {
                required: true
            },
            BranchName: {
                required: true
            },
            BranchCode: {
                required: true
            },
            BranchContactNo: {
                required: true
            },
            BranchAddress: {
                required: true
            }
        },
        messages: {
            OrganizationId: {
                required: "Please select an option"
            },
            BranchName: {
                required: "Please enter Branch name"
            },
            BranchCode: {
                required: "Please enter Branch code"
            },
            BranchContactNo: {
                required: "Please enter Branch contact"
            },
            BranchAddress: {
                required: "Please enter Branch address"
            }
        }
    });
    function loadBranchTable() {
        $.get("/Setup/Getbranches/", function (data) {
            var tblHtml = "";
            $.each(data, function (i, val) {
                tblHtml += "<tr><td></td><td>" + val.Organization.OrganizationName + "</td>";
                tblHtml += "<td>" + val.BranchName + "</td>";
                tblHtml += "<td>" + val.Organization.OrganizationCode + "-" + val.BranchCode + "</td>";
                tblHtml += "<td>" + val.BranchContactNo + "</td>";
                tblHtml += "<td>" + val.BranchAddress + "</td>";
                tblHtml += '<td><button type="button" id="btnEdit" class="btn btn-primary btnEdit"><i class="fa fa-edit"></i></button>';
                tblHtml += '<button type="button" class="btn btn-danger btnDelete" id="btnDelete"><i class="fa fa-remove"></i></button></td></tr>';
            });

            $("#branchTableBody").html(tblHtml);
        });
    }
    $("#saveBranchButton").click(function () {
        if ($("#saveBranchForm").valid()) {
            var organizationId = $("#OrganizationId").val();
            var branchName = $("#BranchName").val();
            var branchCode = $("#BranchCode").val();
            var branchContactNo = $("#BranchContactNo").val();
            var branchAddress = $("#BranchAddress").val();

            $.post("/Setup/SaveBranch/",
                {
                    OrganizationId: organizationId,
                    BranchName: branchName,
                    BranchCode: branchCode,
                    BranchContactNo: branchContactNo,
                    BranchAddress: branchAddress
                },
                function (data, status) {
                    if (status === "success") {
                        $("#OrganizationId").prop('selectedIndex', 0);
                        $("#BranchName").val("");
                        $("#BranchCode").val("");
                        $("#BranchContactNo").val("");
                        $("#BranchAddress").val("");
                        alertify.success("Data: " + data + "\nStatus: " + status);
                        loadBranchTable();
                    } else {
                        loadBranchTable();
                        alertify.error("Data: " + data + "\nStatus: " + status);
                    }

                });

        }
    });
    $("#cancelBranchButton").click(function () {
        $("#OrganizationId").prop('selectedIndex', 0);
        $("#BranchName").val("");
        $("#BranchCode").val("");
        $("#BranchContactNo").val("");
        $("#BranchAddress").val("");
        alertify.error("All Cleared");
    });
    /*
     * Outlet/ Branch setup code ends here
     */

    /*
     * Party setup code starts here
     */
    $("#savePartyForm").validate({
        rules: {
            PartyTypeId: {
                required: true
            },
            PartyName: {
                required: true
            },
            PartyCode: {
                required: true
            },
            PartyContactNo: {
                required: true
            },
            PartyEmail: {
                required: true
            },
            PartyAddress: {
                required: true
            }
        },
        messages: {
            PartyTypeId: {
                required: "Please select an option"
            },
            PartyName: {
                required: "Please enter party name"
            },
            PartyCode: {
                required: "Please enter party code"
            },
            PartyContactNo: {
                required: "Please enter party contact"
            },
            PartyEmail: {
                required: "Please enter party email address"
            },
            PartyAddress: {
                required: "Please enter party address"
            }
        }
    });
    function loadPartyTable() {
        $.get("/Setup/GetParties/", function (data) {
            var tblHtml = "";
            $.each(data, function (i, val) {
                tblHtml += "<tr><td></td><td>" + val.PartyType.Type + "</td>";
                tblHtml += "<td>" + val.PartyName + "</td>";
                tblHtml += "<td>" + val.PartyCode + "</td>";
                tblHtml += "<td>" + val.PartyContactNo + "</td>";
                tblHtml += "<td>" + val.PartyEmail + "</td>";
                tblHtml += "<td>" + val.PartyAddress + "</td>";
                tblHtml += '<td><button type="button" id="btnEdit" class="btn btn-primary btnEdit"><i class="fa fa-edit"></i></button>';
                tblHtml += '<button type="button" class="btn btn-danger btnDelete" id="btnDelete"><i class="fa fa-remove"></i></button></td></tr>';
            });

            $("#partyTableBody").html(tblHtml);
        });
    }
    $("#savePartyButton").click(function () {
        if ($("#savePartyForm").valid()) {
            var partyTypeId = $("#PartyTypeId").val();
            var partyName = $("#PartyName").val();
            var partyCode = $("#PartyCode").val();
            var partyContactNo = $("#PartyContactNo").val();
            var partyEmail = $("#PartyEmail").val();
            var partyAddress = $("#PartyAddress").val();

            $.post("/Setup/SaveParty/",
                {
                    PartyTypeId: partyTypeId,
                    PartyName: partyName,
                    PartyCode: partyCode,
                    PartyContactNo: partyContactNo,
                    PartyEmail: partyEmail,
                    PartyAddress: partyAddress
                },
                function (data, status) {
                    if (status === "success") {
                        $("#PartyTypeId").prop('selectedIndex', 0);
                        $("#PartyName").val("");
                        $("#PartyCode").val("");
                        $("#PartyContactNo").val("");
                        $("#PartyEmail").val("");
                        $("#PartyAddress").val("");
                        alertify.success("Data: " + data + "\nStatus: " + status);
                        loadPartyTable();
                    } else {
                        loadPartyTable();
                        alertify.error("Data: " + data + "\nStatus: " + status);
                    }

                });

        }
    });
    $("#cancelPartyButton").click(function () {
        $("#PartyTypeId").prop('selectedIndex', 0);
        $("#PartyName").val("");
        $("#PartyCode").val("");
        $("#PartyContactNo").val("");
        $("#PartyEmail").val("");
        $("#PartyAddress").val("");
        alertify.error("All Cleared");
    });


    $("#partyTable tbody").on("click", ".btnDelete", function (e) {
        e.preventDefault();
        var periodStart = $(this).closest('tr').children('td:eq(1)').text();
        alert(periodStart);
    });
    /*
     * Party setup code ends here
     */

    /*
     * Employee setup code starts here
     */
    $("#saveEmployeeForm").validate({
        rules: {
            BranchId: {
                required: true
            },
            EmployeeName: {
                required: true
            },
            EmployeeFatherName: {
                required: true
            },
            EmployeeMotherName: {
                required: true
            },
            EmployeeCode: {
                required: true
            },
            EmployeeJoinDate: {
                required: true
            },
            EmployeeContactNo: {
                required: true
            },
            EmployeeEmergencyContactNo: {
                required: true
            },
            EmployeeNId: {
                required: true
            },
            EmployeeUsername: {
                required: true
            },
            EmployeePassword: {
                required: true
            },
            EmployeeConfirmPassword: {
                required: true
            },
            EmployeeEmail: {
                required: true
            },
            EmployeePresentAddress: {
                required: true
            },
            EmployeePermanentAddress: {
                required: true
            }
        },
        messages: {
            BranchId: {
                required: "Please select an option"
            },
            EmployeeName: {
                required: "Please enter your name"
            },
            EmployeeFatherName: {
                required: "Please enter your father's name"
            },
            EmployeeMotherName: {
                required: "Please enter your Mother's name"
            },
            EmployeeCode: {
                required: "Please enter code"
            },
            EmployeeJoinDate: {
                required: "Please select a date"
            },
            EmployeeContactNo: {
                required: "Please enter your contact no."
            },
            EmployeeEmergencyContactNo: {
                required: "Please enter your emergency contact"
            },
            EmployeeNId: {
                required: "Please enter your national id"
            },
            EmployeeUsername: {
                required: "Please enter your username"
            },
            EmployeePassword: {
                required: "Please enter your password"
            },
            EmployeeConfirmPassword: {
                required: "Please enter your password again"
            },
            EmployeeEmail: {
                required: "Please enter your email address"
            },
            EmployeePresentAddress: {
                required: "Please enter your present address"
            },
            EmployeePermanentAddress: {
                required: "Please enter your Permanent address"
            }
        }
    });
    //$("#saveEmployeeForm").children("div").steps({
    //    headerTag: ".h",
    //    bodyTag: "tab-pane",
    //    transitionEffect: "slideLeft",
    //    onStepChanging: function (event, currentIndex, newIndex) {
    //        $("#saveEmployeeForm").validate().settings.ignore = ":disabled,:hidden";
    //        return $("#saveEmployeeForm").valid();
    //    },
    //    onFinishing: function (event, currentIndex) {
    //        $("#saveEmployeeForm").validate().settings.ignore = ":disabled";
    //        return $("#saveEmployeeForm").valid();
    //    },
    //    onFinished: function (event, currentIndex) {
    //        alert("Submitted!");
    //    }
    //});
    $("#EmployeeJoinDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: "yy-mm-dd"

    }).datepicker("setDate", "0");

    $("#nextButton").click(function () {
        if ($("#saveEmployeeForm").valid()) {
            $("#personal").addClass("active show");
            $("#personalTab").addClass("active show");
            $("#official").removeClass("active show");
            $("#officialTab").removeClass("active show");
            $("#nextButton").addClass("d-none");
            $("#saveEmployeeButton").removeClass("d-none");
            $("#previousButton").removeClass("d-none");

        }
    });

    $("#previousButton").click(function () {
        $("#personal").removeClass("active show");
        $("#personalTab").removeClass("active show");
        $("#official").addClass("active show");
        $("#officialTab").addClass("active show");
        $("#nextButton").removeClass("d-none");
        $("#saveEmployeeButton").addClass("d-none");
        $("#previousButton").addClass("d-none");
    });

    $("#saveEmployeeButton").click(function () {
        if ($("#saveEmployeeForm").valid()) {

            var branchId = $("#BranchId").val();
            var employeeName = $("#EmployeeName").val();
            var employeeFatherName = $("#EmployeeFatherName").val();
            var employeeMotherName = $("#EmployeeMotherName").val();
            var employeeCode = $("#EmployeeCode").val();
            var employeeJoinDate = $("#EmployeeJoinDate").val();
            var employeeContactNo = $("#EmployeeContactNo").val();
            var employeeEmergencyContactNo = $("#EmployeeEmergencyContactNo").val();
            var employeeNId = $("#EmployeeNId").val();
            var employeeUsername = $("#EmployeeUsername").val();
            var employeePassword = $("#EmployeePassword").val();
            var employeeEmail = $("#EmployeeEmail").val();
            var employeePresentAddress = $("#EmployeePresentAddress").val();
            var employeePermanentAddress = $("#EmployeePermanentAddress").val();

            $.post("/Setup/SaveEmployee/",
                {
                    BranchId: branchId,
                    EmployeeName: employeeName,
                    EmployeeFatherName: employeeFatherName,
                    EmployeeMotherName: employeeMotherName,
                    EmployeeCode: employeeCode,
                    EmployeeJoinDate: employeeJoinDate,
                    EmployeeContactNo: employeeContactNo,
                    EmployeeEmergencyContactNo: employeeEmergencyContactNo,
                    EmployeeNId: employeeNId,
                    EmployeeUsername: employeeUsername,
                    EmployeePassword: employeePassword,
                    EmployeeEmail: employeeEmail,
                    EmployeePresentAddress: employeePresentAddress,
                    EmployeePermanentAddress: employeePermanentAddress
                },
                function (data, status) {
                    if (status === "success") {
                        $("#BranchId").prop('selectedIndex', 0);
                        $("#EmployeeName").val("");
                        $("#EmployeeFatherName").val("");
                        $("#EmployeeMotherName").val("");
                        $("#EmployeeCode").val("");
                        $("#EmployeeJoinDate").val("");
                        $("#EmployeeContactNo").val("");
                        $("#EmployeeEmergencyContactNo").val("");
                        $("#EmployeeNId").val("");
                        $("#EmployeeUsername").val("");
                        $("#EmployeePassword").val("");
                        $("#EmployeeEmail").val("");
                        $("#EmployeePresentAddress").val("");
                        $("#EmployeePermanentAddress").val("");
                        alertify.success("Data: " + data + "\nStatus: " + status);
                    } else {
                        alertify.error("Data: " + data + "\nStatus: " + status);
                    }

                });

        }
    });
    $("#cancelEmployeeButton").click(function () {
        $("#BranchId").prop('selectedIndex', 0);
        $("#EmployeeName").val("");
        $("#EmployeeFatherName").val("");
        $("#EmployeeMotherName").val("");
        $("#EmployeeCode").val("");
        $("#EmployeeJoinDate").val("");
        $("#EmployeeContactNo").val("");
        $("#EmployeeEmergencyContactNo").val("");
        $("#EmployeeNId").val("");
        $("#EmployeeUsername").val("");
        $("#EmployeePassword").val("");
        $("#EmployeeEmail").val("");
        $("#EmployeePresentAddress").val("");
        $("#EmployeePermanentAddress").val("");
        alertify.error("All Cleared");
    });
    /*
     * Employee setup code ends here
     */

    /*
     * Purchase operation code starts here
     */
    $("#PurchaseDateTime").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: "yy-mm-dd"

    }).datepicker("setDate", "0");

    $("#BranchId").change(function () {
        if ($("#BranchId").val() === "") {
            $("#EmployeeId").find('option:not(:first)').remove();
        } else {
            var branchId = $("#BranchId").val();
            $.post("/Operation/GetEmployees/",
                {
                    branchId: branchId
                },
                function (data, status) {
                    if (status === "success") {
                        var markup = "<option value=''>Select Employee</option>";
                        $.each(data, function (i, val) {
                            markup += "<option value=" + val.EmployeeId + ">" + val.EmployeeName + "</option>";
                        });
                        //for (var x = 0; x < data.length; x++) {
                        //    markup += "<option value=" + data[x].Value + ">" + data[x].Text + "</option>";
                        //}
                        $("#EmployeeId").html(markup).show();
                    } else {
                        alertify.error("Data: " + data + "\nStatus: " + status);
                    }

                });
        }

    });

    $("#Item_ItemName").autocomplete({
        source: function (request, response) {
            $.post("/Operation/GetItems/",
                {
                    itemName: request.term
                },
                function (data, status) {
                    if (status === "success") {
                        response($.map(data, function (item) {
                            return item;
                        }));
                    } else {
                        alertify.error("Data: " + data + "\nStatus: " + status);
                    }

                });
        },
        select: function (e, i) {
            $("#Item_ItemId").val(i.item.id);
            $("#Item_CostPrice").val(i.item.cost);
        }
    });
    var listOfPurchaseItem = [];

    $("#addPurchaseItemForm").validate({
        rules: {
            ItemName: {
                required: true
            },
            Quantity: {
                required: true
            },
            CostPrice: {
                required: true
            }
        },
        messages: {
            ItemName: {
                required: "Please enter item name"
            },
            Quantity: {
                required: "Please enter item quantity"
            },
            CostPrice: {
                required: "Please enter item cost price"
            }
        }
    });
    $("#addPurchaseItemButton").click(function () {

        if ($("#addPurchaseItemForm").valid()) {
            var itemName = $("#Item_ItemName").val();
            var qty = $("#Item_Quantity").val();
            var price = $("#Item_CostPrice").val();
            var id = $('#Item_ItemId').val();

            var anItem = {};

            if (typeof listOfPurchaseItem !== 'undefined' && listOfPurchaseItem.length > 0) {
                // the array is defined and has at least one element
                var found = listOfPurchaseItem.some(function (el) {
                    if (el.ItemName === itemName) {
                        el.Quantity = parseInt(el.Quantity) + parseInt(qty);
                        return true;
                    }

                });
                if (!found) {
                    anItem = {
                        ItemName: itemName,
                        Quantity: qty,
                        Price: price,
                        ItemId: id
                    };
                    listOfPurchaseItem.push(anItem);
                }
            } else {
                anItem = {
                    ItemName: itemName,
                    Quantity: qty,
                    Price: price,
                    ItemId: id
                };

                listOfPurchaseItem.push(anItem);
            }



            var tblHtml = "";
            $.each(listOfPurchaseItem, function (i, val) {

                tblHtml += "<tr><td></td><td>" + val.ItemName + "</td>";
                tblHtml += "<td>" + val.Quantity + "</td>";
                tblHtml += "<td>" + val.Price + "</td>";
                tblHtml += "<td>" + val.Quantity * parseFloat(val.Price).toFixed(2) + "</td>";
                tblHtml += '<td><button type="button" id="btnEdit" class="btn btn-primary btnEdit"><i class="fa fa-edit"></i></button>';
                tblHtml += '<button type="button" class="btn btn-danger btnDelete" id="btnDelete"><i class="fa fa-remove"></i></button></td></tr>';
            });
            var sum = 0.0;
            $.each(listOfPurchaseItem, function (i, val) {
                sum += (val.Quantity * parseFloat(val.Price).toFixed(2));
            });
            $("#purchaseTableBody").html(tblHtml);
            $("#PurchaseTransaction_Total").val(sum);
            $("#Item_ItemName").val("");
            $("#Item_Quantity").val("1");
            $("#Item_CostPrice").val("");
        }
    });
    $('.purchaseEnter').keypress(function (e) {
        if (e.which == 13) {
            $('#addPurchaseItemButton').click();
        }
    });

    $("#savePurchaseForm").validate({
        rules: {
            BranchId: {
                required: true
            },
            PartyId: {
                required: true
            },
            EmployeeId: {
                required: true
            },
            PurchaseDateTime: {
                required: true
            },
            PurchaseTransaction_Total: {
                required: true
            },
            PurchaseTransaction_Paid: {
                required: true
            },
            PurchaseTransaction_Return: {
                required: true
            }
        },
        messages: {
            BranchId: {
                required: "Please select an option"
            },
            PartyId: {
                required: "Please select an option"
            },
            EmployeeId: {
                required: "Please select an option"
            },
            PurchaseDateTime: {
                required: "Please select an date"
            },
            PurchaseTransaction_Total: {
                required: "Please enter party name"
            },
            PurchaseTransaction_Paid: {
                required: "Please enter party name"
            },
            PurchaseTransaction_Return: {
                required: "Please enter party name"
            }
        }
    });
    function showPurchaseResult(supplierName, branchName, purchaseDate, employeeName) {
        $("#Supplier").val(supplierName);
        $("#Branch").val(branchName);
        $("#PurchaseDate").val(purchaseDate);
        $("#PurchasedBy").val(employeeName);
        $('#myModal').modal('show');

        var tblHtml = "";
        $.each(listOfPurchaseItem, function (i, val) {

            tblHtml += "<tr><td></td><td>" + val.ItemName + "</td>";
            tblHtml += "<td>" + val.Quantity + "</td>";
            tblHtml += "<td>" + val.Price + "</td>";
            tblHtml += "<td>" + val.Quantity * parseFloat(val.Price).toFixed(2) + "</td></tr>";
        });
        $("#purchaseResultTableBody").html(tblHtml);
    };
    $("#savePurchaseButton").click(function () {
        if ($("#savePurchaseForm").valid()) {
            var branchId = $("#BranchId").val();
            var employeeId = $("#EmployeeId").val();
            var partyId = $("#PartyId").val();
            var purchaseDate = $("#PurchaseDateTime").val();
            var total = $("#PurchaseTransaction_Total").val();
            var paidAmount = $("#PurchaseTransaction_Paid").val();
            var returnAmount = $("#PurchaseTransaction_Return").val();

            var branchName = $("#BranchId option:selected").html();
            var supplierName = $("#PartyId option:selected").html();
            var employeeName = $("#EmployeeId option:selected").html();

            $.post("/Operation/SaveStockIn/",
                {
                    BranchId: branchId,
                    EmployeeId: employeeId,
                    PartyId: partyId,
                    PurchaseDateTime: purchaseDate,
                    PurchaseTransaction: {
                        Total: total,
                        Paid: paidAmount,
                        Return: returnAmount
                    },
                    Items: listOfPurchaseItem
                },
                function (data, status) {
                    if (status === "success") {
                        showPurchaseResult(supplierName, branchName, purchaseDate, employeeName);
                        $("#BranchId").prop('selectedIndex', 0);
                        $("#EmployeeId").prop('selectedIndex', 0);
                        $("#PartyId").prop('selectedIndex', 0);
                        $("#PurchaseTransaction_Total").val("0");
                        $("#PurchaseTransaction_Paid").val("0");
                        $("#PurchaseTransaction_Return").val("0");
                        $("#purchaseTableBody").empty();
                        listOfPurchaseItem = [];

                        alertify.success("Data: " + data + "\nStatus: " + status);
                    } else {
                        alertify.error("Data: " + data + "\nStatus: " + status);
                    }

                });
        }
    });

    $('#PurchaseTransaction_Paid').keyup(function (e) {
        var total = $("#PurchaseTransaction_Total").val();
        var paidAmount = $("#PurchaseTransaction_Paid").val();
        var returnAmount = paidAmount - total;
        $('#PurchaseTransaction_Return').val(returnAmount);
    });

    $("#deletePurchasetemButton").click(function () {
        $("#Item_ItemName").val("");
        $("#Item_Quantity").val("1");
        $("#Item_CostPrice").val("");

    });
    $("#cancelPurchaseButton").click(function () {
        $("#BranchId").prop('selectedIndex', 0);
        $("#EmployeeId").prop('selectedIndex', 0);
        $("#PartyId").prop('selectedIndex', 0);
        $("#PurchaseTransaction_Total").val("0");
        $("#PurchaseTransaction_Paid").val("0");
        $("#PurchaseTransaction_Return").val("0");
        $("#purchaseTableBody").empty();
        listOfPurchaseItem = [];
    });

    $("#closePurchaseResult").click(function () {
        //$("#BranchId").prop('selectedIndex', 0);
        //$("#EmployeeId").prop('selectedIndex', 0);
        //$("#PartyId").prop('selectedIndex', 0);
        //$("#PurchaseTransaction_Total").val("0");
        //$("#PurchaseTransaction_Paid").val("0");
        //$("#PurchaseTransaction_Return").val("0");
        //$("#purchaseTableBody").empty();
        //listOfPurchaseItem = [];
        alertify.error("Print canceled");
    });
    $("#purchaseTable").on("click", ".btnDelete", function (e) {
        e.preventDefault();
        var selectedName = $(this).closest('tr').children('td:eq(1)').text();

        $.each(listOfPurchaseItem, function (i, val) {
            if (val.ItemName === selectedName) {
                var newTotal = $("#PurchaseTransaction_Total").val() - val.Price;
                $("#PurchaseTransaction_Total").val(newTotal);
                listOfPurchaseItem.splice($.inArray(selectedName, listOfPurchaseItem), 1);

            }
        });
        $(this).parents("tr").remove();
    });
    /*
     * Purchase operation code ends here
     */
    /*
     * Sale operation code starts here
     */
    $("#SaleDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: "yy-mm-dd"

    }).datepicker("setDate", "0");

    $(".itemName").autocomplete({
        source: function (request, response) {
            $.post("/Operation/GetItems/",
                {
                    itemName: request.term
                },
                function (data, status) {
                    if (status === "success") {
                        response($.map(data, function (item) {
                            return item;
                        }));
                    } else {
                        alertify.error("Data: " + data + "\nStatus: " + status);
                    }

                });
        },
        select: function (e, i) {
            $("#Item_ItemId").val(i.item.id);
            $("#StockQuantity").val(i.item.stockQuantity);
            $("#Item_SalePrice").val(i.item.sale);
        }
    });
    //$("#addSalesForm").validate({
    //    rules: {
    //        ItemName: {
    //            required: true
    //        },
    //        Quantity: {
    //            required: true
    //        },
    //    },
    //    messages: {
    //        ItemName: {
    //            required: "Please enter item name"
    //        },
    //        Quantity: {
    //            required: "Please enter item quantity"
    //        }
    //    },
    //    ignore: ".ignore"
    //});
    var listOfItem = [];

    $("#addItemButton").click(function () {
        if ($("#addSalesForm").valid()) {
            var itemName = $("#Item_ItemName").val();
            var qty = $("#Item_Quantity").val();
            var price = $("#Item_SalePrice").val();
            var id = $('#Item_ItemId').val();
            var anItem = {};
            if (typeof listOfItem !== 'undefined' && listOfItem.length > 0) {
                // the array is defined and has at least one element
                var found = listOfItem.some(function (el) {
                    if (el.ItemName === itemName) {
                        el.Quantity = parseInt(el.Quantity) + parseInt(qty);
                        return true;
                    }

                });
                if (!found) {
                    anItem = {
                        ItemName: itemName,
                        Quantity: qty,
                        Price: price,
                        ItemId: id
                    };
                    listOfItem.push(anItem);
                }
            } else {
                anItem = {
                    ItemName: itemName,
                    Quantity: qty,
                    Price: price,
                    ItemId: id
                };
                listOfItem.push(anItem);
            }
            //var anItem = {
            //    ItemName: itemName,
            //    Quantity: qty,
            //    Price: price
            //};
            //listOfItem.push(anItem);

            var tblHtml = "";
            $.each(listOfItem, function (i, val) {
                tblHtml += "<tr><td></td><td>" + val.ItemName + "</td>";
                tblHtml += "<td>" + val.Quantity + "</td>";
                tblHtml += "<td>" + val.Price + "</td>";
                tblHtml += "<td>" + val.Quantity * parseFloat(val.Price).toFixed(2) + "</td>";
                tblHtml += '<td><button type="button" id="btnEdit" class="btn btn-primary btnEdit"><i class="fa fa-edit"></i></button>';
                tblHtml += '<button type="button" class="btn btn-danger btnDelete" id="btnDelete"><i class="fa fa-remove"></i></button></td></tr>';
            });

            $("#salesTableBody").html(tblHtml);
            var sum = 0.00;
            $.each(listOfItem, function (i, val) {
                sum += (val.Quantity * parseFloat(val.Price).toFixed(2));
            });

            $("#total").text(sum);
            $("#SalesTransaction_SubTotal").val(sum);
            var subTotal = $("#SalesTransaction_SubTotal").val();
            var vat = $("#SalesTransaction_Vat").val();
            var discount = $("#SalesTransaction_Discount").val();
            var discountAmount = subTotal * (discount / 100);
            var vatAmount = (subTotal * (vat / 100));
            var total = (subTotal - discountAmount) + vatAmount;
            //alert("Vat/n" + "SubTotal: " + subTotal + "/n Vat: " + vat + "/n Discount: " + discount + "/n DiscountAmount: " +
            //    discountAmount + "/n VatAmount: " + vatAmount + "/n Total: " + total);
            $("#SalesTransaction_Total").val(total);
            $("#Item_ItemName").val("");
            $("#Item_Quantity").val("1");
            $("#Item_SalePrice").val("");
            $("#StockQuantity").val("");
        }
    });
    function showSalesResult(customerName, customerContact, branchName, saleDate, employeeName) {
        $("#Name").val(customerName);
        $("#Contact").val(customerContact);
        $("#Branch").val(branchName);
        $("#Date").val(saleDate);
        $("#SoldBy").val(employeeName);
        $('#myModal').modal('show');

        var tblHtml = "";
        $.each(listOfItem, function (i, val) {

            tblHtml += "<tr><td></td><td>" + val.ItemName + "</td>";
            tblHtml += "<td>" + val.Quantity + "</td>";
            tblHtml += "<td>" + val.Price + "</td></tr>";
        });
        $("#salesResultTableBody").html(tblHtml);
    };
    $("#saveSalesButton").click(function () {
        if ($("#saveSalesForm").valid()) {
            var branchId = $("#BranchId").val();
            var employeeId = $("#EmployeeId").val();
            var saleDate = $("#SaleDate").val();
            var subTotal = $("#SalesTransaction_SubTotal").val();
            var vat = $("#SalesTransaction_Vat").val();
            var discount = $("#SalesTransaction_Discount").val();
            var total = $("#SalesTransaction_Total").val();
            var paidAmount = $("#SalesTransaction_PaidAmount").val();
            var returnAmount = $("#SalesTransaction_ReturnAmount").val();

            var branchName = $("#BranchId option:selected").html();
            var employeeName = $("#EmployeeId option:selected").html();
            var customerName = $("#CustomerName").val();
            var customerContact = $("#CustomerContact").val();


            $.post("/Operation/SaveStockOut/",
                {
                    BranchId: branchId,
                    EmployeeId: employeeId,
                    SaleDate: saleDate,
                    SalesTransaction: {
                        SubTotal: subTotal,
                        Vat: vat,
                        Discount: discount,
                        Total: total,
                        PaidAmount: paidAmount,
                        ReturnAmount: returnAmount
                    },
                    Items: listOfItem
                },
                function (data, status) {
                    if (status === "success") {
                        showSalesResult(customerName, customerContact, branchName, saleDate, employeeName);
                        $("#BranchId").prop('selectedIndex', 0);
                        $("#EmployeeId").prop('selectedIndex', 0);
                        $("#SalesTransaction_SubTotal").val("0");
                        $("#SalesTransaction_Vat").val("0");
                        $("#SalesTransaction_Discount").val("0");
                        $("#SalesTransaction_Total").val("0");
                        $("#SalesTransaction_PaidAmount").val("0");
                        $("#SalesTransaction_ReturnAmount").val("0");
                        $("#CustomerName").val("");
                        $("#CustomerContact").val("");
                        $("#salesTableBody").empty();
                        $("#total").text("0");
                        listOfItem = [];

                        alertify.success("Data: " + data + "\nStatus: " + status);
                    } else {
                        alertify.error("Data: " + data + "\nStatus: " + status);
                    }

                });
        }
    });


    $('#Item_ItemName,#Item_Quantity').keypress(function (e) {
        if (e.which == 13) {
            $('#addItemButton').click();
        }
    });

    $('#SalesTransaction_Vat').keyup(function (e) {
        var subTotal = $("#SalesTransaction_SubTotal").val();
        var vat = $("#SalesTransaction_Vat").val();
        var discount = $("#SalesTransaction_Discount").val();
        var discountAmount = subTotal * (discount / 100);
        var vatAmount = (subTotal * (vat / 100));
        var total = (subTotal - discountAmount) + vatAmount;
        $("#SalesTransaction_Total").val(total);

    });

    $('#SalesTransaction_Discount').keyup(function (e) {
        var subTotal = $("#SalesTransaction_SubTotal").val();
        var vat = $("#SalesTransaction_Vat").val();
        var discount = $("#SalesTransaction_Discount").val();
        var discountAmount = subTotal * (discount / 100);
        var vatAmount = (subTotal * (vat / 100));
        var total = (subTotal - discountAmount) + vatAmount;
        $("#SalesTransaction_Total").val(total);
    });

    $('#SalesTransaction_PaidAmount').keyup(function (e) {
        var total = $("#SalesTransaction_Total").val();
        var paidAmount = $("#SalesTransaction_PaidAmount").val();
        var returnAmount = paidAmount - total;
        $('#SalesTransaction_ReturnAmount').val(returnAmount);
    });
    $("#salesTable").on("click", ".btnDelete", function (e) {
        e.preventDefault();
        var selectedName = $(this).closest('tr').children('td:eq(1)').text();

        $.each(listOfItem, function (i, val) {
            if (val.ItemName === selectedName) {
                listOfItem.splice($.inArray(selectedName, listOfItem), 1);
            }
        });
        var sum = 0.00;
        $.each(listOfItem, function (i, val) {
            sum += (val.Quantity * parseFloat(val.Price).toFixed(2));
        });
        $("#total").text(sum);
        $("#SalesTransaction_SubTotal").val(sum);
        var subTotal = $("#SalesTransaction_SubTotal").val();
        var vat = $("#SalesTransaction_Vat").val();
        var discount = $("#SalesTransaction_Discount").val();
        var discountAmount = subTotal * (discount / 100);
        var vatAmount = (subTotal * (vat / 100));
        var total = (subTotal - discountAmount) + vatAmount;
        $("#SalesTransaction_Total").val(total);
        $(this).parents("tr").remove();
    });

    $("#deleteItemButton").click(function () {
        $("#Item_ItemName").val("");
        $("#Item_Quantity").val("");
        $("#Item_SalePrice").val("");
        $("#StockQuantity").val("");
    });

    $("#resetSalesButton").click(function () {
        $("#BranchId").prop('selectedIndex', 0);
        $("#EmployeeId").prop('selectedIndex', 0);
        $("#SalesTransaction_SubTotal").val("0");
        $("#SalesTransaction_Vat").val("0");
        $("#SalesTransaction_Discount").val("0");
        $("#SalesTransaction_Total").val("0");
        $("#SalesTransaction_PaidAmount").val("0");
        $("#SalesTransaction_ReturnAmount").val("0");
        $("#CustomerName").val("");
        $("#CustomerContact").val("");
        $("#salesTableBody").empty();
        $("#total").text("0");
        listOfItem = [];
    });
    
    /*
     * Sale operation code ends here
     */
});