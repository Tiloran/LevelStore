define(["require", "exports", "jquery"], function (require, exports, $) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    class UserList {
        constructor() {
            this.users = new Array();
        }
        load() {
            $.getJSON('http://localhost:21204/Home/GetUsers', (data) => {
                this.users = data;
                alert('данные загружены');
            });
        }
        displayUsers() {
            var table = '<table class="table">';
            for (var i = 0; i < this.users.length; i++) {
                var tableRow = '<tr>' +
                    '<td>' + this.users[i].Id + '</td>' +
                    '<td>' + this.users[i].Name + '</td>' +
                    '<td>' + this.users[i].Age + '</td>' +
                    '</tr>';
                table += tableRow;
            }
            table += '</table>';
            $('#content').html(table);
        }
    }
    class User {
    }
    window.onload = () => {
        var userList = new UserList();
        $("#loadBtn").click(() => { userList.load(); });
        $("#displayBtn").click(() => { userList.displayUsers(); });
    };
});
//# sourceMappingURL=test.js.map