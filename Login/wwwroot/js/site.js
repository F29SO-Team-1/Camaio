// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Function to change the follow button to follow 
$(document).ready(

	function click(){
	$('.follow-button').on("click", function(){
		$('.follow-button').css('background-color','green');
		$('.follow-button').html('<div class="icon-ok"></div> Following');
	});	
	}

);

function alertMessage() {
	confirm("Your channel will be deleted");
  }
