/*
 * Description: secretSafe namespace js file and viewmodels declaration
 */

// Namespace
var secretSafe = {};

// Models

secretSafe.chatMessage = function (sender, content, dateSent, color) {
    var self = this;
    self.username = sender;
    self.content = content;
    if (dateSent != null) {
        self.timestamp = dateSent;
    }
    self.color = color;

    
    console.log(sender + " for js");
    console.log(content + " for js");
    console.log(dateSent + " for js");
    console.log(color + " for js");
}

secretSafe.user = function (username, userId, roomname, color) {
    var self = this;
    self.username = username;
    self.id = userId;
    self.roomname = roomname;
    self.color = color;


}

// ViewModels

secretSafe.chatViewModel = function () {
    var self = this;
    self.messages = ko.observableArray();
}

secretSafe.connectedUsersViewModel = function () {
    var self = this;
    self.contacts = ko.observableArray();
    self.customRemove = function (userToRemove) {
        var userIdToRemove = userToRemove.id;
        self.contacts.remove(function (item) {
            return item.id === userIdToRemove;
        });
    }
}

