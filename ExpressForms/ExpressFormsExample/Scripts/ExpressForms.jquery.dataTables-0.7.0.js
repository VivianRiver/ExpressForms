// This Javascript, along with jQuery, provides the code that Express Forms uses to
// make the jquery.dataTables extension work client-side.

(function () {
    'use strict';
    var ajaxExtension;

    function initTable($table) {        
        $table.dataTable({
            "bProcessing": true,
            "bServerSide": true,
            "sAjaxSource": $('#hdnGetAjaxUrl').val(),
            "fnDrawCallback": function () {
                window.ef.resetButtonHandlers();
            },
            // Don't use built in filtering because we will use our own.
            "bFilter": false,
            // This part tells ExpressForms to read the filter values and send them to the server.
            "fnServerParams": function (aoData, fnCallback) {
                aoData.push({ name: 'ef_Filter', value: JSON.stringify(ef.readFilterCriteria()) });
            }
        });
    }
    
    function redrawTable($table) {
        $table.fnDraw();
    }

    ajaxExtension = {
        initTable: initTable,
        redrawTable: redrawTable
    };

    ef.registerAjaxExtension(ajaxExtension);
})();