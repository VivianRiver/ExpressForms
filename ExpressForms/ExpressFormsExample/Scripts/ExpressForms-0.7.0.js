// A journey of a thousand miles begins with the first step.
// Yes, it did :-)

// Express Forms v0.7.0
// Requires jQuery

(function () {
    'use strict';

    var ef, inputFormIos = {}, buttons = {}, filters = {}, ajaxExtension, $table;

    ef = {};

    /* Constructor function for InputFormIO.
    Creates an object that can be used to read and write values from a particular type of Express Forms input.
    It is called internally to create the InputIO objects for the basic HTML input types such as text, checkbox, select, and textarea.
    This constructor should be used to extend ExpressForms with new input types once they have been defined in the server-side code. */
    function InputFormIO(name, getValue, setValue) {
        this.name = name;
        this.getValue = getValue; // function($element)
        this.setValue = setValue; // function($element, value)
    }

    /* Constructor function for Button.
    Creates an object that represents a type of button to be used with Express Forms.
    It is used internally to set up the built-in Express Forms buttons.
    This constructor should be used to extend ExpressForms with new buttons once they have been defined in the server-side code. */
    function Button(name, clickHandler) {
        this.name = name;
        this.clickHandler = clickHandler;
    }

    /* Constructor function for Filter.
    Creates an object that represents a type of filter for the user to filter data by on an index page with Express Forms.
    It is called internally to create the Filter objects for the basic field types such as string, integer, and DateTime.
    This constructor should be used to extend ExpressForms with new filters once they have been defined in the server-side code. */
    function Filter(name, getValue, filterChangeHandler) {
        this.name = name;
        this.getValue = getValue;
    }

    function readFromForm(formName) {
        var $formElements, values;
        $formElements = getFormElements(formName);
        values = {};
        $formElements.each(function () {
            var $element, key, value;
            $element = $(this);
            key = $element.attr('data-inputname');
            value = readSingleValueFromForm($element);
            values[key] = value;
        });
        return values;

        function readSingleValueFromForm($element) {
            var inputType;
            inputType = getFormElementInputType($element);
            return inputFormIos[inputType].getValue($element);
        }
    }

    function writeToForm(formName, values) {
        var $formElements, key;
        $formElements = getFormElements(formName);

        $formElements.each(function () {
            var $element, key, value;
            $element = $(this);
            key = $element.attr('data-inputname');
            value = values[key];
            writeSingleValueToForm($element, value);
            values[key] = value;
        });

        function writeSingleValueToForm($element, value) {
            var inputType;
            inputType = getFormElementInputType($element);
            inputFormIos[inputType].setValue($element, value);
        }
    }

    function getFormElements(formName) {
        var $formElements = $('.ExpressForms[data-formname="0"]'.replace(/0/g, formName));
        return $formElements;
    }

    function getFormElementInputType($element) {
        return $element.attr('data-inputtype');
    }

    function resetButtonHandlers() {
        var x, button;
        for (x in buttons) {
            button = buttons[x];
            $('.' + button.name)
                .unbind('click')
                .bind('click', button.clickHandler);
        }
    }

    function filterCriteriaChanged() {
        redrawAjaxTable();
    }

    // Reads what the user entered into the filter forms.
    // It is expected that this function will be called by an AJAX extension to send the filter criteria to the server.
    function readFilterCriteria() {
        var $filterElements, values;
        $filterElements = getFilterElements();
        values = {};
        $filterElements.each(function () {
            var $element, key, value;
            $element = $(this);
            key = $element.attr('data-filtername');
            value = readSingleValueFromFilterGroup($element);
            values[key] = value;
        });
        return values;

        function readSingleValueFromFilterGroup($element) {
            var filterType;
            filterType = getFilterElementType($element);
            return filters[filterType].getValue($element);
        }
    }

    function getFilterElements() {
        var $filterElements = $('.ExpressFormsFilterGroup')
            .find('.ExpressFormsFilter');
        return $filterElements;
    }

    function getFilterElementType($element) {
        return $element.attr('data-filtertype');
    }

    // When an AJAX extension is present, call a method to redraw the table.
    // This should be used when the user edits the filtering criteria.
    function redrawAjaxTable() {
        if (!ajaxExtension)
            throw new Error('Cannot redraw table without ajax extension present.');
        ajaxExtension.redrawTable($table);
    }

    function registerInputFormIO(io) {
        inputFormIos[io.name] = io;
    }
    function registerButton(button) {
        buttons[button.name] = button;
    }
    function registerFilter(filter) {
        filters[filter.name] = filter;
    }

    function registerAjaxExtension(extension) {
        // Only one object may be passed in.
        if (ajaxExtension)
            throw new Error('Only one ajaxExtension is allowed.');
        // It is expected that the object passed in have two methods:
        // initTable and redrawTable to which we will pass the jQuery object representing the table.
        if (!extension.initTable)
            throw new Error('Ajax Extension missing initTable method.');
        else if (!extension.redrawTable)
            throw new Error('Ajax Extension missing redrawTable method.');
        ajaxExtension = extension;
    }

    $(document).ready(function () {
        // When the document is ready, load the table into memory to be used with the ajax extension (if any)
        // and set up all button handlers.                

        resetButtonHandlers();

        if (ajaxExtension) {
            $table = $('.ExpressFormsTableContainer').find('table');
            ajaxExtension.initTable($table);
        }
    });

    ef = {
        InputFormIO: InputFormIO,
        registerInputFormIO: registerInputFormIO,
        registerButton: registerButton,
        registerFilter: registerFilter,
        registerAjaxExtension: registerAjaxExtension,
        readFromForm: readFromForm,
        writeToForm: writeToForm,
        resetButtonHandlers: resetButtonHandlers,
        filterCriteriaChanged: filterCriteriaChanged,
        readFilterCriteria: readFilterCriteria,
        redrawAjaxTable: redrawAjaxTable,
        ajaxExtension: ajaxExtension
    };

    window.ef = ef;
})();