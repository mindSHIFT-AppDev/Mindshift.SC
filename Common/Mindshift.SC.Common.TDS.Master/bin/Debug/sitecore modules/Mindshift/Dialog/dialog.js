// TODO: read from service call!
// TODO: put that call somewhere!
// TODO: which jquery?

function getQueryStringParameterByName(name, url) {
	if (!url) url = window.location.href;
	name = name.replace(/[\[\]]/g, "\\$&");
	var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
			results = regex.exec(url);
	if (!results) return null;
	if (!results[2]) return '';
	return decodeURIComponent(results[2].replace(/\+/g, " "));
}

if (!String.prototype.endsWith) {
	String.prototype.endsWith = function (searchString, position) {
		var subjectString = this.toString();
		if (typeof position !== 'number' || !isFinite(position) || Math.floor(position) !== position || position > subjectString.length) {
			position = subjectString.length;
		}
		position -= searchString.length;
		var lastIndex = subjectString.indexOf(searchString, position);
		return lastIndex !== -1 && lastIndex === position;
	};
}

var app = angular.module('DialogApp', ['ui.router', 'oc.lazyLoad', 'ngCookies', 'ui.bootstrap', 'ngRoute', 'ngResource', 'ngSanitize']);

app.config(function ($stateProvider, $locationProvider, $urlRouterProvider, $ocLazyLoadProvider, $controllerProvider) {
	//$locationProvider.html5mode(true);
	app._controller = app.controller
	app.controller = function (name, constructor){
		$controllerProvider.register(name, constructor);
		return (this);
	}


	$stateProvider.state("dialog", {
		url: "/dialog/:name",
		views: {
			"viewDialog": {
				templateUrl: function ($stateParams) {
					return 'html/' + $stateParams.name + '.html';
				}
			}
		},
		resolve: {
			loadMyCtrl: function ($stateParams, $ocLazyLoad) {
				return $ocLazyLoad.load({
					files: ['js/' + $stateParams.name + '.js', 'css/' + $stateParams.name + '.css']
				});
			}
		}
	});


	//$urlRouterProvider.otherwise('/test');

	//$routeProvider.
	//	when('/dialog/:name*', {
	//		templateUrl: function (urlattr) {
	//			return '/html/' + urlattr.name + '.html';
	//		},
	//		controller: function (urlattr) {
	//			return urlattr.name + 'Controller';
	//		},
	//		resolve: {
	//			loadMyCtrl: ['$ocLazyLoad', '$stateParam', function ($ocLazyLoad, $stateParam) {
	//				return $ocLazyLoad.load('js/' + $stateParam.name + '.js').load('css/' + $stateParam.name + '.css');
	//			}]
	//		}
	//	}).otherwise({
	//		redirectTo: '/dialog'
	//	});

});
app.run(function ($rootScope, $location, $cookies, $http, $timeout, $cookieStore) {
	$rootScope.ServiceURL = '/mindshiftAPI'; // TODO: update this!

	$rootScope.GetObject = function (args) {
		$rootScope.error = null;
		var opts = angular.extend({
			url: '',
			success: function () {
			},
			error: function () {
			}
		}, args);
		//var header = { 'X-Security-Token': $rootScope.CurrentUser.SecurityToken };
		var webMethod = $rootScope.ServiceURL + opts.url;
		var payload = {
			List: opts.obj
		};
		var req = {
			method: 'GET',
			url: webMethod
		};
		$http(req).success(args.success).error(function (data, status, headers, config) {
			console.log(data);
			//processError(data, status, headers, config, args.error);
		});
	};

	$rootScope.PostObject = function (args) {
		//if ($rootScope.CheckLogin()) {
		var opts = angular.extend({
			url: '', obj: {}, success: function () {
			}, error: function () { }
		}, args);
		var webMethod = $rootScope.ServiceURL + opts.url; ///' + building.PlotPlayerBuildingID + '
		//var payload = { SecurityToken: $scope.CurrentUser.SecurityToken, Buildings: [building] };
		var payload = opts.obj; // TODO: was Data - when we need more meta properties this made sense (like paging)
		var req = {
			method: 'POST', url: webMethod, data: payload, withCredentials: true
		}
		$http(req).success(args.success).error(function (data, status, headers, config) { processError(data, status, headers, config, args.error) });
	}

	$rootScope.formatDateTime = function (dateString) {
		return moment(new Date(dateString)).format('YYYY-MM-DD H:mm:ss');
	}

});
