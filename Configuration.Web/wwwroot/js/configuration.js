var configuration = {
    entityNames: [],

    initList: function () {
        $(".config-entity").each(function (index, item) {
            debugger;
            configuration.entityNames.push({ id: $(item).data("id"), name: $(item).data("name") });
        })

        $(".search-input").on('keyup', function (e) {
            if (e.key === 'Enter' || e.keyCode === 13) {
                debugger;
                var inputVal = $(this).val();
                $.each(configuration.entityNames, function (index, val) {
                    $("#configRow_" + val.id).show();
                });

                if (inputVal != "") {
                    var notValidResults = configuration.entityNames.filter(o => o.name.indexOf(inputVal) === -1);
                    $.each(notValidResults, function (index, val) {
                        debugger;
                        $("#configRow_" + val.id).hide();
                    });
                }
            }
        });
    },

    addOrUpdateConfig: function (isEdit) {
        var model = configuration.getConfigData(isEdit);

        request.post("/Configuration/AddOrUpdateConfiguration", model, function (response) {
            debugger;
            if (response) {
                console.log("success");
                location.href = "/Configuration/List";
            } else {
                alert("Ýþlem Baþarýsýz. Lütfen girmiþ olduðunuz verileri kontrol edip tekrar deneyiniz.")
            }
        })
    },

    getConfigData: function (isEdit) {
        debugger;
        var data = {};

        if (isEdit) {
            data = {
                Id: $("#frmEditConfig #ConfigurationId").val(),
                Name: $("#frmEditConfig #EditName").val(),
                Type: $("#frmEditConfig #EditType").val(),
                Value: $("#frmEditConfig #EditValue").val()
            }
        } else {
            data = {
                Name: $("#frmAddNewConfig #Name").val(),
                Type: $("#frmAddNewConfig #Type").val(),
                Value: $("#frmAddNewConfig #Value").val()
            }
        }

        return data;
    },

    editModal: function (elem, id) {
        debugger;
        var name = $("#editConfig_" + id).data("name");
        var type = $("#editConfig_" + id).data("type");
        var value = $("#editConfig_" + id).data("value");

        $("#editConfigurationModal #ConfigurationId").val(id);
        $("#editConfigurationModal #EditName").val(name);
        $("#editConfigurationModal #EditType").val(type);
        $("#editConfigurationModal #EditValue").val(value);
    },

    deleteModal: function (isBulk, itemId) {
        var valueList = [];
        if (isBulk) {
            $(".select-custom-checkbox:checked").each(function (index, elem) {
                valueList.push($(elem).val());
            })
        } else {
            valueList = [itemId];
        }

        $("#frmDeleteConfig #DeleteConfigurationId").val(valueList);
    },

    deleteConfig: function () {
        debugger;
        var configIdList = $("#frmDeleteConfig #DeleteConfigurationId").val().split(",");

        request.post("/Configuration/DeleteConfiguration", {configIdList: configIdList}, function (response) {
            debugger;
            if (response) {
                console.log("success");
                location.href = "/Configuration/List";
            } else {
                alert("Ýþlem Baþarýsýz. Lütfen girmiþ olduðunuz verileri kontrol edip tekrar deneyiniz.")
            }
        })
    }
}