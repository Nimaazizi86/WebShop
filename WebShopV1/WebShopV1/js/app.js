/// <reference path="C:\Users\deltagare\Source\Repos\WebShop\WebShopV1\WebShopV1\partials/store.html" />
/// <reference path="C:\Users\deltagare\Source\Repos\WebShop\WebShopV1\WebShopV1\partials/store.html" />
/// <reference path="angular.min.js" />

/*creat angular module with the name of Demo and define some routes for it
*/

(function () {
    'use strict';

    var Demo = angular.module('Demo', [
        // Angular modules
        //'ngAnimate',
        'ngRoute' // <-- This is needed to use AngularJS Routing!
        // Custom modules

        // 3rd Party Modules

    ]);

    Demo.controller('storeController', function ($scope, $http, $rootScope) {

        $http.get("/Home/AjaxThatReturnsJson")
            .then(function (response) {
                $scope.store = response.data;
                $scope.message = "Products List"
            });
        
        $http.get("/Admin/AjaxThatReturnsAdmin")
        .then(function (response) {
            $scope.role = response.data;
            console.log($scope.role);

        });

        $scope.AddToCart = function (Id) {
            $http.get("../Home/AjaxThatReturnsJsonTotal/" + Id)
                 .then(function (response) {
                     $rootScope.order = response.data;
                 })
        }
    });

    Demo.controller("CreateController", function ($scope, $http, $location) {
        $scope.sendform = function () {
            $http({
                method: "POST",
                url: "../home/NewItem",
                data: $scope.form
            })
                   .success(function (data) {
                       if (data.errors) {
                           console.log(data.errors)
                       } else {
                           alert("Posted successfully");
                       }
                       $location.url('/list')
                   })
        }
    })

    Demo.controller("DeleteController", function ($scope, $http, $routeParams, $location, $rootScope) {
        $scope.paramss = $routeParams;

        $http.get("Home/AjaxThatReturnsJsonItem/" + $scope.paramss.Id)
           .then(function (response) {
               $scope.detail = response.data;
               $scope.delmessage = "Confirm deleting"
           });

        $scope.DeleteItem = function (Id) {
            $http.get("../Home/Delete/" + Id)
                 .then(function (response) {
                     $rootScope.order = response.data;
                     $location.url('/store')
                 })
        }

    })
    
    Demo.controller("EditController", function ($scope, $http, $location, $routeParams) {
        $scope.params = $routeParams;
        console.log("Test Edit:");
        console.log($scope.params);
        $http.get("Home/AjaxThatReturnsJsonItem/" + $scope.params.Id)
           .then(function (response) {
               $scope.detailToEdit = response.data;
               console.log("detail:");
               console.log($scope.detailToEdit);
           })

        $scope.sendform = function () {
            alert('saving changes...');
            console.log($scope.detailToEdit);
            $http({
                method: "POST",
                url: "../home/Edit",
                //data: { 'Id': Id }
                data: $scope.detailToEdit
                //data: { 'Id': $scope.detailToEdit }
            })
                   .success(function (data) {
                       if (data.errors) {
                           console.log(data.errors)
                       } else {
                           alert("edited successfully");
                       }
                       $location.url('/Store')
                   })
        }
    })
    
    Demo.controller('checkOutController', function ($scope, $http, $rootScope, $location) {
        $http.get("Home/AjaxThatReturnsJsonCartList")
           .then(function (response) {
               $rootScope.cart = response.data;
               $scope.message = "Cart List"
           });

        $scope.RefreshCart = function () {
            $http.get("Home/AjaxThatReturnsJsonCartList")
             .then(function (response) {
                 $rootScope.cart = response.data;
                 $scope.message = "Cart List"
             });
        }

        $scope.removeFromCart = function (Id) {
            $http.get("../Home/AjaxThatReturnsJsonRemove/" + Id)
                 .then(function (response) {
                     $rootScope.order = response.data;
                     $scope.RefreshCart();
                     //update cart
                 })
        }


    });

    Demo.controller('UserDetailsController', function ($scope, $http, $rootScope, $location) {
        $http.get("Home/userDetails")
           .then(function (response) {
               $rootScope.customer = response.data;
               $scope.infoMessage = "Please enter you info"
           });


        //$scope.BuyItems = function (Id) {
        //    $location.path("/receipt");
        //}

        // using ng-hide/show to show the next form and hide the last things
        $scope.SaveFormData = function () {
            $scope.showInfo = true;
        }

        $scope.EditInformation = function () {
            $scope.showInfo = false;
        }


    });

    Demo.controller('ReceiptController', function ($scope, $http, $rootScope, $location, $routeParams) {
        $scope.paramsss = $routeParams;
        console.log("bUY:");
        console.log($scope.paramsss);
        $http.get("Home/Buy/" + $scope.paramsss.Id)

           .then(function (response) {
               $rootScope.cart = response.data;
               $scope.Tmessage = "Thanks For shopping"
               $scope.message = "Summery of your shoppoing"
           });


        //$scope.BuyItems = function () {
        //    $location.path("/CheckoutUserDetails");
        //}

        // using ng-hide/show to show the next form and hide the last things
        $scope.SaveFormData = function () {
            $scope.showInfo = true;
        }

        $scope.EditInformation = function () {
            $scope.showInfo = false;
        }

        $scope.save = function () {
            $http.get("../Home/SaveFile")
                 .then(function (response) {
                     $rootScope.status = response.data;
                 })

        }

        //$scope.RefreshCart = function () {
        //    $http.get("Home/Buy")
        //     .then(function (response) {
        //         $rootScope.cart = response.data;
        //         $scope.message = "Thanks For Shopping"
        //     });
        //}

        //$scope.removeFromCart = function (Id) {
        //    $http.get("../Home/AjaxThatReturnsJsonRemove/" + Id)
        //         .then(function (response) {
        //             $rootScope.order = response.data;
        //             $scope.RefreshCart();
        //             //update cart
        //         })
        //}

    });

    Demo.config(['$routeProvider', // <-- This is needed to use AngularJS Routing!
      function ($routeProvider) {
          $routeProvider.
            when('/store', {
                templateUrl: '/partials/store.html',
                controller: 'storeController'
            }).
            when('/checkOut', {
                templateUrl: '/partials/checkOut.html',
                controller: 'checkOutController'
            }).


            when("/edit/:Id", {
                templateUrl: "/partials/edit.html",
                controller: "EditController"
            }).

            when("/delete/:Id", {
                templateUrl: "/partials/delete.html",
                controller: "DeleteController"
            }).

            when("/create", {
                templateUrl: "/partials/create.html",
                controller: "CreateController"
            }).

            when("/receipt/:Id", {
                templateUrl: "/partials/receipt.html",
                controller: "ReceiptController"
            }).

            when("/userDetails", {
                templateUrl: "/partials/userDetails.html",
                controller: "UserDetailsController"
            }).


            otherwise({
                redirectTo: '/store'
            });

      }]);

})();

