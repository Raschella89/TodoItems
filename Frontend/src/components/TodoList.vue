<template>
  <div>
    <h1>Todo List</h1>
    <div class="add-todo">
      <input v-model="newTitle" placeholder="Title" />
      <input v-model="newDescription" placeholder="Description" />
      <input v-model="newCategory" placeholder="Category" />
      <button @click="addTodo">Add</button>
    </div>

    <ul>
      <li v-for="todo in todos" :key="todo.id">
        {{ todo.id }}) {{ todo.title }} - {{ todo.description }} ({{ todo.category }}) Completed: {{ todo.isCompleted }}
        <ul>
          <li v-for="p in todo.progressions" :key="p.date">
            {{ p.date }} - {{ p.percent }}% |{{ 'O'.repeat(Math.round(p.percent / 2)).padEnd(50) }}|
          </li>
        </ul>
        <button @click="removeTodo(todo.id)">Remove</button>
      </li>
    </ul>
  </div>
</template>

<script lang="ts">
import { ref, onMounted } from 'vue';

interface Progression {
  date: string;
  percent: number;
}

interface Todo {
  id: number;
  title: string;
  description: string;
  category: string;
  isCompleted: boolean;
  progressions: Progression[];
}

const API_URL = 'http://localhost:5101/api/todolist';

export default {
  setup() {
    const todos = ref<Todo[]>([]);
    const newTitle = ref('');
    const newDescription = ref('');
    const newCategory = ref('');

    const loadTodos = async () => {
      try {
        const res = await fetch(API_URL);
        todos.value = await res.json();
      } catch (err) {
        console.error(err);
      }
    };

    const addTodo = async () => {
      try {
        await fetch(API_URL, {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({
            title: newTitle.value,
            description: newDescription.value,
            category: newCategory.value
          })
        });
        newTitle.value = '';
        newDescription.value = '';
        newCategory.value = '';
        await loadTodos();
      } catch (err) {
        console.error(err);
      }
    };

    const removeTodo = async (id: number) => {
      try {
        await fetch(`${API_URL}/${id}`, { method: 'DELETE' });
        await loadTodos();
      } catch (err) {
        console.error(err);
      }
    };

    onMounted(() => {
      loadTodos();
    });

    return { todos, newTitle, newDescription, newCategory, addTodo, removeTodo };
  }
};
</script>

<style>
.add-todo input {
  margin-right: 8px;
}
button {
  margin-left: 4px;
}
</style>
