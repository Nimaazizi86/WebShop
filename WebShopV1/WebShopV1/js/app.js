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
        $scope.AddToCart = function (Id) {
            $http.get("../Home/AjaxThatReturnsJsonTotal/" + Id)
                 .then(function (response) {
                     $rootScope.order = response.data;
                 })
        }
    });
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


            when("/addItem/:ID", {
                templateUrl: "/partials/store.html",
                controller: "AddItemController"
            }).

            otherwise({
                redirectTo: '/store'
            });



      }]);

})();

