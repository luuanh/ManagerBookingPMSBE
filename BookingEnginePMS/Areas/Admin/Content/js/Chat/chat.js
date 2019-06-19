var urlGetAll = "/Admin/Chat/GetAllUser"
var urlGetMessage = "/Admin/Chat/GetMessage"
var urlGetAllMessageAndNotify = "/Admin/Chat/GetAllMessageAndNotify"
var urlSendMessage = "/Admin/Chat/SendMessage"
var urlReadNotify = "/Admin/Notification/ReadNotify"

app.controller('chatcontroller', ['$rootScope', '$http', 'notify', '$timeout',
    function ($rootScope, $http, notify, $timeout) {
        $rootScope.boxChat = [];
        $rootScope.userSend = "";
        $rootScope.userSendTo = "";
        $rootScope.messageSend = "";
        $rootScope.init = function () {
            var chat = $.connection.chatHub;
            $rootScope.userSend = $("#UserProfile").attr("username")
            chat.client.broadcastMessage = function (user, sendFrom) {
                if (user == $("#UserProfile").attr("username")) {
                    $("#userSendFrom").val(sendFrom)
                    $("#userSendFrom").click()
                }
            };
            chat.client.requireUserOnline = function (username) {
                chat.server.getAllUserOnline($("#UserProfile").attr("username"));
            };
            chat.client.getAllUserOnline = function (username) {
                $("#listUserOnline").val(username)
                $("#listUserOnline").click()
            };
            chat.client.getNotification = function () {
                $("#btn_getNotification").click()
                otherUpdateNotify = true;
            };
            $.connection.hub.start().done(function () {
                chat.server.requireUserOnline();
                $('#send_message_hidden').click(function () {
                    chat.server.sendMessage($rootScope.userSend, $rootScope.userSendTo);
                });
                $('#get_notify').click(function () {
                    chat.server.getNotification();
                });
            });
        }
        $rootScope.refreshUserOnline = function () {
            var user = $rootScope.users.find(x => x.UserName == $("#listUserOnline").val())
            if (user != null)
                user.Status = true
        }
        $rootScope.getAllUser = function () {
            $http({
                url: urlGetAll,
                method: "GET"
            }).then(function success(respone) {
                $rootScope.users = respone.data
                $rootScope.users.forEach(x => x.Status = false)
                $rootScope.init()
                $rootScope.GetAllMessageAndNotify()
            }, function error(respone) {

            })
        }
        $rootScope.addBoxChat = function (item) {
            if (item.UserName == $("#UserProfile").attr("username"))
                return;
            var indexBoxExist = $rootScope.boxChat.findIndex(x => x.friend == item.UserName)
            if (indexBoxExist >= 0)
                return;
            $rootScope.getInforMessOfBox(item)
        }
        $rootScope.removeBoxChat = function (user) {
            var index = $rootScope.boxChat.findIndex(x => x.friend == user)
            $rootScope.boxChat.splice(index, 1)
        }
        $rootScope.getInforMessOfBox = function (user) {
            $http({
                url: urlGetMessage,
                method: "GET",
                params: {
                    friend: user.UserName
                }
            }).then(function success(respone) {
                var data = JSON.parse(respone.data)
                data.forEach(x => {
                    x.PhotoFriend = user.Photo
                    x.PhotoUser = $("#UserProfile>img").attr("src")
                })
                $rootScope.boxChat.push({
                    friend: user.UserName,
                    friendFullName: user.FullName,
                    messages: data
                })
                $timeout(function () {
                    var index = $rootScope.boxChat.findIndex(x => x.friend == user.UserName)
                    if (index >= 0)
                        $('.chatbox_content_' + index).scrollTop($('.chatbox_content_' + index)[0].scrollHeight);
                }, 10)
            }, function error(respone) {

            })
        }
        $rootScope.SendMessage = function (event, boxMessage, index) {
            if (boxMessage.TextChat == null || boxMessage.TextChat == "")
                return
            if (!event.shiftKey && (event.charCode == 13 || event.keyCode == 13)) {
                $rootScope.userSendTo = boxMessage.friend
                $rootScope.messageSend = boxMessage.TextChat

                var indexBoxExist = $rootScope.boxChat.findIndex(x => x.friend == $rootScope.userSendTo)
                $rootScope.boxChat[indexBoxExist].messages.push({
                    UserSend: $rootScope.userSend,
                    PhotoFriend: '',
                    PhotoUser: $("#UserProfile>img").attr("src"),
                    Date: new Date(),
                    Content: $rootScope.messageSend
                })
                $timeout(function () {
                    $("#txtChat_" + index).val("")
                    $('.chatbox_content_' + index).scrollTop($('.chatbox_content_' + index)[0].scrollHeight);
                }, 10)

                $http({
                    url: urlSendMessage,
                    method: "POST",
                    data: {
                        firstUser: $rootScope.userSend,
                        secondUser: $rootScope.userSendTo,
                        message: $rootScope.messageSend
                    }
                }).then(function success(respone) {
                    $('#send_message_hidden').click()
                    $rootScope.GetAllMessageAndNotify()
                }, function error(respone) {

                })
            }
        }
        $rootScope.refreshBoxMessage = function () {
            var sendFrom = $("#userSendFrom").val()
            var user = $rootScope.users.find(x => x.UserName == sendFrom)
            $http({
                url: urlGetMessage,
                method: "GET",
                params: {
                    friend: user.UserName
                }
            }).then(function success(respone) {
                var data = JSON.parse(respone.data)
                data.forEach(x => {
                    x.PhotoFriend = user.Photo
                    x.PhotoUser = $("#UserProfile>img").attr("src")
                })
                var index = $rootScope.boxChat.findIndex(x => x.friend == user.UserName)
                if (index >= 0) {
                    $rootScope.boxChat[index].messages = data
                }
                else {
                    $rootScope.boxChat.push({
                        friend: user.UserName,
                        friendFullName: user.FullName,
                        messages: data
                    })
                }
                $timeout(function () {
                    index = $rootScope.boxChat.findIndex(x => x.friend == user.UserName)
                    if (index >= 0)
                        $('.chatbox_content_' + index).scrollTop($('.chatbox_content_' + index)[0].scrollHeight);
                }, 10)
                $rootScope.GetAllMessageAndNotify()
            }, function error(respone) {

            })
        }
        $rootScope.getAllUser()
    }]);

