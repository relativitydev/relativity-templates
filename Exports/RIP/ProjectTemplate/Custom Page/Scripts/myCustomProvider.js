// Copy the folloing Javascript files from the Relativity Integration Points SDK into the script folder: 
// frame-messaging.js, jquery-1.8.2.js, jquery-postMessage.js
$(function () {
    //Create a new communication object that talks to the host page.
    var message = IP.frameMessaging();

    var getModel = function () {
        var model = {
            ConfigSetting1: $('#exampleViewField1').val(),
            ConfigSetting2: $('#exampleViewField2').val(),
            ConfigSetting3: $('#exampleViewField3').val(),
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
        $('#exampleViewField1').val(model.ConfigSetting1);
        $('#exampleViewField2').val(model.ConfigSetting2);
        $('#exampleViewField3').val(model.ConfigSetting3);
    });
});