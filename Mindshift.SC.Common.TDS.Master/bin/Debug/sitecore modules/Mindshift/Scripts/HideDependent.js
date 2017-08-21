function HideDependentFields(element, useEffects) {

	var action = 'show',
			$element = $(element),
			hideByDefault = $element.readAttribute('data-hide-default'),
			hideCount = $element.readAttribute('data-hide-count'),
			hideValues = $element.readAttribute('data-hide-values'),
			siblingElements = $element.up('.scEditorFieldMarker').nextSiblings();
	//hideFieldsJSON = $element.readAttribute('data-hide-fields');//

	var hideFields = JSON.parse($element.readAttribute('data-hide-fields').replace(/'/g, "\""));
	var hiddenFields = [];
	for (var i = 0; i < hideFields.length; i++) {
		if ($element.getValue().replace(/{/g, '').replace(/}/g, '') == hideFields[i].source) { // TODO: double check casing, etc.
			hiddenFields = hideFields[i].fields;
			break;
		}
	}

	var allSiblingElements = siblingElements;

	// TODO: get this array differently...
	siblingElements = []; // blank result
	//siblingElements = siblingElements[0];
	for (var i = 0; i < hiddenFields.length; i++) {
		//siblingElements.push($(jQuery('.scEditorFieldLabel:contains("' + hiddenFields[i] + '")').closest('table.scEditorFieldMarker')[0])); // note: $ is converting to prototype

		siblingElements.push(
			$(
				jQuery('.scEditorFieldLabel')
				.filter(function () {
					return jQuery(this).text().replace(/ \[.*?\]/g, '') == hiddenFields[i] + ':'; // note: the replace is to remove anything in []
				})

			.closest('table.scEditorFieldMarker')[0]
			)
		); // note: $ is converting to prototype


	}

	// for checkbox type elements check if checked
	if (element.type == 'checkbox') {
		if ((!element.checked && hideByDefault == 'true') || (element.checked && hideByDefault == 'false')) {
			action = 'hide';
		}
	}

	// TODO: figure out how it was showing again?

	// for select elements check the selected value
	if (element.nodeName.toLowerCase() == 'select') {
		if (hiddenFields && hiddenFields.length > 0) {
			action = 'hide';
		} else {
			var elVal = $element.getValue();
			if (hideByDefault && elVal == '') {
				action = 'hide';
			}
			else if (hideValues.length > 0) {
				var arrValues = hideValues.split('|');
				if (arrValues.indexOf(elVal) != -1) {
					action = 'hide';
				}
			}
		}
	}


	// TODO: opposite now!
	// show all...
	allSiblingElements.invoke('show');

	// hide relevant.
	siblingElements.invoke('hide');
		

	// otherwise use fade effects
	//var effectFn = (action == 'show') ? function (el) { el.appear({ duration: 0.5 }) } : function (el) { el.fade({ duration: 0.5 }) };
	//Effect.multiple(siblingElements, effectFn, { speed: 0 });

};

// add script to be called when content editor is loaded
document.observe("sc:contenteditorupdated", function (event) {
	$$('.hide-dependent-fields').each(function (element, index) {
		HideDependentFields(element, false);
	});
});