import '../styles/site.scss';

import $ from 'jquery';

window.jQuery = $;
window.$ = $;
window.jquery = $;
window.moment = moment;

require('velocity-animate');
import bsCustomFileInput from 'bs-custom-file-input';

global.bsCustomFileInput = bsCustomFileInput;

import 'popper.js';
import 'bootstrap';
import 'mdbootstrap-pro/js/mdb';
import 'mdbootstrap-pro/js/addons-pro/steppers';
import "mdbootstrap-pro/scss/addons-pro/_steppers.scss";

//Add a namspace
window.MFP = {};


var Routes = {
	Home: {
		init: function () {
			// controller-wide code
		},
		Privacy: function () {
			// Privacy action code
		}
	},
};


var Router = {
	exec: function (controller, action) {
		action = action === undefined ? "init" : action;

		if (controller !== "" && Routes[controller] && typeof Routes[controller][action] === "function") {
			Routes[controller][action]();
		}
	},

	init: function () {
		let body = document.body;
		let controller = body.getAttribute("data-controller");
		let action = body.getAttribute("data-action");

		Router.exec(controller);
		Router.exec(controller, action);

	}
};

$(function () {
	//run this when the DOM is ready
	Router.init();
	$(".mdb-select").materialSelect();
	$('.datepicker').pickadate({
		format: 'dd/mm/yyyy',
		formatSubmit: 'yyyy-mm-dd',
		hiddenName: true
	});

	$('.stepper').mdbStepper();
	$(".button-collapse").sideNav();

	// loading animation for save button
	$("#btnSubmit").click(function (e) {
		var btn = $(this);
		var frm = $("form");
		e.preventDefault();
		if (frm.valid()) {
			// disable button
			btn.prop("disabled", true);
			// add spinner to button
			btn.html(
				`<span class="spinner-border spinner-border-sm mr-2" role="status" aria-hidden="true"></span>Processing`
			);
			frm.submit();
		}
	});

	$(".clear-search").click(function () {
		$("input").val("");
		$("input[type='text']").val("");
		$("input[type='number']").val("");
		$("input[type='checkbox']").prop('checked', false);
		$("select").each(function () { this.selectedIndex = 0 });
		Search();
	});



});



