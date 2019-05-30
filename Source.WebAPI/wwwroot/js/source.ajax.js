/*****************************************
* GET请求
* 输入参数：请求地址
* 输出参数：json
****************************************/
var get = function (url) {
    var promise = new Promise(function (resolve, reject) {
        var client = new XMLHttpRequest();
        client.open("GET", url, true);
        client.onreadystatechange = handler;
        client.responseType = "json";
        client.setRequestHeader("Accept", "application/json");
        client.send(data);

        function handler() {
            if (this.readyState !== 4) {
                return;
            }
            if (this.status === 200) {
                resolve(this.response);
            } else {
                reject(new Error(this.statusText));
            }
        };
    });

    return promise;
};

/*****************************************
* POST请求
* 输入参数：请求地址,数据对象
* 输出参数：json
****************************************/
var post = function (url, data) {
    var promise = new Promise(function (resolve, reject) {
        var client = new XMLHttpRequest();
        client.open("POST", url, true);
        client.onreadystatechange = handler;
        client.responseType = "json";
        client.setRequestHeader("Content-Type", "application/json");
        client.send(data);

        function handler() {
            if (this.readyState !== 4) {
                return;
            }
            if (this.status === 200) {
                resolve(this.response);
            } else {
                reject(new Error(this.statusText));
            }
        };
    });

    return promise;
};

