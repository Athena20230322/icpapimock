(function () {
	// libs.check
	window.libs = window.libs || {};
	libs.check = {
		checkDate: function(dateStr) {
			if (!dateStr) return false;
			var separator;
			if (dateStr.indexOf('/') != -1)
				separator = '/';
			else if (dateStr.indexOf('-') != -1)
				separator = '-';
			else if (dateStr.indexOf('.') != -1)
				separator = '.';
			
			var parts = dateStr.split(separator);
			var day = parseInt(parts[2], 10);
			var month = parseInt(parts[1], 10);
			var year = parseInt(parts[0], 10);

			if (year < 1900 || year > 2910 || month == 0 || month > 12 || day == 0)
				return false;

			var monthLength = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

			if (year % 400 == 0 || (year % 100 != 0 && year % 4 == 0))
				monthLength[1] = 29;

			return day <= monthLength[month - 1];
		}
	};
})();