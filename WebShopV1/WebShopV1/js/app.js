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


    Demo.controller("EditController", function ($scope, $http, $location, $routeParams) {
        $scope.params = $routeParams;
        $http.get("Home/AjaxThatReturnsJsonItem/" + $scope.params.Id)
           .then(function (response) {
               $scope.detailToEdit = response.data;
           })

        $scope.sendform = function () {
            alert('saving changes...');
            console.log($scope.detailToEdit);
            $http({
                method: "POST",
                url: "../home/Edit",
                data: $scope.detailToEdit
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


            //when("/addItem/:ID", {
            //    templateUrl: "/partials/store.html",
            //    controller: "AddItemController"
            //}).

            when("/edit/:ID", {
                templateUrl: "/partials/edit.html",
                controller: "EditController"
            }).

            otherwise({
                redirectTo: '/store'
            });



      }]);

})();

