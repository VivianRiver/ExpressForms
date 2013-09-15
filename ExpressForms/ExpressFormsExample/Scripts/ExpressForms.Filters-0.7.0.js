// Register the default ExpressForms Filters
(function () {
    'use strict';
    // ExpressFormsFilterText
    window.ef.registerFilter({
        name: 'ExpressFormsFilterText',
        getValue: function ($element) {
            var filterText, filterMode;
            filterText = $element.find('.ExpressFormsFilterText').val();
            filterMode = $element.find('input[type="radio"]:checked').val();
            return {
                filterText: filterText,
                filterMode: filterMode
            };
        }
    });

    // ExpressFormsFilterNumber
    window.ef.registerFilter({
        name: 'ExpressFormsFilterNumber',
        getValue: function ($element) {
            var minNumber, maxNumber;
            minNumber = $element.find('.ExpressFormsFilterNumberMin').val();
            maxNumber = $element.find('.ExpressFormsFilterNumberMax').val();
            return {
                minNumber: minNumber,
                maxNumber: maxNumber
            };
        }
    });

    // ExpressFormsFilterDateTime
    window.ef.registerFilter({
        name: 'ExpressFormsFilterDate',
        getValue: function ($element) {
            var minDate, maxDate;
            minDate = $element.find('.ExpressFormsFilterDateMin').val();
            maxDate = $element.find('.ExpressFormsFilterDateMax').val();
            return {
                minDate: minDate,
                maxDate: maxDate
            };
        }
    });

    // ExpressFormsFilterBool
    window.ef.registerFilter({
        name: 'ExpressFormsFilterBool',
        getValue: function ($element) {
            var selection;
            selection = $element.find('.ExpressFormsFilterBool').val();
            return {
                selection: selection
            };
        }
    });
})();