$(document).ready(
	// Function to change the follow button to follow 
	//console.log(window.location.pathname),
	function click(){
	$('.follow-button').on("click", function(){
		$('.follow-button').css('background-color','green');
		$('.follow-button').html('<div class="icon-ok"></div> Following');
	});	
	},

	window.onload = function(){
		/* This hides the messages warning theuser to enable JS */
		try{
			var hideme = document.getElementById("JSwarning");
			hideme.style.display = "none";
		}catch{}


		/* This checks and sets wether or not the user is on mobile for future use */
		var isMobile = false; //initiate as false
		// device detection
		if(/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|ipad|iris|kindle|Android|Silk|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino/i.test(navigator.userAgent) 
			|| /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(navigator.userAgent.substr(0,4))) { 
			isMobile = true;
		}

		/*
		this orders the pictures on the homepage into roughly equal columns.
		we define the columns and a map to store their heights,
		and initialise the shortest as the first column
		*/
		var pictures = document.getElementsByClassName("image-box");
		const parent1 = document.getElementById("column-one");
		const parent2 = document.getElementById("column-two");
		const parent3 = document.getElementById("column-three");
		const parent4 = document.getElementById("column-four");
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
			//re-calculates the shortest column
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


function drawDiv(div){
	var x = div;//document.getElementById(div);
	if (x.style.display == "block") {
	  x.style.display = "none";
	} else {
	  x.style.display = "block";
	}
}

function toggleLikeButton(){
	var likeBtn = document.getElementById("likeBtn");
	var dislikeBtn = document.getElementById("disLikeBtn");

	if (likeBtn.classList.contains("d-none")){
		likeBtn.classList.remove("d-none");
		dislikeBtn.classList.add("d-none")
		console.log("test1")
	}
	else if (dislikeBtn.classList.contains("d-none")){
		dislikeBtn.classList.remove("d-none");
		likeBtn.classList.add("d-none")
		console.log("test2")
	}
}