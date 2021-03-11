﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Function to change the follow button to follow 
$(document).ready(

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
		for (var i = 0; i < pictures.length; i++) {
			console.log("t");
			if (i%4==0){
				parent1.appendChild(pictures[i]);
			}
			else if (i%4==1){
				parent2.appendChild(pictures[i]);
			}
			else if (i%4==2){
				parent3.appendChild(pictures[i]);
			}
			else if (i%4==3){
				parent4.appendChild(pictures[i]);
			}
		}
	}

);

