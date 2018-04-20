$(document).ready(function() {
	var featureImage = $('.featureImage').waypoint(function() {
	  $('.featureImage').addClass('showUp');
	})

	var leftRightFade = $('.heading-left').waypoint(function() {
	  $('.heading-left').addClass('showUp');
	  $('.heading-right').addClass('showUp');
	}, {
		offset: '75%'
	})

	var description = $('.description').waypoint(function() {
	  $('.description').addClass('showUp');
	}, {
		offset: '75%'
	})

	var fadeHeadingFirst = $('.first-heading').waypoint(function() {
	  $('.first-heading').addClass('showUp');
	}, {
		offset: '75%'
	})

	var fadeHeadingSecond = $('.second-heading').waypoint(function() {
	  $('.second-heading').addClass('showUp');
	}, {
		offset: '75%'
	})

	var fadeHeadingThird = $('.third-heading').waypoint(function() {
	  $('.third-heading').addClass('showUp');
	}, {
		offset: '75%'
	})

	function readURL(input) {
	    if (input.files && input.files[0]) {
	        var reader = new FileReader();

	        reader.onload = function (e) {
	            $('#uploadMsg').addClass('hidden');
	            $('#imagePreview').attr('src', e.target.result).removeClass("hidden");
	            $("#adNameInput").removeClass("hidden");
	        }
	        reader.readAsDataURL(input.files[0]);
	    }
	}

	var _validFileExtensions = [".jpg", ".jpeg", ".bmp", ".gif", ".png"];
	$("#uploadedFile").change(function () {
	    if (this.type == "file") {
	        var sFileName = this.value;
	        if (sFileName.length > 0) {
	            var blnValid = false;
	            for (var j = 0; j < _validFileExtensions.length; j++) {
	                var sCurExtension = _validFileExtensions[j];
	                if (sFileName.substr(sFileName.length - sCurExtension.length, sCurExtension.length).toLowerCase() == sCurExtension.toLowerCase()) {
	                    blnValid = true;
	                    readURL(this);
	                    $('#uploadBtn').prop('disabled', false);
	                    break;
	                }
	            }

	            if (!blnValid) {
	                $('#imagePreview').addClass("hidden");
	                $('#uploadBtn').prop('disabled', true);
	                $('#uploadMsg').text("Sorry, " + sFileName.substring(12) + " is invalid.").removeClass('hidden');
	                this.value = "";
	                return false;
	            }
	        }
	    }
	    return true;
	});

	if ($('#CountryDropDown').val() == 'Canada') {
	    $('#ProvinceInput').removeClass('hidden');
	}
	else {
	    $('#ProvinceInput').addClass('hidden');
	    $('#ProvinceDropDown').val('');
	}

	$("#CountryDropDown").change(function () {
	    if ($('#CountryDropDown').val() == 'Canada') {
	        $('#ProvinceInput').removeClass('hidden');
	    }
	    else {
	        $('#ProvinceInput').addClass('hidden');
	        $('#ProvinceDropDown').val('');
	    }
	})
});