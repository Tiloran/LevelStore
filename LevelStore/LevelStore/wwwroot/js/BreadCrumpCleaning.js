window.onload = function(){
    var deletes = $(".breadcrumpSelector span");
    for(let i = 0; i < deletes.length; i++){
        if(i!== 0){
        $(deletes[i]).replaceWith( "<div class=\"breadcrumpBlockRight\"></div>" );
        }
        else{
            $(deletes[i]).replaceWith( "<div class=\"breadcrumpBlockUp\"></div>" );
        }
    }
    var textBlocks = $(".breadcrumpSelector li");
    var lastWithContent = -1;
    for (let i = 0; i < textBlocks.length; i++) {
        
        $(textBlocks[i]).addClass("breadcrumpTextBlock");
        if (!($(textBlocks[i]).is(':empty'))) {
            lastWithContent = i;
        }
        
    }
    if (lastWithContent !== -1) {
        $(textBlocks[lastWithContent]).addClass("breadcrumpTextBlockActive");
    }
    $(".breadcrumpSelector").show();
}
 