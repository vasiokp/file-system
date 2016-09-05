angular.module('fileSystem', []).controller('mainCtrl', function ($scope, $http, $q) {
	var canceller = null;
	$scope.loaderState = "display:inline;";
	$scope.textState = "display:none;";
	$scope.browse = function (currentPath, pathToBrowse) {
		if (canceller) {
			canceller.resolve();
			canceller = null;
		}
		canceller = $q.defer();
		$http({
			method: 'POST',
			url: 'Home/Browse',
			data: {
				currentPath: currentPath,
				pathToBrowse: pathToBrowse
			}
		}).then(function (response) {
			$scope.directory = response.data;
		});
		$scope.loaderState = "display:inline;";
		$scope.textState = "display:none;";
		$http({
			method: 'POST',
			url: 'Home/GetCounts',
			data: {
				currentPath: currentPath,
				pathToBrowse: pathToBrowse
			},
			timeout: canceller.promise
		}).then(function (response) {
			$scope.counts = response.data;
			$scope.loaderState = "display:none;";
			$scope.textState = "display:inline;";
		});
	};
	$scope.setId = function (path) {
		return (path == 'This PC') ? 'not-active' : 'active';
	};
	$scope.browse('', 'This PC');
});