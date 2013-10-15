(function () {
    'use strict';
    // Attach click handlers to any EF [Edit] button on screen.
    window.ef.registerButton({
        name: 'ExpressFormsEditButton',
        clickHandler: ExpressFormsEditButton_click
    });
    // Attach click handlers to 'INSERT', 'UPDATE', and 'DELETE' buttons on the screen
    window.ef.registerButton({
        name: 'ExpressFormsModifyDataButton',
        clickHandler: ExpressFormsModifyDataButton_click
    });

    function ExpressFormsEditButton_click() {
        var linkUrl;
        linkUrl = $(this).attr('data-linkurl');

        // Direct the user to the page where he can edit the data.
        if (linkUrl) {
            self.location = linkUrl;
        }
    } // end ExpressFormsEditButton_click

    function ExpressFormsModifyDataButton_click() {
        var $button, confirmationMessage, formName, postUrl, actionType, postType, idForDeletion, tableIdForDeletion, values;
        $button = $(this);

        // If a confirmation message is supplied, show it; then cancel this operation if the user clicks [Cancel].
        confirmationMessage = $button.attr('data-message');
        if (confirmationMessage && !confirm(confirmationMessage))
            return;

        formName = $button.attr('data-formname');
        postUrl = $button.attr('data-posturl');
        postType = $button.attr('data-posttype');
        actionType = $button.attr('data-actiontype');
        idForDeletion = $button.attr('data-id'); // only used for deletion
        tableIdForDeletion = $button.attr('data-tableid'); // only used for deletion

        if (actionType.toUpperCase() === 'DELETE') {
            // The delete button has a data-id attribute that tells which record to delete.  The others read the record from a form.
            values = { id: idForDeletion };
        } else {
            values = window.ef.readFromForm(formName);
        }

        if (postType && postType.toUpperCase() === 'AJAX') {
            processAjaxPost($button, formName, postUrl, postType, actionType, idForDeletion, tableIdForDeletion, values);
        } else if (postType && postType.toUpperCase() === 'FORM') {
            processFormPost(formName, postUrl, postType, actionType, values);
        } else {
            alert('Invalid postType encountered: ' + postType);
        }

        function processAjaxPost($button, formName, postUrl, postType, actionType, idForDeletion, tableIdForDeletion, values) {
            values.actionType = actionType;
            values.postType = postType;

            $.ajax({
                url: postUrl,
                data: JSON.stringify(values),
                type: 'POST',
                contentType: 'application/json',
                dataType: 'json',
                success: function (response) {
                    var isInsert, isDelete;
                    isInsert = actionType.toUpperCase() === 'INSERT';
                    isDelete = actionType.toUpperCase() === 'DELETE';
                    if (isInsert) {
                        // Upon a successful insertion, change the insert button to an update button and put the ID returned on the form.
                        $button.attr('data-actiontype', 'UPDATE');
                        values.Id = response.Id;
                        window.ef.writeToForm(formName, values);
                    } else if (isDelete) {
                        // Upon a successful deletion, remove the deleted row from the display.
                        var $table, $rowToDelete;

                        // Find the row to delete.
                        $table = $('table[data-formname="0"]'.replace(/0/, tableIdForDeletion))
                        $rowToDelete = $table.find('tr[data-rowid="0"]'.replace(/0/, idForDeletion))
                        if ($rowToDelete.length === 0) {
                            // If an AJAX extension is used, this method may not find the row, so instead, look for a TR tag that contains the button.                            
                            $rowToDelete = $button.parent().parent();
                        }
                        $rowToDelete.remove();
                    }
                },
                error: function (xhr) { alert(xhr.responseText) }
            });
        }

        // When a Postback button is clicked, ExpressForms does the following:
        // (1) Read the data from a form specified by the data-formname attribute on the button.    
        // (2) Create an HTML form with inputs containing the form data and add it to a hidden div on the page.    
        // (3) The form will have method and action specified by the data-method and data-actionurl attributes, respectively.
        // (4) Submit the form.
        // You may ask, "Why not just submit the form as it is?"
        // The answer is, "Because the form may be developed with custom Express Forms extensions that don't post as ordinary forms.
        function processFormPost(formName, postUrl, postType, actionType, values) {
            var $form, x;
            // Verify that method is either POST or GET                                    
            $form = createHiddenForm(postUrl, 'POST');
            for (x in values) {
                addInputWithValueToForm($form, x, values[x]);
            }
            addInputWithValueToForm($form, 'actionType', actionType);
            addInputWithValueToForm($form, 'postType', postType);
            $(document.body).append($form);
            $form.submit();

            function createHiddenForm(actionUrl, method) {
                var $form;
                $form = $('<form>', {
                    method: method,
                    action: actionUrl
                }).css('display', 'none');
                return $form;
            }
            function addInputWithValueToForm($form, name, value) {
                // This may look like an odd way to build a form, but this is what I found will make the form work without choking on line-breaks and < > &
                var $input;
                $input = $('<textarea>', {
                    name: name
                }).text(value);
                $form.append($input);
            }
        }
    }
})();