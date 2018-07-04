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
                tblHtml += "<td>" + val.ItemCode + "</td>";
                tblHtml += "<td>" + val.ItemDescription + "</td>";
                tblHtml += "<td>" + val.CostPrice + " tk</td>";
                tblHtml += "<td>" + val.SalePrice + " tk</td>";
                tblHtml += '<td><button type="button" id="btnEdit" class="btn btn-primary btnEdit"><i class="fa fa-edit"></i></button>';
                tblHtml += '<button type="button" class="btn btn-danger btnDelete" id="btnDelete"><i class="fa fa-remove"></i></button></td></tr>';
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
        var code = ($("#CategoryId option:selected").html()).slice(0, 3);
        $("#ItemCode").val(code.toLocaleUpperCase());
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
                tblHtml += "<td>" + val.BranchCode + "</td>";
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
    /*
     * Party setup code ends here
     */

    /*
     * Organization setup code starts here
     */

    /*
     * Organization setup code ends here
     */
});