(function () {
    var accessToken = "";
    var getHeaders= function () {
        if (accessToken) {
            return { "Authorization": "Bearer " + accessToken };
        }
    };
    var dataSourceUsers = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/api/lookups/users',
                type: 'GET',
                headers: getHeaders(),
                datatype: 'json'
            }
        }
    });
 
    var saveAccessToken = function (data) {
        accessToken = data.access_token;
    };

    var showResponse = function (object) {

        $("#output").text(JSON.stringify(object, null, 4));

    };

    var login = function () {
        var url = "/Token";
        var data = $("#userData").serialize();
        data = data + "&grant_type=password";
        $.post(url, data)
         .success(saveAccessToken)
         .always(showResponse);

        return false;
    };

    $("#login").click(login);

    var NewDataSource = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/api/TaxReturn/assigned',
                type: 'GET',
                datatype: 'json'
            },
            destroy: {
                url: function (data) {
                    return "/api/TaxReturn/" + data.id + "";
                },
                type: "DELETE",
                dataType: "json"
            },
            update: {
                url: '/api/TaxReturn',
                type: 'PUT',
                datatype: 'json',
            },

            parameterMap: function (options, operation) {
                if (operation == "update") {
                    var year, month, day;
                    /*year = options.dueDate.substring(6);
                    month = options.dueDate.substring(3, 5) - 1;
                    day = options.dueDate.substring(0, 2);*/
                  
                    var tax = {
                        Id: options.id,
                        AtlasId: options.atlasId,
                        Description: options.description,
                        Client: options.client,
                        TaxPayer: options.taxPayer,
                        DueDate: new Date(options.dueDate).toISOString(),
                        //DueDate: kendo.parseDate(options.dueDate, "dd/MM/yyyy"),
                        Status: options.status,
                        Year: options.year,
                        AssignedToId: options.assignedToId,
                        AssignedTo: { id: options.assignedToId, userName: options.userNameAssignedTo },
                        CreatedById: options.createdById,
                        CreatedBy: { id: options.createdById, userName: options.userNameCreatedBy }
                    }
                    return tax;
                }
                if (operation == "read") {
                    var field, dir;
                    if (options.sort.length == 0) {
                        field = "description";
                        dir = "asc";
                    }
                    else {
                        field = options.sort[0].field;
                        dir = options.sort[0].dir;
                    }
                    return {
                        page: options.page,
                        pageSize: options.pageSize,
                        sortColumn: field,
                        sortDirection: dir
                    }
                }
            }
        },
        pageSize: 25,
        total: 0,
        serverPaging: true,
        serverSorting: true,
        sort: [
            { field: "description", dir: "asc" }
        ],
        schema: {
            data: function (data) {
                if (data.totalCount == 0)
                {
                    alert("User doesn't have tasks assigned");
                }
                return data.results
            },
            total: function (data) {
                return data.totalCount
                
            },
            model: {
                id: "id",
                fields: {
                    id: { type: "number" },
                    atlasId: { type: "string", editable: false},
                    description: {type: "string",validation: { required: true }},
                    client: {type: "string",validation: { required: true }},
                    dueDate: { type: "string", validation: { required: true } },
                    status: { type: "string", validation: { required: true } },
                    taxPayer: { type: "string", validation: { required: true } },
                    year: { type: "string", validation: { required: true } },
                    userNameAssignedTo: { type: "string",editable: false },
                    userNameCreatedBy: { type: "string", editable: false },
                    createdById: { type: "number" },
                    assignedToId : {type:"number"},
                }
            }
        },
    });

    var wnd, detailsTemplate;

    wnd = $("#details").kendoWindow({
        title: "Assign To",
        modal: true,
        visible: false,
        resizable: false,
        width: "450px",
    }).data("kendoWindow");

    detailsTemplate = kendo.template($("#template").html());

    $("#gridTax").kendoGrid({
        sortable: true,
        dataSource: NewDataSource,
        dataBinding: function (e) {
        },
        dataBound: function (e) {
            var role = "SuperAdmin";
            var grid = this;
            var model;
            
            if (role == "junior") {
                grid.hideColumn(9);
            }
            else {
                grid.tbody.find("tr[role='row']").each(function () {
                    model = grid.dataItem(this);
                    if (role == "senior") {
                        $(this).find(".k-grid-edit").remove();
                        $(this).find(".k-grid-delete").remove();
                    }
                    if (role == "SuperAdmin") {
                        if (model.status == "New") {
                            $(this).find(".k-grid-delete").remove();
                        }
                    }
                })
            }
        },
        columns: [
            { field: "atlasId", title: "Atlas ID"},
            { field: "description", title: "Description", width: "300px" },
            { field: "client", title: "Client"},
            { field: "taxPayer", title: "Tax Payer" },
            {
                field: "year",
                title: "Year"
            },
            {
                field: "dueDate",
                title: "Due Date",
                format: "{0:dd.MM.yyyy}",
                parseFormats: ["dd.MM.yyyy"],
                editor: function (container, options) {
                    $('<input  required="required" validationMessage="dueDate is required" data-text-field="' + options.field + '" data-value-field="' + options.field + '" data-bind="value:' + options.field + '" data-format="' + options.format + '"/>')
                    .appendTo(container)
                    .kendoDatePicker({
                        change: function (e) {
                            if(this.value()== null)
                            {
                                this.value("");
                            }
                        }
                    });
                }
            },
            { field: "status", title: "Status" },
            { field: "userNameAssignedTo", title: "Assigned To" },
            { field: "userNameCreatedBy", title: "Created By" },
            {
                command: [
                  { text: "Assign To", click: showAssingedTo },
                  { name: "edit", text: "Edit" },
                  { name: "destroy", text: "Delete" }
                ],
                title: "&nbsp;",
                width:"250px"
            }
        ],
        editable: {
            mode: "popup",
            confirmation: true,
            //template: kendo.template($("#template-edit").html())
            
        },
        pageable: {
            input: false,
            refresh: true,
            pageSizes: [25, 50, 100],
            buttonCount: 5,
            numeric: true
        }
    });

    function showAssingedTo(e) {
        e.preventDefault();
        var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
        wnd.content(detailsTemplate(dataItem));
        $("#cmbUserAssignedTo").kendoComboBox({
            dataTextField: "username",
            dataValueField:"id",
            dataSource:dataSourceUsers,
            dataBound: function (e) {
                var user = $("#txtAssignedTo").val();
                this.text(user);
            }
        });

        wnd.center().open();


        $("#btn-update").click(function (e) {
            var assignUserId = $("#cmbUserAssignedTo option:selected").val() == undefined ? null : $("#cmbUserAssignedTo option:selected").val();
            var taxId = $("#taxId").val();
            var currentId = $("#txtUserCreatedById").val();
            $.ajax({
                type: 'PUT',
                url: '/api/TaxReturn/Assign?taxId=' + taxId + '&currentId=' + currentId + '&assignUserId=' + assignUserId,
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (result) {
                    wnd.center().close();
                },
                error: function (result) {
                    alert("Error");
                }
            });
        });

        $("#btn-cancel").click(function () {
            wnd.center().close();
        })

    }
})();
