var questionList = document.querySelectorAll('.question');
var nextButton = document.querySelector('.btn-next');
var sendButton = document.getElementById('sendbutton');
var checkboxes = document.querySelectorAll('.checkbox-answer');

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
        nextButton.classList.add('btn--hidden');  
      }else{
        questionList[index-1].classList.add('question--hidden');
        sendButton.classList.replace('button-classic--hidden','btn-send');
        nextButton.classList.replace('btn-next','button-classic--hidden')
      }
    }        
}

 function checkboxChange() {
   var parent = this.parentElement;
   var question = parent.parentElement;
   var checkboxList = question.querySelectorAll('input[type=checkbox]')
    checkboxList.forEach((item) => {
        if (item !== this) item.checked = false
    });
    if(this.checked == true){
      nextButton.classList.remove('btn--hidden');
    }
    else{
      nextButton.classList.add('btn--hidden');       
    }
 }

function addListener(){
    nextButton.addEventListener('click', nextQuestion);
     checkboxes.forEach(
       function (element){                             
       element.addEventListener('click', checkboxChange)}
   );
}

nextQuestion();
document.addEventListener("DOMContentLoaded", addListener);