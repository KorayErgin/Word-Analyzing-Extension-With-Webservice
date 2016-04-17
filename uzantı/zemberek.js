

function sleep(milliseconds) {
  var start = new Date().getTime();
  for (var i = 0; i < 1e7; i++) {
    if ((new Date().getTime() - start) > milliseconds){
      break;
    }
  }
}

function count() {
  

  
  
  
  var x = document.createElement("IFRAME");
    x.setAttribute("src", "http://localhost:6951/ZemberekAnaliz.asmx/veritabani_Sil");
	x.visibility="hidden";
	x.display="none";
	x.width="0px";
	x.height="0px";
    document.body.appendChild(x);
sleep(250);
var f = document.getElementById('if1');
f.src = f.src;




}
document.getElementById('do-count').onclick = count;