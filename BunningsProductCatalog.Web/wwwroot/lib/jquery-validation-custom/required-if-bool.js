$.validator.addMethod("requiredifbool", function (value, element, params) {
	var e = $("input[name='" + params[0] + "']");
	var actualValue;
	var type = e.prop('type');
	if (type === "checkbox") {
		actualValue = e.is(":checked");
	} else {
		if (type === "radio") {
			actualValue = $("input[name='" + params[0] + "']:checked").val().toLowerCase() === "true";
		} else {
			actualValue = e.val().toLowerCase() === "true";
		}
	}

	var boolParam = params[1] === "True";

	var required = actualValue === boolParam;

	if (required && (element.value === "")) {
		return false;
	}
	return true;
});

$.validator.unobtrusive.adapters.add("requiredifbool", ["otherpropertyname", "otherpropertyvalue"], function (options) {
	options.rules["requiredifbool"] = [options.params.otherpropertyname, options.params.otherpropertyvalue];
	options.messages["requiredifbool"] = options.message;
});