app.controller('messageNotifyController', ['$rootScope', '$http', 'notify', '$timeout',
    function ($rootScope, $http, notify, $timeout) {
        $rootScope.numberNotRead = 0;
        $rootScope.userChats = [];
        $rootScope.numberOldNotify = 0;

        $rootScope.GetAllMessageAndNotify = function () {
            $http({
                url: urlGetAllMessageAndNotify,
                method: "GET"
            }).then(function success(respone) {
                var data = JSON.parse(respone.data)
                $rootScope.numberMessageNotRead = data.numberMessageNotRead
                $rootScope.numberNotify = data.numberNotify
                $rootScope.userChats = data.userChats
                $rootScope.notifications = data.notifications
                $rootScope.userChats.forEach(x => {
                    var friend = x.FirstUser == $rootScope.userSend ? x.SecondUser : x.FirstUser
                    var user = $rootScope.users.find(x => x.UserName == friend)
                    if (user != null) {
                        x.Photo = user.Photo
                        x.Friend = user.FullName + " - " + user.UserName
                        x.UserName = user.UserName
                    }
                })

                if ($rootScope.numberOldNotify < $rootScope.numberNotify) {
                    $rootScope.ringNotify()
                }
                $rootScope.numberOldNotify = $rootScope.numberNotify
            }, function error(respone) {

            })
        }
        $rootScope.ringNotify = function () {
            if (otherUpdateNotify)
                playAudio()
        }
        $rootScope.showNotification = function (notify) {
            $("#loader").css("display", "block")
            $rootScope.notifyChosen = notify
            UIkit.modal("#ModalShowNotify").show()
            $("#loader").css("display", "none")
            if (notify.Status == 1) {
                $http({
                    url: urlReadNotify,
                    method: "POST",
                    data: {
                        NotificationId: notify.NotificationId
                    }
                }).then(function success(respone) {
                    $rootScope.GetAllMessageAndNotify()
                }, function error(respone) {

                })
            }
        }
        $rootScope.showMessageBoxByFriend = function (item) {
            $("#userSendFrom").val(item.UserName)
            $rootScope.refreshBoxMessage()
        }
    }]);