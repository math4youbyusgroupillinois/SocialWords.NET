var meanings = {
    "syzygy": "the straight line configuration of 3 celestial bodies (as the sun and earth and moon) in a gravitational system" ,
    "likelihood": "Likelihood is a measure of the chance that the hazardous event will occur" ,
    "incident": "an event or condition that doesn’t cause harm but has the potential to do so" ,
   "pollution": "Pollution is the release of harmful substances or energy into the environment. It can contaminate air, land and water"
};

document.querySelector(".lookup-button").addEventListener("click", function () {
    document.querySelector(".card-container").classList.add("onscreen");    
    document.querySelector(".card-container").classList.add("selected");

    var lookupInput = document.querySelector(".lookup-input").innerText.replace(/\s+/ig, '');

    document.querySelector(".word").innerHTML = lookupInput + ' <span class="speech-part">(n.)</span>';
    document.querySelector(".card-small-content").innerHTML = lookupInput;
    
    document.querySelector(".meaning").innerHTML = '<span class="speech-part">(n.)</span> ' + meanings[lookupInput];
    document.querySelector(".hash").innerHTML = '#' + lookupInput;
});

document.querySelector(".share-button").addEventListener("click", function(){
    document.querySelector(".share-container").classList.add("onscreen");    
    document.querySelector(".card-container").classList.remove("selected");
    document.querySelector(".share-container").classList.add("selected");

    if(document.querySelector(".card").classList.contains("hidden")){
        var node = document.querySelector(".card-small").cloneNode(true); 
        node.style.cssText = "-webkit-transform: scale(0.6) translate(-50px,-15px)";
        document.querySelector(".current-card-container").innerHTML = "";
        document.querySelector(".current-card-container").appendChild(node);
    }
    else{
        var node = document.querySelector(".card").cloneNode(true); 
        node.style.cssText = "-webkit-transform: scale(0.38) translate(-300px,-160px)";
        document.querySelector(".current-card-container").innerHTML = "";    
        document.querySelector(".current-card-container").appendChild(node);
    }

    var lookupInput = document.querySelector(".lookup-input").innerText.replace(/\s+/ig, '');

    var url = "http://pearson.lexicum.net/" + "definition" + "/index/?word=" + lookupInput;
    document.getElementById("facebook").setAttribute("href", "http://www.facebook.com/sharer.php?src=sp&u=" + url);

    
});

document.getElementById("question").addEventListener("click", function(){
    document.getElementById("question").classList.add("selected");
    document.getElementById("definition").classList.remove("selected");

    document.querySelector(".card-small-container").classList.remove("hidden");    
    document.querySelector(".card").classList.add("hidden");
    query_type = "question";
});

document.getElementById("definition").addEventListener("click", function(){
    document.getElementById("question").classList.remove("selected");
    document.getElementById("definition").classList.add("selected");

    document.querySelector(".card-small-container").classList.add("hidden");    
    document.querySelector(".card").classList.remove("hidden");
    query_type = "definition";
});