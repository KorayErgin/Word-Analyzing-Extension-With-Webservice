
alert('enes');
function saveClipboard(data) {
	alert('e11aaa11nes');
		var result;
  	$.ajax({
        url:'http://localhost:18466/WebService1.asmx?op=HelloWorld',
        type:'GET',
        async:false,
        dataType:"json",
       alert(data);
    });
    return result;
}

