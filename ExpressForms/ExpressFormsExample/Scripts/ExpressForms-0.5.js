// A journey of a thousand miles begins with the first step.

// Express Forms v0.1
// Requires jQuery

(function () {
    'use strict';

    var ef, inputFormIos = {}, buttons = {};

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
            var inputType, inputMultiplicity;
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

    function registerInputFormIO(io) {
        inputFormIos[io.name] = io;
    }
    function registerButton(button) {
        buttons[button.name] = button;
        $(document).ready(function () { $('.' + button.name).click(button.clickHandler); });
    }

    ef = {
        InputFormIO: InputFormIO,
        registerInputFormIO: registerInputFormIO,
        registerButton: registerButton,
        readFromForm: readFromForm,
        writeToForm: writeToForm
    };

    window.ef = ef;

})();