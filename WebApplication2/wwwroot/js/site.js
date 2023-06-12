const todoForm = document.getElementById('todoForm');
const todoInput = document.getElementById('todoInput');
const todoList = document.getElementById('todoList');

todoForm.addEventListener('submit', function (event) {
    event.preventDefault();
    const todoText = todoInput.value.trim();
    if (todoText !== '') {
        addTodoItem(todoText);
        todoInput.value = '';
    }
});

todoList.addEventListener('click', function (event) {
    if (event.target.matches('input[type="checkbox"]')) {
        const todoItem = event.target.closest('li');
        const id = todoItem.querySelector("input").getAttribute("data-id");
        markAsCompleted(Number(id), todoItem);
    } else if (event.target.matches('.btn-remove')) {
        const todoItem = event.target.closest('li');
        todoItem.remove();
    }
});


function markAsCompleted(id, todoItem) {
    fetch(`/Todoes/MarkAsComplete/${id}`)
        .then(response => {
            if (response.ok) {
                Toastify({
                    text: "Completed",
                    duration: 3000,
                    close: true,
                    gravity: "bottom",
                    position: "right",
                    stopOnFocus: true,
                    style: {
                        background: "linear-gradient(to right, #00b09b, #96c93d)",
                    },
                    onClick: function () { }
                }).showToast();

                todoItem.classList.toggle('completed');
                return response.json();
            } else if (response.status === 400) {
                return response.json().then(error => {
                    Toastify({
                        text: error.message,
                        duration: 3000,
                        close: true,
                        gravity: "bottom", 
                        position: "right", 
                        stopOnFocus: true,
                        style: {
                            background: "linear-gradient(to right, #FF4E50, #F9D423)",
                        },
                        onClick: function () { }
                    }).showToast();
                });
            } else {
                throw new Error("Request failed");
            }
        })
        .then(data => {
        })
        .catch(error => {
            console.error(error);
        });

}


function addTodoItem(text) {
    const todoItem = document.createElement('li');
    todoItem.className = 'todo-item';
    todoItem.innerHTML = `
        <input type="checkbox">
        <label>${text}</label>
        <button class="btn btn-remove">Remove</button>
      `;
    todoList.appendChild(todoItem);
}