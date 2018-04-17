// Copy the folloing Javascript files from the Relativity Integration Points SDK into the script folder: 
// frame-messaging.js, jquery-1.8.2.js, jquery-postMessage.js
$(function () {
    //Create a new communication object that talks to the host page.
    var message = IP.frameMessaging();

    var getModel = function () {
        var model = {
            exampleViewField1: $('#exampleViewField1').val(),
            exampleViewField2: $('#exampleViewField2').val(),
            exampleViewField3: $('#exampleViewField3').val(),
            exampleViewField4: $('#exampleViewField4').val(),
            exampleViewField5: $('#exampleViewField5').val()
        }
        return model;
    };

    //An event raised when the user has clicked the Next or Save button.
    message.subscribe('submit', function () {
        //Execute save logic that persists the state

        var serializedModel = JSON.stringify(getModel());
        this.publish("saveState", serializedModel);
        //Communicate to the host page that it to continue.
        this.publish('saveComplete', serializedModel);
    });

    //An event raised when a user clicks the Back button.
    message.subscribe('back', function () {
        //Execute save logic that persists the state.
        var serializedModel = JSON.stringify(getModel());
        this.publish('saveState', serializedModel);
    });

    //An event raised when the host page has loaded the current settings page.
    message.subscribe('load', function (model) {
        $('#exampleViewField1').val(model.exampleViewField1);
        $('#exampleViewField2').val(model.exampleViewField2);
        $('#exampleViewField3').val(model.exampleViewField3);
        $('#exampleViewField4').val(model.exampleViewField4);
        $('#exampleViewField5').val(model.exampleViewField5);
    });
});