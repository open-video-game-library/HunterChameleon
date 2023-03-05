mergeInto(LibraryManager.library, {
    addData:function(jsonData){
        var jsonObj = JSON.parse(Pointer_stringify(jsonData));
        addJsonData(jsonObj);
    },
    downloadData:function(){
        downloadJsonData();
    },
 });