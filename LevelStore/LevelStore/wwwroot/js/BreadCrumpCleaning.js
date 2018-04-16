  function BreadCrumpCleaning(){
    var Deletes = $(".breadcrumpSelector span");
    for(var i = 0; i < Deletes.length; i++){
        if(i!== 0){
        $(Deletes[i]).replaceWith( "<div class=\"breadcrumpBlockRight\"></div>" );
        }
        else{
            $(Deletes[i]).replaceWith( "<div class=\"breadcrumpBlockUp\"></div>" );
        }
    }
    var TextBlocks = $(".breadcrumpSelector li");
    for(var i = 0; i < TextBlocks.length; i++){        
        if(i === (TextBlocks.length - 1)){
            $(TextBlocks[i]).addClass("breadcrumpTextBlockActive");
        }
        else{
            $(TextBlocks[i]).addClass("breadcrumpTextBlock");
        }
      }
      $("#breadCrumb").show();
  }
 