define(['jquery', 'kendo/kendo.all.min'], function () {

    //Variables
    var wnd, detailsTemplate;

    //Datasources
    var dataSourceUsers = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/api/lookups/users',
                type: 'GET',
                datatype: 'json'
            }
        }
    })

    var TaxesDataSource = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/api/TaxReturn/assigned',
                type: 'GET',
                dataType: 'json',
                cache: false
            },
            parameterMap: function (options, operation) {
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
                        //usuario: $('#cmbUsers option:selected').text(),
                        usuario: localStorage.getItem("user"),
                        role: localStorage.getItem("role"),
                        page: options.page,
                        pageSize: options.pageSize,
                        sortColumn: field,
                        sortDirection: dir,

                    }
                }

            }
        },
        requestStart: function () {
            //alert("Antes de leer");
        },
        schema: {
            model: {
                id: "id",
                fields: {
                    atlasId: { type: "string" },
                    description: { type: "string" },
                    client: { type: "string" },
                    dueDate: { type: "string" },
                    status: { type: "string" },
                    taxPayer: { type: "string" },
                    year: { type: "number" },
                    dueDate: { type: "string" },
                    status: { type: "string" },
                    assignedToId: { type: "number" },
                    userNameAssignedTo: { type: "string" },
                    createdById: { type: "number" },
                    userNameCreatedBy: { type: "string" }
                }
            },
            data: function (data) {
                if (data.totalCount == 0) {
                    alert("User doesn't have tasks assigned");
                }
                return data.results
            },
            total: function (data) {
                return data.totalCount
            }
        },
        pageSize: 25,
        total: 0,
        serverPaging: true,
        serverSorting: true,
        sort: [
            { field: "description", dir: "asc" }
        ],
        Batch: false
    });

    //Models
    var modelTaxesSenior = kendo.observable({
        taxes: TaxesDataSource,
        AssignTaxSenior: function (e) {
            var template = kendo.template($("#template-assing").html());
            $("#assingSenior").html(template);
            wnd = $("#assingSenior").kendoWindow({
                title: "Assign To",
                modal: true,
                visible: false,
                resizable: false,
                width: "450px"
            }).data("kendoWindow")
            wnd.content(template);
            wnd.center().open();

            //$.extend(modelAssignTaxRay, e.data);
            modelAssignTaxRay.set("assignedToId", e.data.userNameAssignedTo);
            modelAssignTaxRay.set("description", e.data.description);

            kendo.bind($("#assingSenior"), modelAssignTaxRay);
        },
    });


    var modelTaxesJunior = kendo.observable({
        taxes: TaxesDataSource
    });
    var modelAssignTaxRay = kendo.observable({
        users: dataSourceUsers,
        assignedToId: 0,
        description: "",
        error: "",
        isVisible: true,
        update: function (e) {
            $.ajax({
                type: 'PUT',
                url: '/api/TaxReturn/Assign?taxId=' + e.data.id + '&currentId=' + e.data.createdById + '&assignUserId=' + e.data.assignedToId,
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (result) {
                    wnd.center().close();
                    $('#grid').data('kendoGrid').dataSource.read();
                },
                error: function (result) {
                    alert("Error");
                }
            });
        },
        onOpen: function () {
            //alert("Abriendo el combo");
            //alert(this.get("assignedToId"));
            //var combo = $("#comboUsers").data("kendoComboBox").value(1);
            //combo.select(this.get("assignedToId"));

        },
        onChange: function (e) {
            var combobox = $("#comboUsers").data("kendoComboBox");
            var dataItem = combobox.dataItem();
            var text = combobox.value();
            if (text != "" && dataItem == undefined) {
                this.set("error", "User does not exist");
                this.set("isVisible", false);
                //var combobox = $("#comboUsers").data("kendoComboBox");
                //combobox.select(3);
            }
            else {
                this.set("error", "");
                this.set("isVisible", true);
            }
        },
        cancel: function (e) {
            this.set("isVisible", true);
            this.set("error", "");
            //this.set("assignedToId", e.data.assignedToId);
            //var combobox = $("#comboUsers").data("kendoComboBox");
            //combobox.select(this.get("assignedToId"));
            wnd.center().close();
            
        }
    });

    var modelEditTaxRay = kendo.observable({
        isVisible: true,
        update: function (e) {
            var fecha;
            if (kendo.parseDate(e.data.dueDate, "dd.MM.yyyy")) {
                fecha = kendo.parseDate(e.data.dueDate, "dd.MM.yyyy");
            }
            else {
                fecha = e.data.dueDate;
            }
            /*tax = { 
                Id: e.data.id,
                AtlasId: e.data.atlasId,
                Description: e.data.description,
                Client: e.data.client,
                TaxPayer: e.data.taxPayer,
                DueDate: new Date(fecha).toISOString(),
                Status: e.data.status,
                Year: e.data.year,
                AssignedToId: e.data.assignedToId,
                AssignedTo: { id: e.data.assignedToId, userName: e.data.userNameAssignedTo },
                CreatedById: e.data.createdById,
                CreatedBy: { id: e.data.createdById, userName: e.data.userNameCreatedBy }
            };*/
            $.ajax({
                type: 'PUT',
                url: '/api/TaxReturn/UpdateTax?taxId=' + e.data.id + '&description=' + e.data.description + '&client=' + e.data.client + '&taxPayer=' + e.data.taxPayer + '&year=' + e.data.year + '&dueDate=' + new Date(fecha).toISOString() + '&status=' + e.data.status,
                //data: tax,
                /*data:{
                    'taxId': + e.data.id, 
                    'description': "'" + e.data.description + "'", 
                    'client': "'" + e.data.client + "'", 
                    'taxPayer':e.data.taxPayer, 
                    'year': e.data.year,
                    'dueDate':new Date(fecha).toISOString(), 
                    'status':e.data.status},*/
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (result) {
                    wnd.center().close();
                    $('#grid').data('kendoGrid').dataSource.read();
                },
                error: function (result) {
                    alert("Error");
                }
            });
        },
        cancel: function (e) {
            this.set("isVisible", true);
            $("#edit").empty();
            wnd.center().close();
        },
        onchange: function (e) {
            var validator = $("#formEdit").kendoValidator().data("kendoValidator");
            if (validator.validate()) {
                this.set("isVisible", true);
            }
            else {
                this.set("isVisible", false);
            }
        },
        onchangeDate: function (e) {
            var validatorDate = $("#dueDate").kendoValidator({
                rules: {
                    required: function (input) {
                        if (input.is("[name=dueDate]")) {
                            return $.trim(input.val()) != "";
                        }
                    },
                    date: function (input) {
                        if (input.is("[name=dueDate]")) {
                            if (kendo.parseDate(input.val(), "dd.MM.yyyy")) {
                                return true;
                            }
                            else {
                                return false;
                            }
                        }
                        else {
                            return true;
                        }
                    },
                },
                messages: {
                    required: "due date is required",
                    date: "Format incorrect"
                }
            }).data("kendoValidator");

            if (validatorDate.validate()) {
                this.set("isVisible", true);
            }
            else {
                this.set("isVisible", false);
            }
        }
    });

    var modelHome = kendo.observable({
        logarse: function () {
            //var userSelected = $('#cmbUsers option:selected').text();
            var userSelected = $('#cmbUsers option:selected').text();
            var role = $('#cmbUsers option:selected').attr("permisos");
            localStorage.setItem("user", userSelected);
            localStorage.setItem("role", role);
            layoutLogin.hide();
            layoutView.showIn("#content", viewHome);
            layoutView.render("#app");
            //router.navigate("/Home");
        }
    })


    var modelTaxes = kendo.observable({
        taxes: TaxesDataSource,
        onDataBound: function (e) {
            //var grid = $("#grid");
            var grid = $("#grid").data('kendoGrid');
            grid.tbody.find("tr").each(function () {
                model = grid.dataItem(this);
                if (model.status == "New") {
                    $(this).find(".k-grid-delete").remove();
                }
                //$(this).find(".k-grid-delete").remove();
            })
        },
        AssignTax: function (e) {
            var template = kendo.template($("#template-assing").html());
            $("#assing").html(template);
            wnd = $("#assing").kendoWindow({
                title: "Assign To",
                modal: true,
                visible: false,
                resizable: false,
                width: "450px"
            }).data("kendoWindow")
            wnd.content(template);
            wnd.center().open();
            //$.extend(modelAssignTaxRay, e.data);
            modelAssignTaxRay.set("assignedToId", e.data.userNameAssignedTo);
            modelAssignTaxRay.set("description", e.data.description);
            kendo.bind($("#assing"), modelAssignTaxRay);
        },
        deleteTax: function (e) {
            var r = confirm("Are you sure you want to delete this record?");
            if (r == true) {
                $.ajax({
                    type: 'DELETE',
                    url: '/api/TaxReturn/' + e.data.id,
                    dataType: 'json',
                    complete: function (jqXhr, textStatus) {
                        if (jqXhr.status == "200") {
                            $('#grid').data('kendoGrid').dataSource.read();
                        }
                        else {
                            alert("Error");
                        }
                    }
                });
            }
        },
        EditTax: function (e) {
            var template = kendo.template($("#template-edit").html());
            $("#edit").html(template);
            wnd = $("#edit").kendoWindow({
                title: "Edit",
                modal: true,
                visible: false,
                resizable: false,
                width: "450px"
            }).data("kendoWindow")
            wnd.content(template);
            wnd.center().open();
            $.extend(modelEditTaxRay, e.data);
            kendo.bind($("#edit"), modelEditTaxRay);
        }
    });

    //Views
    var layoutLogin = new kendo.Layout("layout-login",{ model: modelHome });
    var layoutView = new kendo.Layout("layout-template");
    var viewHome = new kendo.View("template-home" );
    var viewTaxes = new kendo.View("template-taxes");
    var viewTravelTracker = new kendo.View("template-travelTracker");
    var viewInmigration = new kendo.View("template-inmigration");
    var viewDocuments = new kendo.View("template-documents");
    var viewTaxRay = new kendo.View("template-taxRay", { model: modelTaxes })
    var viewTaxRaySenior = new kendo.View("template-taxRay-Senior", { model: modelTaxesSenior })
    var viewTaxRayJunior = new kendo.View("template-taxRay-Junior", { model: modelTaxesJunior })
    var viewTaxRayOtros = new kendo.View("template-otros");


    //Routes
    var router = new kendo.Router({
        init: function () {
            layoutLogin.render("#app");
            //layoutView.showIn("#content", viewHome);
            //layoutView.render("#app");
        }
    });


    router.route("/Home", function () {
        $(".optionSelected").removeClass("optionSelected").addClass("option");
        $("#bladeHome").removeClass("option").addClass("optionSelected");
        layoutView.showIn("#content", viewHome);
    });

    router.route("/Taxes", function () {
        $(".optionSelected").removeClass("optionSelected").addClass("option");
        $("#bladeTaxes").removeClass("option").addClass("optionSelected");
        layoutView.showIn("#content", viewTaxes);
    });

    router.route("/TravelTracker", function () {
        $(".optionSelected").removeClass("optionSelected").addClass("option");
        $("#bladeTravelTracker").removeClass("option").addClass("optionSelected");
        layoutView.showIn("#content", viewTravelTracker);
    });

    router.route("/Inmigration", function () {
        $(".optionSelected").removeClass("optionSelected").addClass("option");
        $("#bladeInmigration").removeClass("option").addClass("optionSelected");

        layoutView.showIn("#content", viewInmigration);
    });

    router.route("/Documents", function () {
        $(".optionSelected").removeClass("optionSelected").addClass("option");
        $("#bladeDocuments").removeClass("option").addClass("optionSelected");
        layoutView.showIn("#content", viewDocuments);
    });

    router.route("/TaxRay", function () {
        $(".optionSelected").removeClass("optionSelected").addClass("option");
        $("#bladeTaxRay").removeClass("option").addClass("optionSelected");

        /*var userSelected = $('#cmbUsers option:selected').text();
        var role = $('#cmbUsers option:selected').attr("permisos");
        localStorage.setItem("user", userSelected);
        localStorage.setItem("role", role);*/

        var usuario= localStorage.getItem("user");
        var role = localStorage.getItem("role");

        if (role == "Junior") {
            layoutView.showIn("#content", viewTaxRayJunior);
        }
        if (role == "Senior") {
            layoutView.showIn("#content", viewTaxRaySenior);
        }
        if (role == "Super Admin") {
            layoutView.showIn("#content", viewTaxRay);
        }
        if (role == "Otros") {
            layoutView.showIn("#content", viewTaxRayOtros);
        }

    });

    //Onload
    $(function () {
        router.start();
    })
});
