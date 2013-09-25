(function () {
    // Declare and register the default ExpressForms Filters
    'use strict';
    var filters = {}, x;

    // ExpressFormsFilterText
    filters['ExpressFormsFilterText'] = {
        name: 'ExpressFormsFilterText',
        setupDom: function () {
            var $divFilter, $selection;

            // This is the top level element for each text filter
            $divFilter = $('.ExpressFormsFilterTextDiv');

            $selection = $divFilter.find('select');
            $divFilter.find('input').change(ef.filterCriteriaChanged);
            $divFilter.find('select').change(ef.filterCriteriaChanged);

            $divFilter.find('select').change(function () {
                var selectedValue, $input;

                // Make sure that we match only inputs in the same div as the select box that triggered this.
                $input = $(this).parent().find('input[type="text"]');                                

                selectedValue = $(this).val();
                switch (selectedValue) {
                    case "":
                    case "Blank":                        
                        $input.css('display', 'none')
                            .val('');                        
                        break;
                    case 'Starts With':
                    case 'Contains':                                            
                        $input.css('display', 'inline');                        
                        break;                    
                }
            });
        }, // end setupDom
        getValue: function ($element) {
            var filterText, filterMode;
            filterMode = $element.find('select').val();
            filterText = (filterMode === '' || filterMode === 'Blank') ? '' : $element.find('.ExpressFormsFilterText').val();
            
            return {
                filterText: filterText,
                filterMode: filterMode
            };
        }
    };

    // ExpressFormsFilterNumber
    filters['ExpressFormsFilterNumber'] = {
        name: 'ExpressFormsFilterNumber',
        setupDom: function () {
            var $divFilter, $selection;

            // This is the top level element for each number filter
            $divFilter = $('.ExpressFormsFilterNumberDiv');
            
            $selection = $divFilter.find('select');
            $divFilter.find('input').change(ef.filterCriteriaChanged);
            $divFilter.find('select').change(ef.filterCriteriaChanged);

            $divFilter.find('select').change(function () {
                var selectedValue, $input0, $input1, $text;

                // Make sure that we match only inputs in the same div as the select box that triggered this.
                $input0 = $(this).parent().find('.divFilterInput0');
                $input1 = $(this).parent().find('.divFilterInput1');
                $text = $(this).parent().find('.divFilterText');

                selectedValue = $(this).val();
                switch (selectedValue) {
                    case "":
                        $text.html('');
                        $input0.css('display', 'none')
                        .val('');
                        $input1.css('display', 'none')
                        .val('');
                        break;
                    case 'At Least':
                    case 'At Most':
                    case 'Exactly':
                        $text.html('');
                        $input0.css('display', 'inline');
                        $input1.css('display', 'none')
                        .val('');
                        break;
                    case 'Between':
                        $text.html(' and ');
                        $input0.css('display', 'inline');
                        $input1.css('display', 'inline');
                }
            });
        }, // end setupDom
        getValue: function ($element) {
            var selectVal, input0Val, input1Val, minNumber, maxNumber;
            selectVal = $element.find('select').val();
            input0Val = $element.find('.divFilterInput0').val();
            input1Val = $element.find('.divFilterInput1').val();
            switch (selectVal) {
                case '':
                    minNumber = maxNumber = '';
                    break;
                case 'At Least':
                    minNumber = input0Val;
                    maxNumber = '';
                    break;
                case 'At Most':
                    minNumber = '';
                    maxNumber = input0Val;
                    break;
                case 'Exactly':
                    minNumber = maxNumber = input0Val;
                    break;
                case 'Between':
                    minNumber = input0Val;
                    maxNumber = input1Val;
                    break;
            }
            return {
                minNumber: minNumber,
                maxNumber: maxNumber
            };
        } // end getValue
    }; // end ExpressFormsFilterNumber

    // ExpressFormsFilterDateTime
    filters['ExpressFormsFilterDate'] = {
        name: 'ExpressFormsFilterDate',
        setupDom: function () {
            var $divFilter, $selection;

            // This is the top level element for each date filter
            $divFilter = $('.ExpressFormsFilterDateDiv');

            $selection = $divFilter.find('select');
            $divFilter.find('input').change(ef.filterCriteriaChanged);
            $divFilter.find('select').change(ef.filterCriteriaChanged);

            $divFilter.find('select').change(function () {
                var selectedValue, $input0, $input1, $text;

                // Make sure that we match only inputs in the same div as the select box that triggered this.
                $input0 = $(this).parent().find('.divFilterInput0');
                $input1 = $(this).parent().find('.divFilterInput1');
                $text = $(this).parent().find('.divFilterText');

                selectedValue = $(this).val();
                switch (selectedValue) {
                    case "":
                        $text.html('');
                        $input0.css('display', 'none')
                        .val('');
                        $input1.css('display', 'none')
                        .val('');
                        break;
                    case 'At Earliest':
                    case 'At Latest':
                    case 'Exactly':
                        $text.html('');
                        $input0.css('display', 'inline');
                        $input1.css('display', 'none')
                        .val('');
                        break;
                    case 'Between':
                        $text.html(' and ');
                        $input0.css('display', 'inline');
                        $input1.css('display', 'inline');
                }
            });
        }, // end setupDom
        getValue: function ($element) {
            var selectVal, input0Val, input1Val, minDate, maxDate;
            selectVal = $element.find('select').val();
            input0Val = $element.find('.divFilterInput0').val();
            input1Val = $element.find('.divFilterInput1').val();
            switch (selectVal) {
                case '':
                    minDate = maxDate = '';
                    break;
                case 'At Earliest':
                    minDate = input0Val;
                    maxDate = '';
                    break;
                case 'At Latest':
                    minDate = '';
                    maxDate = input0Val;
                    break;
                case 'Exactly':
                    minDate = maxDate = input0Val;
                    break;
                case 'Between':
                    minDate = input0Val;
                    maxDate = input1Val;
                    break;
            }
            return {
                minDate: minDate,
                maxDate: maxDate
            };
        }
    };

    // ExpressFormsFilterBool
    filters['ExpressFormsFilterBool'] = {
        name: 'ExpressFormsFilterBool',
        setupDom: function () {
            $('div.ExpressFormsFilterBoolDiv')
                .find('select')
                .change(ef.filterCriteriaChanged);
        }, // end setupDom
        getValue: function ($element) {
            var selection;
            selection = $element.find('.ExpressFormsFilterBool').val();
            return {
                selection: selection
            };
        }
    };

    // Register the filters with Express Forms and set up the DOM for each one.
    for (x in filters) {
        window.ef.registerFilter(filters[x]);
        $(document).ready(filters[x].setupDom);
    }

    // Set up autocomplete on the text filters    
    $(document).ready(function () {
        var isUsingFilter, isUsingDialog, $filterDialog, $ExpressFormsFilterText, i;

        // In this code that sets up the filter, the first thing to do is determine whether or not the filter is being used.
        // Check whether or not the filter element exists.
        $ExpressFormsFilterText = $('div.ExpressFormsFilterTextDiv');
        isUsingFilter = $ExpressFormsFilterText.length > 0;
        if (!isUsingFilter)
            return;
        // Next, determine whether or not the filter is being displayed in a dialog.        
        $filterDialog = $('#dialogFilter');
        isUsingDialog = $filterDialog.length > 0;

        for (i = 0; i < $ExpressFormsFilterText.length; i++) {
            var $thisFilter, $query, autocompleteEnabled;
            $thisFilter = $($ExpressFormsFilterText[i]);
            $query = $thisFilter.find('input[type="text"]');
            autocompleteEnabled = $query.attr('data-autocompleteenabled') == 'True';

            if (autocompleteEnabled) {
                setupAutocompleteOnElement($thisFilter, isUsingDialog ? $filterDialog : null);
            } // end if autocompleteEnabled
        } // end for x in $ExpressFormsFilterText
    }); // end $document ready

    // $thisFilter tells which filter we want to setup autocomplete on.
    // $filterDialog optionally tells the function to attach the autocomplete to a jquery-ui dialog.
    function setupAutocompleteOnElement($thisFilter, $filterDialog) {
        var $query, fieldName, autocompleteUrl, autocompleteMaxMatches, filterValues;

        $query = $thisFilter.find('input[type="text"]');
        fieldName = $query.attr('id');
        autocompleteMaxMatches = $query.attr('data-autocompletemaxmatches');
        autocompleteUrl = $query.attr('data-autocompleteurl');

        $query.autocomplete({
            source: function (request, returnAutocompleteEntries) {
                $.ajax({
                    url: autocompleteUrl,
                    dataType: 'json',
                    data: {
                        fieldName: fieldName,
                        filterValues: filters['ExpressFormsFilterText'].getValue($thisFilter),
                        maxMatches: autocompleteMaxMatches
                    },
                    success: function (results) {
                        var mapFunction, autocompleteEntries;
                        mapFunction = function (item) {
                            return {
                                label: item,
                                value: item
                            };
                        };
                        autocompleteEntries = $.map(results, mapFunction);
                        returnAutocompleteEntries(autocompleteEntries);
                    }, // end success handler                                    
                    error: function (xhr) {
                        alert(xhr.responseText);
                    }
                }); // end $.ajax
            },
            minlength: 2
        }); // end $.autocomplete

        // Attach autocomplete to jquery-ui dialog if one was passed in.
        if ($filterDialog)
            $query.autocomplete("option", "appendTo", $filterDialog);
    }
})();