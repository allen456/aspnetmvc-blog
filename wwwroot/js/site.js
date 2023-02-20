$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
    displayMenu(function (json) {
        let menuGrouped = groupBy(json, q => q.moduleController);
        var menustring = "";
        menustring += '<a class="btn btn-sm btn-outline-secondary" href="/Home/Index">Home</a>';
        menuGrouped.forEach(function (menuElement, susi) {
            menuElement.forEach(function (menuChild) {
                menustring += '<a class="link-secondary" href="/' + menuChild.moduleController + '/' + menuChild.moduleAction + '" title="' + menuChild.moduleName + '"><i class="' + menuChild.moduleIconClass + ' mx-2"></i></a>';
            });
        });
        $("#navbar1menu").html(menustring);
    });
    var xhrsocketurl = new XMLHttpRequest();
    xhrsocketurl.onreadystatechange = function () {
        if (xhrsocketurl.readyState === 4) {
            var returnsocketurl = JSON.parse(xhrsocketurl.responseText);
            if (location.protocol == 'https:') {
                returnsocketurl = "wss://" + returnsocketurl;
            }
            else {
                returnsocketurl = "ws://" + returnsocketurl;
            }
            websocketUrl = returnsocketurl;
            connectSocket(returnsocketurl);
        }
    };
    xhrsocketurl.open('GET', '/Account/GetWebSocketURL');
    xhrsocketurl.send();
});
displayMenu = function (callback) {
    if (localStorage.getItem('MenuList')) {
        callback(JSON.parse(localStorage.getItem('MenuList')));
    }
    else {
        $("#navbar1menu").html('<div class="spinner-border" role="status"><span class="sr-only">Loading...</span></div>');
        $.ajax({
            url: "/Account/MenuJSON",
            type: 'GET'
        }).done(function (result) {
            localStorage.setItem('MenuList', JSON.stringify(result));
            callback(result);
        });
    }
};
var websocketUrl = "";
var dataTablesSettings = {
    "decimal": "",
    "emptyTable": "<h1><span class='badge text-bg-secondary'>Empty Results</span></h1>",
    "info": "Showing _START_ to _END_ of _TOTAL_ entries",
    "infoEmpty": "Showing 0 to 0 of 0 entries",
    "infoFiltered": "(filtered from _MAX_ total entries)",
    "infoPostFix": "",
    "thousands": ",",
    "lengthMenu": "Show _MENU_ entries",
    "loadingRecords": "<span class='badge text-bg-secondary'>Loading</span>",
    "processing": "<h1><span class='badge text-bg-secondary'>Loading</h1>",
    "zeroRecords": "<h1><span class='badge text-bg-secondary'>Empty Results</span></h1>",
    "search": "Search: ",
    "paginate": {
        "first": "First",
        "last": "Last",
        "next": "Next",
        "previous": "Previous"
    },
    "aria": {
        "sortAscending": ": activate to sort column ascending",
        "sortDescending": ": activate to sort column descending"
    }
}
function checkJSON(json) {
    if (typeof json == 'object')
        return 'object';
    try {
        return (typeof JSON.parse(json));
    }
    catch (e) {
        return 'string';
    }
};
function displayError(htmlData) {
    var sweetdisplay = document.createElement('div');
    sweetdisplay.innerHTML = htmlData;
    var mainBody = sweetdisplay.getElementsByTagName('main');
    Swal.fire({
        title: 'Request Failed',
        html: mainBody[0].innerHTML,
        confirmButtonText: 'Close'
    });
};
function groupBy(list, keyGetter) {
    let map = new Map();
    list.forEach((item) => {
        let key = keyGetter(item);
        let collection = map.get(key);
        if (!collection) {
            map.set(key, [item]);
        }
        else {
            collection.push(item);
        }
    });
    return map;
}
function connectSocket(socketurl) {
    document.getElementById("wsstateLabel").innerHTML = "connecting";
    socket = new WebSocket(socketurl);
    socket.onopen = function (event) {
        document.getElementById("wsstateLabel").innerHTML = "connected";
    };
    socket.onclose = function (event) {
        document.getElementById("wsstateLabel").innerHTML = "connecting";
        document.getElementById("wsconnidlabel").innerHTML = "";
        let timerInterval = 0;
        const Toast = Swal.mixin({
            toast: true,
            position: 'bottom-end',
            showConfirmButton: false,
            timer: 5000,
            timerProgressBar: true,
        });
        Toast.fire({
            title: 'Connecting',
            didOpen: () => {
                Swal.showLoading();
                timerInterval = setInterval(() => {
                    const content = Swal.getContent()
                    if (content) {
                        const b = content.querySelector('b')
                        if (b) {
                            b.textContent = Swal.getTimerLeft()
                        }
                    }
                }, 100);
            },
            willClose: () => { clearInterval(timerInterval); }
        })
            .then((result) => {
                if (result.dismiss === Swal.DismissReason.timer) {
                    connectSocket(websocketUrl);
                }
            });
    };
    socket.onmessage = function (event) {
        if (isJson(event.data)) {
            var objdata = JSON.parse(event.data);
            if (typeof DataTablesVar === 'object') {
                DataTablesVar.ajax.reload(null, false);
            }
            if (typeof DataTablesVar1 === 'object') {
                DataTablesVar1.ajax.reload(null, false);
            }
            if (typeof DataTablesVar2 === 'object') {
                DataTablesVar2.ajax.reload(null, false);
            }
            if (typeof DataTablesVar3 === 'object') {
                DataTablesVar3.ajax.reload(null, false);
            }
            if (typeof DataTablesVar4 === 'object') {
                DataTablesVar4.ajax.reload(null, false);
            }
        }
        else {
            document.getElementById("wsconnidlabel").innerHTML = event.data;
        }
    };
}
function isJson(str) {
    try {
        JSON.parse(str);
    } catch (e) {
        return false;
    }
    return true;
}
$.fn.dataTable.ext.errMode = function (settings, helpPage, message) {
    if (message.includes("Invalid JSON response.")) {
        Swal.fire({
            html: 'Error reading data from server.',
            icon: 'error',
            title: 'Error',
            confirmButtonText: 'Close'
        });
    } else {
        Swal.fire({
            html: message,
            icon: 'error',
            title: 'Error',
            confirmButtonText: 'Close'
        });
    }
};