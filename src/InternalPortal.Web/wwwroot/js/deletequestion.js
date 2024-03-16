var deleteArray = document.querySelectorAll(".Delete_question");

function check(){
   deleteArray.forEach(
       function (element){                             
       element.addEventListener("click",deleteRequest)}
   );
}

async function deleteRequest(){        
    var link = "/Test/DeleteQuestion/"+ this.name;   
    var el = this; 
    const responce = await fetch(link,
    {
      method:"Get"
    }
    )
    .then(responce => responce.json())
        if (responce.status == "error"){              
              var checkdiv =  el.querySelector(".tooltiptext");
           if(checkdiv == null){              
               let addblock = document.createElement('div')
               addblock.innerHTML = "Удаление не возможно этот вопрос был в тестировании.";
               addblock.className = "tooltiptext";
               addblock.style.display="inline";          
               el.append(addblock);
      
            document.onmouseout = function(e){
              if(addblock){                      
                el.removeChild(addblock);            
                addblock = null;
              }
             }   
           }        
       }
    else{
        if(responce.status == "success"){                        
            window.location.assign('/Test/GetTestQuestions/?' + new URLSearchParams({                
                 topicId: responce.topicId,
                 cashTestId:responce.cashTestId 
            }));           
        }
        else{
            window.location.assign('/Test/GetCashTests/')
        }
         
    }
}

document.addEventListener("DOMContentLoaded", check);