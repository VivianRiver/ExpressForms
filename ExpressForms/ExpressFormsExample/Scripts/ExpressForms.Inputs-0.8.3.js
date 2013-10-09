// Register the default ExpressForms FormInputIOs
(function () {
    'use strict';
    // ExpressFormsTextBox
    window.ef.registerInputFormIO({
        name: 'ExpressFormsTextBox',
        getValue: function ($element) {
            return $element.val();
        }, // end getValue
        setValue: function ($element, value) {
            $element.val(value);
        } // end setValue
    });

    // ExpressFormsTextArea
    window.ef.registerInputFormIO({
        name: 'ExpressFormsTextArea',
        getValue: function ($element) {
            return $element.val();
        }, // end getValue
        setValue: function ($element, value) {
            $element.val(value);
        } // end setValue
    });

    // ExpressFormsCheckBox
    window.ef.registerInputFormIO({
        name: 'ExpressFormsCheckBox',
        getValue: function ($element) {
            return $element.is(':checked');
        }, // end getValue
        setValue: function ($element, value) {
            if (value)
                $element.attr('checked', 'checked');
            else
                $element.removeAttr('checked');
        } // end setValue
    });

    // ExpressFormsListBox
    window.ef.registerInputFormIO({
        name: 'ExpressFormsListBox',
        getValue: function ($element) {
            return (function () {
                var idStringArray, value, x;
                idStringArray = $element.val();
                value = [];
                for (x in idStringArray) {
                    var id;
                    id = parseInt(idStringArray[x]);
                    value.push({ id: id })
                }
                return value;
            })();
        }, // end getValue
        setValue: function ($element, value) {
            alert('setting value to ExpressFormsListBox is not yet implemented');
        } // end setValue
    });

    // ExpressFormsDropDownList
    window.ef.registerInputFormIO({
        name: 'ExpressFormsDropDownList',
        getValue: function ($element) {
            return $element.val();
        }, // end getValue
        setValue: function ($element, value) {
            $element.val(value);
        } // end setValue
    });
})();