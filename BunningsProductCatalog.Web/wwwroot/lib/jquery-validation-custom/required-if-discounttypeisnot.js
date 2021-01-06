$.validator.addMethod("requiredifdiscounttypeisnot", function (value, element, params) {
	var actualValue = $("input:radio[name ='" + params[0] + "']:checked").val();

	var required = actualValue !== params[1];

	if (required && (element.value === "")) {
		return false;
	}
	return true;
	//return params[1] != $("input:radio[name ='" + params[0] + "']:checked").val();
});

$.validator.unobtrusive.adapters.add("requiredifdiscounttypeisnot", ["otherpropertyname", "otherpropertyvalue"], function (options) {
	options.rules["requiredifdiscounttypeisnot"] = [options.params.otherpropertyname, options.params.otherpropertyvalue];
	options.messages["requiredifdiscounttypeisnot"] = options.message;
});