// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Function to change the follow button to follow 
$(document).ready(
	console.log(window.location.pathname),
	function click(){
	$('.follow-button').on("click", function(){
		$('.follow-button').css('background-color','green');
		$('.follow-button').html('<div class="icon-ok"></div> Following');
	});	
	},
	window.onload = function(){
		//this orders the pictures on the homepage into columns
		var pictures = document.getElementsByClassName("image-box");
		const parent1 = document.getElementById("column-one");
		const parent2 = document.getElementById('column-two');
		const parent3 = document.getElementById('column-three');
		const parent4 = document.getElementById('column-four');
		var h1,h2,h3,h3 = 0;
		for (var i = 0; i < pictures.length; i++) {
			var short = min(h1,h2,h3,h4);
			if (i%4==0){
				parent1.appendChild(pictures[i]);
				h1+=pictures[i].clientHeight;
			}
			else if (i%4==1){
				parent2.appendChild(pictures[i]);
				h2+=pictures[i].clientHeight;
			}
			else if (i%4==2){
				parent3.appendChild(pictures[i]);
				h3+=pictures[i].clientHeight;
			}
			else if (i%4==3){
				parent4.appendChild(pictures[i]);
				h4+=pictures[i].clientHeight;
			}
		}
		
		console.log(window.location.pathname);
		//This section hides specific elements in the navbar if you're on the page it links to
		/*if (window.location.pathname == ""){

		}
		*/
	}

);

function alertMessage() {
	confirm("Your channel will be deleted");
}

function searchFunc(){
	var userInputValue = document.getElementById('form1').value;
	//document.location.href = "Search";
	var pictures = document.getElementsByClassName("image-box");
	//for (var i = 0; i < pictures.length; i++){
		//pictures[i];
	//}
}

function drawDiv(div){
	console.log(div)
	var x = div;//document.getElementById(div);
	if (x.style.display === "none") {
	  x.style.display = "block";
	} else {
	  x.style.display = "none";
	}
}