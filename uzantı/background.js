
// Right Click - Copy Function
var itemCopy = chrome.contextMenus.create({ //sağ clik e kendi uzantımızı ekliyoruz
  "title"     : "Analiz Et",
  "contexts"  : ["selection"],
  "onclick"   : function f(data) {  // seçilen metni algılıyoruz
	  


  //var bb= data.selectionText.replace(/"/g, '');
  var bb= data.selectionText.replace(/["“”'?.,\/#!$%\^&\*;:{}=\-_`~()]/g,"") // metindeki özel karakterleri kaldırıyoruz
//alert(data.selectionText);

//alert(bb);
        $.ajax({  // seçilen metni webservis e atıyoruz
            type: "POST",  // webservis metodumuzu seçiyoruz
            url: "http://localhost:6951/ZemberekAnaliz.asmx/get_post", //webservisimiz adresini yazıyoruz
			 data:  '{dizi:"'+ bb+ '"}',   // fonksiyona hangi parametreye hangi değeri yollayacağımızı belirtiyoruız
            contentType: "application/json; charset=utf-8",
            dataType: "json",  
            success: function (r) { // eğer webservis e başarılı şekilde veri yolladıysak aşşağıdaki işlemler gerçekleşir
                //alert(r.d);
				//alert (JSON.stringify(r.d))
				 //yaz(r.d);
				 localStorage.data=r.d;  // 'r.d' webservisin bize geri yolladığı metin , sayfalar arası metin kullanmak için localstroge a metni koyuyoruz
				 var w = window.open('plug.html','_blank');  // yeni sekmeyi açıyoruz
			//	var w = window.open('background.html','_blank');
			//	//document.getElementById('lbltipAddedComment').innerHTML = 'aaaaaaaaaaa!';
       // w.focus();
				/*
				 chrome.tabs.create({
                 url: chrome.extension.getURL("plug.html")				 
        });*/
		//document.getElementById('lbltipAddedComment').innerHTML = 'your tip has been submitted!';
            },
            error: function (r) {
                alert(r.responseText);
            },
            failure: function (r) {
                alert(r.responseText);
            }
        });

	   
  }
  
});

