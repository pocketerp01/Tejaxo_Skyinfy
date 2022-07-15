 app.controller("MyCntrl", function ($scope, $http) {
          //  debugger
            $http.get("/home/GetStudent")
                .then(function (res) {
                  //  debugger
                    $scope.dt = res.data;
                });
        });