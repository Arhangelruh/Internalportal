var antiforgery = document.getElementsByName("__RequestVerificationToken")[0].value
const file = document.getElementById("file");
const formlable = document.querySelector(".choise-file");
var uploadFileElement = document.querySelectorAll(".file-button");
var refuseFileButton = document.querySelector(".revertfile-button");
var resultElement = document.querySelector(".result");

function uploadFile(input){
    let fileName = input.target.files[0].name
    document.querySelector('.file-name').innerText = fileName
    formlable.setAttribute("style","display:none;")
    uploadFileElement.forEach((item) => {
        item.setAttribute("style","display:flex;")
    });
  }

  function skipFile(){
    uploadFileElement.forEach((item)=>{
        item.removeAttribute("style");
    })
    formlable.removeAttribute("style");
    resultElement.removeAttribute("style");
  }

function setInput(){  
    file.addEventListener('change', uploadFile);
    refuseFileButton.addEventListener('click', skipFile);
  } 

  async function arro(data){
    var t = await data.text();    
    JSON.parse(t,(key, value) => {
      if(value!=null&&key!=""){
        showData(value);
      }
    });
  }

  function showData(value){  
    // resultElement.innerHTML='';
     let addblock = document.createElement('div')
       addblock.innerHTML = value;             
       resultElement.appendChild(addblock);
       resultElement.setAttribute("style","display:flex;")     
  }

async function AJAXSubmit(oFormElement) {
    const formData = new FormData(oFormElement);
                     
    try {
        const response = await fetch(oFormElement.action, {
            method: 'POST',
            headers: {                          
                'RequestVerificationToken': antiforgery
            },
            body: formData
        });
        
        if (response.ok) {
            window.location.href = '/Cash/Education/';
          }
          else{      
            arro(response)
          }        
    } catch (error) {
        console.error('Error:', error);
    }
}


document.addEventListener("DOMContentLoaded",setInput);