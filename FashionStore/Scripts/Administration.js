Administration = {
    UserManagement: {
        ShowAddEdit: function (url, div, title) {
            $.ajax({
                url: url,
                type: 'Get',
                success: function (data) {
                    $(div).html(data);
                    $("#modelHeader").text(title);
                    $(div).dialog({
                        modal: true,
                        draggable: false,
                        resizable: false,
                        position: 'absolute',
                        show: 'blind',
                        hide: 'blind',
                        width: 600,
                        title: title
                    });
                    $('body').addClass('modal-open');
                    $.validator.unobtrusive.parse("form");
                },
                error: function (e) {
                    Common.AjaxErrorHandler(e);
                }
            });
        }
    }
};
Common = {
    Show: function (url, div, title, width) {
        $("#spinner").show();
            $.ajax({
                url: url,
                type: 'Get',
                success: function (data) {
                    $(div).html(data);
                    $("#modelHeader").text(title);
                    $(div).dialog({
                        modal: true,
                        draggable: false,
                        resizable: false,
                        position: 'absolute',
                        show: 'blind',
                        hide: 'blind',
                        width: width,
                        title: title
                    });
                    $("#spinner").hide();
                },
                error: function (e) {
                    AjaxErrorHandler(e);
                    $("#spinner").hide();
                }
            });
        }, CheckCart: function () {
            var count = $("#paymentCartCount").text();
            if(count != "" && parseFloat(count))
            {
                var response = confirm("Are you sure you want to clear the Cart!");
                if(response)
                {
                    $.ajax({
                        url: document.ClearCartUrl,
                        type: 'Get',
                        success: function (data) {
                            return true;
                        },
                        error: function (e) {
                            AjaxErrorHandler(e);
                            return false;
                        }
                    });
                } else {
                    return false;
                }
            }
            return true;
        },
        OpenOrder: function (url, div, title, width) {
            var response = Common.CheckCart();
            if(response)
            {
                $.ajax({
                    url: url,
                    type: 'Get',
                    success: function (data) {
                        $(div).html(data);
                        $("#modelHeader").text(title);
                        $(div).dialog({
                            modal: true,
                            draggable: false,
                            resizable: false,
                            position: 'absolute',
                            show: 'blind',
                            hide: 'blind',
                            width: width,
                            title: title
                        });
                    },
                    error: function (e) {
                        AjaxErrorHandler(e);
                    }
                });
            }
            return false;
        },
        ShowPaymentSummary: function () {
            $("#spinner").show();
            $.ajax({
                url: document.paymentSummary,
                type: 'Get',
                success: function (data) {
                    $("#divSearch").html(data);
                    $("#modelHeader").text("Payment Summary");
                    $("#divSearch").dialog({
                        modal: true,
                        draggable: false,
                        resizable: false,
                        position: 'absolute',
                        show: 'blind',
                        hide: 'blind',
                    });
                    $("#spinner").hide();
                },
                error: function (e) {
                    AjaxErrorHandler(e);
                }
            });
            return false;
        },
        AjaxErrorHandler: function (e) {
            debugger;
            if (e.status == 200 && e.readyState == 4 && e.responseText != "") {
                alert("Your session is expired. Please login again.");
                var url = document.AjaxErrorUrl.replace("-1", e.statusText);
                window.location.href = url;
                return;
            }
            if (e.status === 0) {
                alert("Connection is lost.\nPlease verify your network connection.");
            } else if (e.status == 404) {
                alert("The requested page not found. [404]");
            } else if (e.status == 500) {
                alert("Internal Server Error [500].");
            } else {
                alert("Uncaught Error.\n" + e.responseText);
            }
            //else {
            //    alert("something seems wrong");
            //}            
        }

};