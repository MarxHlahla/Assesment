$(function () {
	getDefault();
	$(".btnLi").click(function () {
		let scenario = $(this).attr("data-sc");
		WritetoFile(scenario);
	});
	$(".rdbtn").change(function () {
		$(".dynView").hide();
		$(".rdbtn").prop("checked", false)
		$(this).prop("checked", true);
		let $div = $(this).attr("data-scenario");
		getMethod($div);
	});
});
let WritetoFile = function (scenario) {
	$.getJSON("../Home/WriteToFile?Scenario="+scenario,
		function (data) {
			if (data.Success) {
				$("#msgContainer").addClass("success_Panel");
				$("#txtMsg").text(data.Message);
				setTimeout(function () {
					$("#msgContainer").removeClass("success_Panel");
					$("#txtMsg").text("");
				}, 5000);
			} else {
				$("#msgContainer").addClass("error_Panel");
				$("#txtMsg").text(data.Error);
				setTimeout(function () {
					$("#msgContainer").removeClass("error_Panel");
					$("#txtMsg").text("");
				}, 5000);
			}
			
		})
}
let getMethod = function (name) {
	if (name == "Default")
	{
		getDefault();
	}
	else if (name == "Scenario1")
	{
		getAddress();
	}
	else if (name == "Scenario2")
	{
		getFreq();
	}
}
let getDefault = function () {

$.getJSON("../Home/GetLoadData",
		function (data) {
			$("#Default").show();
			$(".tr").remove();
		$(data).each(function (index) {
			let firstName = data[index].Firstname;
			let LastName = data[index].Lastname;
			let address = data[index].Address.StreetNumber + " " + data[index].Address.StreetName;
			let phoneNumber = data[index].PhoneNumber
			$("#tableBody").append("<tr class='tr'><td>"+firstName+"</td><td>"+LastName+"</td><td>"+address+"</td><td>"+phoneNumber+"</td</tr>");
		});
		});
}
let getAddress = function () {
	$.getJSON("../Home/GetAddress",
			function (data) {
				$("#Scenario1").show();
				$(".tr").remove();
				$(data).each(function (index) {
					let address = data[index].StreetNumber + " " + data[index].StreetName;
					$("#tableBodyS1").append("<tr class='tr'><td>" + address + "</td></tr>");
				});
			});
}
let getFreq = function(){
	$.getJSON("../Home/GetFreq",
			function (data) {
				$("#Scenario2").show();
				$(".tr").remove();
				let kpvName = JSON.stringify(data.FrequentNames).replace(/[`~!@#$%^&*()|+\?'".<>\{\}\[\]\\\/]/gi, '').split(',');
				$(kpvName).each(function (index) {
					
					let kpv = kpvName[index].split(":");
					let name = kpv[0];
					let count = kpv[1];
					$("#tableBodyS2a").append("<tr class='tr'><td>" + name + "</td><td>" + count + "</td></tr>");
				});
				let kpvNameLast = JSON.stringify(data.FrequentNameSurFreq).replace(/[`~!@#$%^&*()|+\?'".<>\{\}\[\]\\\/]/gi, '').split(',');
				$(kpvNameLast).each(function (index) {
					let kpv = kpvNameLast[index].split(":");
					let name = kpv[0];
					let count = kpv[1];
					$("#tableBodyS2b").append("<tr class='tr'><td>" + name + "</td><td>" + count + "</td></tr>");
				});
			});
}
