app.controller('DynamicPlaceholdersController', ['$scope', '$http', '$cookies', '$timeout', '$rootScope', '$location', '$routeParams', '$modal', function ($scope, $http, $cookies, $timeout, $rootScope, $location, $routeParams, $modal) {
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



	var saveState = function () {
		$.jStorage.set('dynamiclayout-stateCookie', JSON.stringify($scope.state));
	}

	var stateCookie = $.jStorage.get('dynamiclayout-stateCookie', null);

	if (stateCookie) {
		$scope.state = JSON.parse(stateCookie);
	} else {
		$scope.state = {
			currentLayoutType: 'shared',
			openState: {},
			editState: {},
			addRenderingState: {}
		}
		saveState();
	}


	//javascript:Sitecore.PageModes.PageEditor.postRequest('webedit:componentoptions(referenceId={0A73E269-C3FD-4A07-B3E5-C9C493988DB4},renderingId={71F05C92-D11F-4E00-B0D4-E05CD0545BB3},id={DE319DD2-6CE9-4797-BADA-5BBCCCF65F15})',null,false)
	// objects
	$scope.view = {
		tab: $routeParams.tab,
		currentLayoutType: null,
		itemId: getQueryStringParameterByName('itemid'),
		database: getQueryStringParameterByName('database'),
		data: {
			LayoutTypes: null
		}
	}; // TODO: find playerId some other way...

	var Load = function (retFunc) {
		//'/dynamicPlaceholderAPI/Dynamic/GetRenderings/' + id + '/' + database
		if ($scope.view.data.List == null) {
			var webMethod = '/DynamicPlaceholders/GetRenderings/' + $scope.view.itemId + '/' + $scope.view.database; // TODO: different path?
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
	Load(function (items) {
		// TODO: add width heights to these... like boxes...
		//for (var i = modules.length - 1; i < 20; i++) {
		//    modules.push({ cssClass: 'inactive' });
		//}

		// TODO: parse items somehow? It is XML not an object... 
		$scope.view.data = items; // note: we could make a custom object for modules on in TypeScript - but aren't at this point.

		var layoutTypeMatch = $scope.view.data.LayoutTypes.filter(function (layout) {
			return layout.Name == $scope.state.currentLayoutType;
		});

		if (layoutTypeMatch && layoutTypeMatch.length > 0) {
			$scope.view.currentLayoutType = layoutTypeMatch[0];
		} else {
			$scope.view.currentLayoutType = $scope.view.data.LayoutTypes[0];
			$scope.setCurrentLayoutType($scope.view.currentLayoutType);
		}

		$scope.view.flatRenderingList = [];
		$scope.view.flatPlaceholderList = [];

		// TODO: flat object list
		for (var i = 0; i < $scope.view.data.LayoutTypes.length; i++) {
			for (var j = 0; j < $scope.view.data.LayoutTypes[i].Devices.length; j++) {
				for (var k = 0; k < $scope.view.data.LayoutTypes[i].Devices[j].Placeholders.length; k++) {
					$scope.view.flatPlaceholderList = $scope.view.flatPlaceholderList.concat($scope.view.data.LayoutTypes[i].Devices[j].Placeholders[k]);
					getFlatRenderingList($scope.view.data.LayoutTypes[i].Devices[j].Placeholders[k].Renderings, $scope.view.flatRenderingList);
				}
			}
		}

	});

	var getFlatRenderingList = function (items, ret) {
		//ret = ret.concat(items);
		$scope.view.flatRenderingList = $scope.view.flatRenderingList.concat(items);
		for (var i = 0; i < items.length; i++) {
			$scope.view.flatPlaceholderList = $scope.view.flatPlaceholderList.concat(items[i].Placeholders);
			for (var j = 0; j < items[i].Placeholders.length; j++) {
				getFlatRenderingList(items[i].Placeholders[j].Renderings, ret);
			}
		}
	}

	$scope.setCurrentLayoutType = function (layoutType) {
		$scope.view.currentLayoutType = layoutType;
		$scope.state.currentLayoutType = layoutType.Name;
		saveState();
	}

	$scope.setOpenState = function (rendering) {
		var id = rendering.UniqueId ? rendering.UniqueId : rendering.Id;
		if (!$scope.state.openState[id]) $scope.state.openState[id] = false;
		$scope.state.openState[id] = !$scope.state.openState[id];
		saveState();
	}

	$scope.getOpenState = function (rendering) {
		var id = rendering.UniqueId ? rendering.UniqueId : rendering.Id;
		//if (!$scope.state.openState) $scope.state.openState = {}; // shouldn't ever be needed - just needed it because I added it later
		return $scope.state.openState[id]; // note: null or undefined is false so...
	}


	$scope.setEditState = function (obj) {
		var id = obj.UniqueId ? obj.UniqueId : obj.Id;
		if (!$scope.state.editState[id]) $scope.state.editState[id] = false;
		$scope.state.editState[id] = !$scope.state.editState[id];
		saveState();
	}

	$scope.getEditState = function (obj) {
		var id = obj.UniqueId ? obj.UniqueId : obj.Id;
		//if (!$scope.state.openState) $scope.state.openState = {}; // shouldn't ever be needed - just needed it because I added it later
		return $scope.state.editState[id]; // note: null or undefined is false so...
	}


	$scope.setMoveState = function (rendering, placeholder) {
		if ($scope.view.newPositionRendering && $scope.view.newPositionRendering.UniqueId == rendering.UniqueId) {
			$scope.view.newPositionRendering = null;
			$scope.view.newPositionOldPlaceholder = null;
		} else {
			$scope.view.newPositionRendering = rendering;
			$scope.view.newPositionOldPlaceholder = placeholder;
		}
	}


	$scope.getAddRenderingState = function (device, obj, placeholder) {
		var id = device.Name;
		if (obj) {
			id += '-' + obj.UniqueId ? obj.UniqueId : obj.Id;
		}
		id += '~' + placeholder.Name;
		//if (!$scope.state.openState) $scope.state.openState = {}; // shouldn't ever be needed - just needed it because I added it later
		return $scope.state.addRenderingState[id]; // note: null or undefined is false so...

	}


	$scope.setAddRenderingState = function (device, obj, placeholder) {
		var id = device.Name;
		if (obj) {
			id += '-' + obj.UniqueId ? obj.UniqueId : obj.Id;
		}
		id += '~' + placeholder.Name;
		if (!$scope.state.addRenderingState[id]) $scope.state.addRenderingState[id] = false;
		$scope.state.addRenderingState[id] = !$scope.state.addRenderingState[id];
		saveState();
	}


	$scope.moveItemBetween = function (obj) {
		// obj is the rendering above where you want... what about the parent? hmm
		// TODO: move $scope.view.newPositionLayout to where obj is
	}

	$scope.moveItemOrder = function (rendering, placeholder, direction) {
		// TODO: move it up in the list it's in.
		// the renderings index it in the placeholder
		var index = placeholder.Renderings.findIndex(function (x) {
			return x.UniqueId == rendering.UniqueId;
		});

		var newPos = index - direction;

		if (index == -1) return;
		if (newPos >= this.length) newPos = placeholder.Renderings.length;
		if (newPos < 0) newPos = 0;

		placeholder.Renderings.splice(index, 1);
		placeholder.Renderings.splice(newPos, 0, rendering);

		// TODO: should I make a note of the move?

	}


	$scope.moveItemUnder = function (placeholder, rendering) {
		var oldPlaceholder = null;
		if (!rendering) {
			if (!$scope.view.newPositionRendering) return; // nothing selected to move.
			rendering = $scope.view.newPositionRendering;
			oldPlaceholder = $scope.view.newPositionOldPlaceholder;
		}

		// obj is the rendering above where you want... what about the parent? hmm
		// TODO: move $scope.view.newPositionLayout to where obj is

		placeholder.Renderings.push(rendering);
		// TODO: we are finding the placeholder now, not the parent rendering
		//if (placeholder.ParentUniqueId) { // only need to remove from the old one if there was an old one (does this still track - meh for top level? Does the layout have a UniqueId?)

		// TODO: what if there was no ParentUniqueId!
		//	$scope.view.flatPlaceholderList.find(function (x) {
		//	return x.Name == rendering.PlaceholderName && (rendering.ParentUniqueId==null || x.ParentUniqueId == rendering.ParentUniqueId);
		//});

		//if (!oldPlaceholder) { // if it wasn't found by name and id, just try name and no id
		//	oldPlaceholder = $scope.view.flatPlaceholderList.find(function (x) {
		//		return x.Name == rendering.PlaceholderName && x.ParentUniqueId == rendering.ParentUniqueId;
		//	});


		//}

		//var oldParentRendering = $scope.view.flagRenderingList.find(function (x) {
		//	return x.UniqueId == rendering.ParentUniqueId;
		//});

		//var index = parentRendering.Renderings.map(function (x) { return x.id; }).indexOf($scope.view.newPositionRendering.UniqueId);
		if (oldPlaceholder) { // if it's not new!
			var index = oldPlaceholder.Renderings.findIndex(function (x) {
				return x.UniqueId == rendering.UniqueId;
			});
			oldPlaceholder.Renderings.splice(index, 1);

			// TODO: build the new path (or not needed?)
			rendering.PlaceholderName = placeholder.Name;

			var hasParentUniqueId = (rendering.ParentUniqueId != null);


			//rendering.PlaceholderPath = rendering.PlaceholderPath.replace(oldPlaceholder.Name.replace(/-/g, ''), placeholder.Name.replace(/-/g, ''));
			//rendering.PlaceholderPath = rendering.PlaceholderPath.replace(rendering.ParentUniqueId.replace(/-/g, ''), placeholder.ParentUniqueId.replace(/-/g, ''));

			// TODO: this needs to be better - it should really build the path - or just reload... or something
			// if it doesn't end in a GUID - basically, it needs a GUID added instead.
			if (hasParentUniqueId) {
				rendering.PlaceholderPath = rendering.PlaceholderPath.replace(oldPlaceholder.Name + "~" + rendering.ParentUniqueId.replace(/-/g, ''), placeholder.Name + "~" + placeholder.ParentUniqueId.replace(/-/g, ''));
			} else { // TODO: Fix this!
				rendering.PlaceholderPath = placeholder.Name + '~' + placeholder.ParentUniqueId.replace(/-/g, '');
			}

		} else { // TODO: Fix this!
			rendering.PlaceholderPath = placeholder.Name + '~' + placeholder.ParentUniqueId.replace(/-/g, '');
		}

		rendering.ParentUniqueId = placeholder.ParentUniqueId; // this is what's important for the "Save"


		$scope.view.newPositionRendering = null;
		$scope.view.newPositionOldPlaceholder = null;
	}

	$scope.saveChanges = function () {
		var webMethod = '/DynamicPlaceholders/SaveRenderings/' + $scope.view.itemId + '/' + $scope.view.database; // TODO: different path?
		$scope.PostObject({
			url: webMethod,
			obj: $scope.view.data,
			success: function (data) {
				window.top.dialogClose();
			},
			error: function (data, status, headers, config) {
			}
		});


	}

	$scope.cancelChanges = function () {
		window.top.dialogClose();

	}

	$scope.editDatasource = function (rendering) {
		//window.top.scForm.postRequest('', '', '', 'item:load(id=' + rendering.DataSourceId + ')');
		// TODO: just open in new tab?
		//window.top.Sitecore.PageModes.PageEditor.postRequest('webedit:setdatasource(referenceId={3598EB23-B54C-4968-A8E5-649F83AA4DD4},renderingId={45106690-824D-4C11-9AEA-0F57693E9653},id={3D04D39E-74FD-4C0D-867E-AEC6541174B6})', null, false)
		// TODO: open in lightbox instead?
		window.open('/sitecore/shell/Applications/Content%20Editor.aspx?fo=' + rendering.DataSourceId);
	}

	$scope.getChangeDatasourceState = function (obj) {
		var id = (obj.UniqueId ? obj.UniqueId : obj.Id) + '-changeDataSource';
		return $scope.state.editState[id]; // note: null or undefined is false so...
	}

	$scope.setChangeDatasourceState = function (obj) {
		var id = (obj.UniqueId ? obj.UniqueId : obj.Id) + '-changeDataSource';
		if (!$scope.state.editState[id]) $scope.state.editState[id] = false;
		$scope.state.editState[id] = !$scope.state.editState[id];
		saveState();
	}

	$scope.getChangeDatasourceIframeSource = function (rendering) {
		return decodeURIComponent(rendering.DatasourceLocation);


		// sitecore://master/{C28482C3-DBFC-41BD-B2DA-C0693C471E2C}?lang=en&ver=1|sitecore://master/{87FA6B8D-0E11-4810-A007-5156BC625A72}?lang=en&ver=1|sitecore://master/{C59A3D62-0A40-4586-A862-4BF5DC13B86D}?lang=en&ver=1
		//return ('/sitecore/shell/default.aspx?xmlcontrol=Sitecore.Shell.Applications.Dialogs.SelectRenderingDatasource' +
		//	'&hdl=FAC0E8645D824E5E9C541DB55CCA9D3D' +
		//	'&ro=sitecore%3A%2F%2Fmaster%2F%7BC28482C3-DBFC-41BD-B2DA-C0693C471E2C%7D%3Flang%3Den%26ver%3D1' +
		//	'&fo=sitecore%3A%2F%2Fmaster%2F' + rendering.ItemId + '%3Flang%3Den%26ver%3D1' +
		//	'&sr=1' +
		//	'&ic=BusinessV2%2F32x32%2Fcabinet_open.png' +
		//	'&txt=Select%20the%20content%20that%20you%20want%20to%20associate%20with%20the%20rendering%20and%20use%20as%20the%20data%20source.' +
		//	'&ti=Select%20the%20Associated%20Content' +
		//	'&bt=OK' +
		//	'&rt=Id' +
		//	'&cDS=%2Fsitecore%2Fcontent%2FRicoh%2FSites%2FUSA%2FSite%20Data%2FPage%20Content%2FSite%20Tangos' +
		//	'&dsDN=Tango%20Across' +
		//	'&dsRoots=' + encodeURIComponent(rendering.DatasourceLocation) + //			'&dsRoots=sitecore%3A%2F%2Fmaster%2F%7BC28482C3-DBFC-41BD-B2DA-C0693C471E2C%7D%3Flang%3Den%26ver%3D1%7Csitecore%3A%2F%2Fmaster%2F%7B87FA6B8D-0E11-4810-A007-5156BC625A72%7D%3Flang%3Den%26ver%3D1%7Csitecore%3A%2F%2Fmaster%2F%7BC59A3D62-0A40-4586-A862-4BF5DC13B86D%7D%3Flang%3Den%26ver%3D1' +
		//	'&clang=en');
	}


	$scope.changeDatasource = function (rendering, $event) {
		//window.top.scForm.postRequest('', '', '', 'item:load(id=' + rendering.DataSourceId + ')');
		// TODO: just open in new tab?
		//window.top.Sitecore.PageModes.PageEditor.postRequest('webedit:setdatasource(referenceId={3598EB23-B54C-4968-A8E5-649F83AA4DD4},renderingId={45106690-824D-4C11-9AEA-0F57693E9653},id={3D04D39E-74FD-4C0D-867E-AEC6541174B6})', null, false)
		// TODO: open in lightbox instead?

		// referenceId = UniqueId, renderingId = ItemId, id=DataSourceId
		//		window.top.Sitecore.PageModes.PageEditor.postRequest('webedit:setdatasource(referenceId=' + rendering.UniqueId + ',renderingId=' + rendering.ItemId + ',id=' + rendering.DataSourceId + ')', null, false)
		//window.open('/sitecore/shell/default.aspx?xmlcontrol=Sitecore.Shell.Applications.Dialogs.SelectRenderingDatasource&hdl=FAC0E8645D824E5E9C541DB55CCA9D3D&ro=sitecore%3A%2F%2Fmaster%2F%7BC28482C3-DBFC-41BD-B2DA-C0693C471E2C%7D%3Flang%3Den%26ver%3D1&fo=sitecore%3A%2F%2Fmaster%2F' + rendering.ItemId + '%3Flang%3Den%26ver%3D1&sr=1&ic=BusinessV2%2F32x32%2Fcabinet_open.png&txt=Select%20the%20content%20that%20you%20want%20to%20associate%20with%20the%20rendering%20and%20use%20as%20the%20data%20source.&ti=Select%20the%20Associated%20Content&bt=OK&rt=Id&cDS=%2Fsitecore%2Fcontent%2FRicoh%2FSites%2FUSA%2FSite%20Data%2FPage%20Content%2FSite%20Tangos&dsDN=Tango%20Across&dsRoots=sitecore%3A%2F%2Fmaster%2F%7BC28482C3-DBFC-41BD-B2DA-C0693C471E2C%7D%3Flang%3Den%26ver%3D1%7Csitecore%3A%2F%2Fmaster%2F%7B87FA6B8D-0E11-4810-A007-5156BC625A72%7D%3Flang%3Den%26ver%3D1%7Csitecore%3A%2F%2Fmaster%2F%7BC59A3D62-0A40-4586-A862-4BF5DC13B86D%7D%3Flang%3Den%26ver%3D1&clang=en');
		// C28482C3-DBFC-41BD-B2DA-C0693C471E2C = the Site folder? huh?

		// TODO: maybe something on window.top?

		//javascript:return scForm.invoke('mindshift:setdynamiclayoutdetails', event)

		//javascript:return scForm.postEvent(this,event,'contentrenderingdatasource:browse(id=FIELD75739124)')
		//console.log($event);
		//window.top.scForm.postEvent($event.currentTarget, $event, 'contentrenderingdatasource:browse(id=FIELD77739897)');
		//window.top.scForm.postRequest("", "", "", 'contentrenderingdatasource:browse(id=FIELD77739897)');
		return scForm.postEvent(this, $event, 'contentrenderingdatasource:browse(id=FIELD77739897)');


		//		window.open('/sitecore/shell/Applications/Content%20Editor.aspx?fo=' + rendering.DataSourceId);

		//javascript:Sitecore.PageModes.PageEditor.postRequest('webedit:setdatasource(referenceId={86F7DB07-9C19-4041-AA6B-B6377F68ADEF},renderingId={BDB534C9-025B-4278-8A13-6893B149D13C},id={64AC9051-9078-4D4B-BA55-5822F4E8E690})',null,false)
	}


	//javascript:Sitecore.PageModes.PageEditor.postRequest('webedit:setdatasource(referenceId={86F7DB07-9C19-4041-AA6B-B6377F68ADEF},renderingId={BDB534C9-025B-4278-8A13-6893B149D13C},id={64AC9051-9078-4D4B-BA55-5822F4E8E690})',null,false)
	//<img src="/temp/IconCache/Office/16x16/data.png" alt="Associate a content item with this component.">


	$scope.addRendering = function (device, placeholder, parentRendering, placeholderRendering) {

		//public string PlaceholderName { get; private set; }
		//public string PlaceholderPath { get; private set; }

		//public string DataSourceId { get; private set; }
		//public string DataSourcePath { get; private set; }

		//public string Icon { get; set; }

		//public string Error { get; set; }

		//public string DatasourceLocation { get; private set; }

		// TODO: get placeholders for this one... just like always!
		var rendering = $.extend(true, { PlaceholderName: placeholder.Name }, placeholderRendering); // TODO: make {} the new things I need!
		$scope.moveItemUnder(placeholder, rendering);

		$scope.setAddRenderingState(device, parentRendering, placeholder);

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
