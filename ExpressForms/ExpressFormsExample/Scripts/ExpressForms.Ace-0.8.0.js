(function () {
    // ExpressFormsAceInput
    window.ef.registerInputFormIO({
        name: 'ExpressFormsAceInput',
        getValue: function ($element) {
            var id, editor;
            id = $element.attr('id');
            editor = ace.edit(id);
            return editor.getValue();
        }, // end getValue
        setValue: function ($element, value) {
            var id, editor;
            id = $element.attr('id');
            editor = ace.edit(id);
            editor.setValue(value);
        } // end setValue
    });
})();