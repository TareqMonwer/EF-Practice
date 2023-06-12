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
                showToast("Completed");

                todoItem.classList.toggle('completed');
                return response.json();
            } else if (response.status === 400) {
                return response.json().then(error => {
                    showToast(error.message, isError=true);
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


function createTodoItem(text) {
    const data = {
        "Title": '',
        "IsDeleted": false,
        "IsCompleted": false
    }
    return fetch("/Todoes", {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    })
        .then(response => response.json())
        .then(responseData => {
            if (responseData.status == 200) {
                return true;
            } else {
                return responseData.message;
            }
        })
        .catch(error => {
            return error.message;
        });
}

async function addTodoItem(text) {
    const created = await createTodoItem(text);

    if (created === true) {
        const todoItem = document.createElement('li');
        todoItem.className = 'todo-item';
        todoItem.innerHTML = `
        <input type="checkbox">
        <label>${text}</label>
        <button class="btn btn-remove">Remove</button>
      `;
        todoList.appendChild(todoItem);
    } else {
        showToast(created, isError = true);
    }
}


function showToast(message, isError) {
    Toastify({
        text: message,
        duration: 3000,
        close: true,
        gravity: "bottom",
        position: "right",
        stopOnFocus: true,
        style: {
            background: isError ?
                "linear-gradient(to right, #FF4E50, #F9D423)" :
                "linear-gradient(to right, #00b09b, #96c93d)",
        },
        onClick: function () { }
    }).showToast();
}