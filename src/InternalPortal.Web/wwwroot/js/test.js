var questionList = document.querySelectorAll('.question');
var nextButton = document.querySelector('.btn-next');
var sendButton = document.getElementById('sendbutton');
let index = 0;

function nextQuestion(){
    if(index == 0){
    questionList[index].classList.remove('question--hidden');        
    index++;
    }
    else{
      if(index < questionList.length){
        questionList[index-1].classList.add('question--hidden');    
        questionList[index].classList.remove('question--hidden');
        index++;
      }else{
        sendButton.classList.replace('button-classic--hidden','btn-send');
        nextButton.classList.replace('btn-next','button-classic--hidden')
      }
    }        
}


function addListener(){
    nextButton.addEventListener('click', nextQuestion)
}

 nextQuestion();

document.addEventListener("DOMContentLoaded", addListener);