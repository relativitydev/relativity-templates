define(function () {
	"use strict";
	function sampleHandle(api) {
		var api = api;
		// FORMATTING TEXT EXAMPLE
		function subjectFormatter(value, options, rowObject, formatterApi) {
			var data, resultHtml;
			data = formatterApi.getCellData(value) || "";
			resultHtml = "<span style='color: #0b3e6f'>File type: " + data + "</span><hr><span>Another Line</span>";
			return resultHtml;
		}
		// OPEN MODAL EXAMPLE
		function authorFormatter(value, options, rowObject, formatterApi) {
			var valueText, resultHtml, modalId, modalContent;
			valueText = formatterApi.getCellData(value) || "";
			modalContent = {
				title: "Modal for " + valueText,
				modalClass: "rlh-new-rdo-dialog",
				modalName: "rlhRdoModalDialog",
				template: "<span>Some sample text</span>",
				buttons: [{
					name: "Apply",
					eventName: "apply_click",
				}, {
					name: "Close",
					eventName: "close_click"
				}],
				init: function (scope, el) {
					scope.$on("apply_click", function () {
						alert("OK clicked - " + valueText);
						api.modalService.hideModal(modalId);
					});
					scope.$on("close_click", function () {
						api.modalService.hideModal(modalId);
					});
				}
			}
			var modalId = api.modalService.createModal(modalContent);
			resultHtml = valueText + "<br><a href='#' " + formatterApi.getOnClickForModal(modalId) + ">Open modal (formatterApi)</a>";
			return resultHtml;
		}
		function cellFormattersInit(formatterApi) {
			var fieldSubject, fieldAuthor;
			fieldSubject = formatterApi.fields.find(function (field) {
				return field.displayName === "File Type";
			});
			if (fieldSubject) {
				formatterApi.setFormatter(fieldSubject.columnName, subjectFormatter);
			}
			fieldAuthor = formatterApi.fields.find(function (field) {
				return field.displayName === "File Path";
			});
			if (fieldAuthor) {
				formatterApi.setFormatter(fieldAuthor.columnName, authorFormatter);
			}
		}
		return {
			cellFormattersInit: cellFormattersInit
		};
	}
	return sampleHandle;
});