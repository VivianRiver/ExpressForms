(function () {
    // Declare and register the default ExpressForms Filters
    'use strict';
    var filters = {}, x;

    // ExpressFormsFilterText
    filters['ExpressFormsFilterText'] = (function () {
        // Helper function to show and hide the text inputs as the user changes the value in the select box.
        function showHideInputs($element) {
            var selectedValue, $select, $input;
            $select = $element.find('select');
            $input = $element.find('input[type="text"]');
            selectedValue = $select.val();
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
        }

        return {
            name: 'ExpressFormsFilterText',
            setupDom: function () {
                var $divFilter, $selection;

                // This is the top level element for each text filter
                $divFilter = $('.ExpressFormsFilterTextDiv');

                $selection = $divFilter.find('select');
                $divFilter.find('input').change(ef.filterCriteriaChanged);
                $divFilter.find('select').change(ef.filterCriteriaChanged);

                $divFilter.find('select').change(function () {
                    var selectedValue, $element;
                    // Make sure that we match only inputs in the same div as the select box that triggered this.
                    $element = $(this).parent();
                    showHideInputs($element);
                });
            }, // end setupDom
            getValue: function ($element) {
                var filterText, filterMode;
                filterMode = $element.find('select').val();
                filterText = (filterMode === '' || filterMode === 'Blank') ? '' : $element.find('.ExpressFormsFilterText').val();

                return {
                    filterText: filterText,
                    filterMode: filterMode,
                    isDefaultValue: function () {
                        return filterMode === '' && filterText === '';
                    }
                };
            },
            getDefaultValue: function ($element) {
                return {
                    filterText: '',
                    filterMode: ''
                };
            },
            setValue: function ($element, value) {
                $element.find('select').val(value.filterMode);
                $element.find('.ExpressFormsFilterText').val(value.filterText);
                showHideInputs($element);
            }
        };
    })(); // end ExpressFormsFilterText

    // ExpressFormsFilterNumber
    filters['ExpressFormsFilterNumber'] = (function () {
        // Helper function to show and hide the text inputs as the user changes the value in the select box.
        function showHideInputs($element) {
            var $select, $input0, $input1, $text;
            $select = $element.find('select');
            $input0 = $element.find('.divFilterInput0');
            $input1 = $element.find('.divFilterInput1');
            $text = $element.find('.divFilterText');

            switch ($select.val()) {
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
        } // end function showHideInputs

        return {
            name: 'ExpressFormsFilterNumber',
            setupDom: function () {
                var $divFilter, $selection;

                // This is the top level element for each number filter
                $divFilter = $('.ExpressFormsFilterNumberDiv');

                $selection = $divFilter.find('select');
                $divFilter.find('input').change(ef.filterCriteriaChanged);
                $divFilter.find('select').change(ef.filterCriteriaChanged);

                $divFilter.find('select').change(function () {
                    showHideInputs($(this).parent());
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
                    maxNumber: maxNumber,
                    isDefaultValue: function () {
                        return minNumber === '' && maxNumber === '';
                    }
                };
            }, // end getValue        
            getDefaultValue: function ($element) {
                return {
                    minNumber: '',
                    maxNumber: ''
                };
            },
            setValue: function ($element, value) {
                var $select, $text0, $text1;
                $select = $element.find('select');
                $text0 = $element.find('.divFilterInput0');
                $text1 = $element.find('.divFilterInput1');
                if (value.minNumber === '' && value.maxNumber === '') {
                    $select.val('');
                } else if (value.minNumber !== '' && value.maxNumber === '') {
                    $select.val('At Least');
                    $text0.val(value.minNumber);
                } else if (value.minNumber === '' && value.maxNumber !== '') {
                    $select.val('At Most');
                    $text0.val(value.maxNumber);
                } else {
                    if (value.minNumber === value.maxNumber) {
                        $select.val('Exactly');
                        $text0.val(value.minNumber);
                    } else {
                        $select.val('Between');
                        $text0.val(value.minNumber);
                        $text1.val(value.maxNumber);
                    }
                }
                showHideInputs($element);
            } // end setValue
        }; // end return
    })(); // end ExpressFormsFilterNumber

    // ExpressFormsFilterDateTime
    filters['ExpressFormsFilterDate'] = (function () {
        // Helper function to show and hide the text inputs as the user changes the value in the select box.
        // At this writing, this is indeed exactlt the same helper function used with ExpressFormsFilterNumber,
        // but perhaps it will change in the future.
        function showHideInputs($element) {
            var $select, $input0, $input1, $text;
            $select = $element.find('select');
            $input0 = $element.find('.divFilterInput0');
            $input1 = $element.find('.divFilterInput1');
            $text = $element.find('.divFilterText');

            switch ($select.val()) {
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
        } // end function showHideInputs

        return {
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
                    maxDate: maxDate,
                    isDefaultValue: function () {
                        return minDate === '' && maxDate === '';
                    }
                };
            }, // end getValue
            getDefaultValue: function ($element) {
                return {
                    minDate: '',
                    maxDate: ''
                };
            },
            setValue: function ($element, value) {
                var $select, $text0, $text1;
                $select = $element.find('select');
                $text0 = $element.find('.divFilterInput0');
                $text1 = $element.find('.divFilterInput1');
                if (value.minDate === '' && value.maxDate === '') {
                    $select.val('');
                } else if (value.minDate !== '' && value.maxDate === '') {
                    $select.val('At Earliest');
                    $text0.val(value.minDate);
                } else if (value.minDate === '' && value.maxDate !== '') {
                    $select.val('At Latest');
                    $text0.val(value.maxDate);
                } else {
                    if (value.minDate === value.maxDate) {
                        $select.val('Exactly');
                        $text0.val(value.minDate);
                    } else {
                        $select.val('Between');
                        $text0.val(value.minDate);
                        $text1.val(value.maxDate);
                    }
                }
                showHideInputs($element);
            } // end setValue
        };
    })(); // end ExpressFormsFilterDate

    // ExpressFormsFilterBool
    filters['ExpressFormsFilterBool'] = (function () {
        return {
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
                    selection: selection,
                    isDefaultValue: function () {
                        return selection === $element.find('select option:first').val();
                    }
                };
            },
            getDefaultValue: function ($element) {
                return {
                    // Will return either True False and Null, or True and False depending on whether
                    // the value is nullable.
                    selection: $element.find('select option:first').val()
                };
            },
            setValue: function ($element, value) {
                $element.find('select').val(value.selection);
            }
        };
    })(); // end ExpressFormsFilterBool

    // Register the filters with Express Forms.
    for (x in filters) {
        window.ef.registerFilter(filters[x]);
    }
    // When the document is ready, set up the DOM for each filter and prefill it from the URL, if possible.
    $(document).ready(function () {
        for (x in filters) {
            filters[x].setupDom();
        }
    });

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