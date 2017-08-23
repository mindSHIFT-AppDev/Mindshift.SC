
app.controller('AdoLoggingController', ['$scope', '$http', '$cookies', '$timeout', '$rootScope', '$location', '$routeParams', '$modal', function ($scope, $http, $cookies, $timeout, $rootScope, $location, $routeParams, $modal) {
	$('.DialogHeader').html("Log Entry Detail");

	// some notes for tomorrow:
	// TODO: cookies
	// TODO: edit
	// TODO: buttons to re-order, move up (drag+drop?)
	// - e.g. up, down, to parent (or maybe a good old sitecore "Move" or "Duplicate", etc
	// - TODO: on hover only!
	// Nice to have: preview?

	// TODO: click on data source to show item edit... like the one in the experience editor? but better?

	// TODO: what to add to gain access to these?
	//javascript:Sitecore.PageModes.PageEditor.postRequest('webedit:fieldeditor(referenceId={0A73E269-C3FD-4A07-B3E5-C9C493988DB4},command={70C4EED5-D4CD-4D7D-9763-80C42504F5E7},fields={88622EDE-E93C-4DA6-B008-B64FF0366226}|{F93E13EE-E360-4FEC-9BC6-DE0E80B5DAF5},renderingId={71F05C92-D11F-4E00-B0D4-E05CD0545BB3},id={DE319DD2-6CE9-4797-BADA-5BBCCCF65F15})',null,false)



	//var saveState = function () {
	//	$.jStorage.set('dynamiclayout-stateCookie', JSON.stringify($scope.state));
	//}

	//var stateCookie = $.jStorage.get('dynamiclayout-stateCookie', null);

	//if (stateCookie) {
	//	$scope.state = JSON.parse(stateCookie);
	//} else {
	//	$scope.state = {
	//		currentLayoutType: 'shared',
	//		openState: {},
	//		editState: {},
	//		addRenderingState: {}
	//	}
	//	saveState();
	//}


	//javascript:Sitecore.PageModes.PageEditor.postRequest('webedit:componentoptions(referenceId={0A73E269-C3FD-4A07-B3E5-C9C493988DB4},renderingId={71F05C92-D11F-4E00-B0D4-E05CD0545BB3},id={DE319DD2-6CE9-4797-BADA-5BBCCCF65F15})',null,false)
	// objects
	$scope.view = {
		loading: true,
		tab: $routeParams.tab,
		id: getQueryStringParameterByName('id'),
		data: {}
	}; // TODO: find playerId some other way...

	//if ($scope.view.database.endsWith("/")) $scope.view.database = $scope.view.database.slice(0, -1);

	var Load = function (retFunc) {
		//'/dynamicPlaceholderAPI/Dynamic/GetRenderings/' + id + '/' + database
		if ($scope.view.data.List == null) {
			var webMethod = '/GetLogEntryDetail/' + $scope.view.id; // TODO: different path?
			$scope.GetObject({
				url: webMethod,
				success: function (data) {
					retFunc(data);
				},
				error: function (data, status, headers, config) {
					//ClearMap();
					//$scope.CurrentUser = null;
				}
			});
		}
	}

	// init:
	Load(function (item) {
		// TODO: add width heights to these... like boxes...
		//for (var i = modules.length - 1; i < 20; i++) {
		//    modules.push({ cssClass: 'inactive' });
		//}

		// TODO: parse items somehow? It is XML not an object... 
		$scope.view.data = item; // note: we could make a custom object for modules on in TypeScript - but aren't at this point.
		$scope.view.loading = false;


	});


	$scope.openItem = function (id) {
		//window.top.scForm.postRequest('', '', '', 'item:load(id=' + rendering.DataSourceId + ')');
		// TODO: just open in new tab?
		//window.top.Sitecore.PageModes.PageEditor.postRequest('webedit:setdatasource(referenceId={3598EB23-B54C-4968-A8E5-649F83AA4DD4},renderingId={45106690-824D-4C11-9AEA-0F57693E9653},id={3D04D39E-74FD-4C0D-867E-AEC6541174B6})', null, false)
		// TODO: open in lightbox instead?
		window.open('/sitecore/shell/Applications/Content%20Manager/default.aspx?fo=' + id);
	}

}]);



//jQuery(function ($) {
//	// TODO: pass objects and stuff - better than a querystring, etc.
//	var id = getParameterByName('id');
//	var database = getParameterByName('database');

//	$.get('/dynamicPlaceholderAPI/Dynamic/GetRenderings/' + id + '/' + database, function (data) {
//		console.log(data);
//	});


//});
