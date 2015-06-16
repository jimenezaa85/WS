/// <reference path="app/Tax.js" />
(function () {
    require.config({
        baseUrl: "Scripts/lib",
        paths: {
            jquery: "jquery-1.10.2.min"
            
        }
    });

    define(['jquery'], function ($) { });
    require(['kendo/kendo.all.min'], function (kendo) {
        require(['../app/Tax']);
    });
    
   
})();