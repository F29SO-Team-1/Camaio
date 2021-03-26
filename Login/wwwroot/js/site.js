﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
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

		/*
		this orders the pictures on the homepage into columns
		we define the columns and a map to store their heights,
		and initialise the shortest as the first column
		*/
		var pictures = document.getElementsByClassName("image-box");
		const parent1 = document.getElementById("column-one");
		const parent2 = document.getElementById('column-two');
		const parent3 = document.getElementById('column-three');
		const parent4 = document.getElementById('column-four');
		let divheights = new Map();
		divheights.set(0,[parent1,0])
		divheights.set(1,[parent2,0])
		divheights.set(2,[parent3,0])
		divheights.set(3,[parent4,0])
		var shortest = divheights.get(0);
		
		// loops over each image placing it in the shortest column
		for (var i = 0; i < pictures.length; i++) {
			shortest[0].appendChild(pictures[i]);
			shortest[1]+=pictures[i].clientHeight;
			for (var j = 0; j < divheights.size; j++){
				if (shortest[1] > divheights.get(j)[1]){
					shortest = divheights.get(j);
					
				}
			}
		}	
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