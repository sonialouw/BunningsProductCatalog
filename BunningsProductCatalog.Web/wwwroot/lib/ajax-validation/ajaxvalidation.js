function getValidationSummary() {
	var $el = $(".validation-summary-errors > ul");
	if ($el.length == 0) {
		$el = $("<div class='validation-summary-errors'><ul></ul></div>")
			.hide()
			.insertBefore('fieldset:first')
			.find('ul');
	}
	return $el;
}

function getResponseValidationObject(response) {
	if (response && response.Tag && response.Tag == "ValidationError")
		return response;
	return null;
}


function isValidationErrorResponse(response, form, summaryElement) {

	var $list,data = getResponseValidationObject(response);
	if (!data) return false;

	$list = summaryElement || getValidationSummary();
	$list.html('');

	$.each(data.State, function (i, item) {

		var $val, lblTxt, errorList = "";

		if (item.Name) {

			var fieldValidationMessage = "form#" + form + " span[data-valmsg-for='" + item.Name + "']";
			$val = $(fieldValidationMessage);

			$val.removeClass("field-validation-valid")
			$val.addClass("invalid-label")
			$val.addClass("invalid-feedback");


			var input = $("form#" + form + " :input[name=" + item.Name + "]");
			input.addClass("is-invalid");
			lblTxt = $("label[for=" + item.Name + "]").text();
			if (lblTxt) { lblTxt += ": "; }
		}

		if ($val != null && $val.length) {
			$val.text(item.Errors.shift());
			if (!item.Errors.length) { return; }
		}

		$.each(item.Errors, function (c, val) {
			errorList += "<li>" + (lblTxt != null ? lblTxt : "") + val + "</li>";
		});

		$list.append(errorList);

	});
	if ($list.find("li:first").length) { $list.closest("div").show(); }
	return true;
}