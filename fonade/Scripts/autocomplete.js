
$(function () {

    $("[id*='txtCustomer']").autocomplete({

        source: function (request, response) {

            var params = new Object();
            params.companyName = request.term;

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Default.aspx/GetCustomerList",
                data: JSON.stringify(params),
                dataType: "json",
                async: true,
                success: function (data) {

                    response($.map(data.d, function (item) {
                        return {
                            label: item
                        }
                    }))
                },
                error: function (request, status, error) {
                    alert(jQuery.parseJSON(request.responseText).Message);
                }
            });

        },

        open: function (event, ui) {

            $(this).autocomplete("widget").css({
                "width": 200, "font-size": 12
            });

        },

        select: function (event, ui) {

            $("[id*='txtCustomer']").val(ui.item.label);

        }


    });
});