<template>
  <div class="todo-app">
    <h1>Todo List</h1>

    <!-- Add Todo -->
    <div class="add-todo">
      <input v-model="newTitle" placeholder="Title" />
      <input v-model="newDescription" placeholder="Description" />
      <select v-model="newCategory">
        <option value="Work">Work</option>
        <option value="Personal">Personal</option>
        <option value="Hobby">Hobby</option>
      </select>
      <button @click="addTodo">Add</button>
    </div>

    <!-- Todos -->
    <ul>
      <li v-for="todo in todos" :key="todo.id">
        {{ todo.id }}) {{ todo.title }} - {{ todo.description }} ({{ todo.category }}) Completed: {{ todo.isCompleted }}

        <!-- Remove -->
        <button @click="removeTodo(todo.id)">Remove</button>

        <!-- Progressions -->
        <div class="progress-section">
          <ul v-if="todo.progressions.length > 0">
            <li v-for="(p, index) in todo.progressions" :key="p.date">
              {{ p.date }} - {{ cumulativePercent(todo, index) }}%
              <div class="progress-bar">
                <div class="progress-fill" :style="{ width: cumulativePercent(todo, index) + '%' }"></div>
              </div>
              <pre class="progress-text">{{ progressBar(todo, index) }}</pre>
            </li>
          </ul>
          <span v-else>No progress yet</span>

          <!-- Add Progress -->
          <div class="add-progress">
            <input type="number" v-model.number="todo.newPercent" placeholder="Progress %" min="0" max="100" />
            <button @click="addProgress(todo)">Add Progress</button>
          </div>
        </div>
      </li>
    </ul>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";

interface Progress {
  date: string;
  percent: number;
}

interface TodoItem {
  id: number;
  title: string;
  description: string;
  category: string;
  isCompleted: boolean;
  progressions: Progress[];
  newPercent?: number;
}

const todos = ref<TodoItem[]>([]);

const newTitle = ref("");
const newDescription = ref("");
const newCategory = ref("Work");

async function loadTodos() {
  const res = await fetch("http://localhost:5101/api/todolist");
  const data = await res.json();
  todos.value = data.map((t: TodoItem) => ({ ...t, newPercent: 0 }));
}

async function addTodo() {
  if (!newTitle.value) return;
  await fetch("http://localhost:5101/api/todolist", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({
      title: newTitle.value,
      description: newDescription.value,
      category: newCategory.value
    })
  });
  newTitle.value = "";
  newDescription.value = "";
  newCategory.value = "Work";
  await loadTodos();
}

async function removeTodo(id: number) {
  await fetch(`http://localhost:5101/api/todolist/${id}`, { method: "DELETE" });
  await loadTodos();
}

async function addProgress(todo: TodoItem) {
  if (!todo.newPercent) return;
  await fetch(`http://localhost:5101/api/todolist/${todo.id}/progress`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ percent: todo.newPercent })
  });
  todo.newPercent = 0;
  await loadTodos();
}

// Compute cumulative percent up to this index
function cumulativePercent(todo: TodoItem, index: number): number {
  return todo.progressions.slice(0, index + 1).reduce((acc, p) => acc + p.percent, 0);
}

// Generate console-style OOO bar
function progressBar(todo: TodoItem, index: number): string {
  const percent = cumulativePercent(todo, index);
  const totalBars = 50; // 50 chars = 100%
  const filled = Math.round((percent / 100) * totalBars);
  return "|" + "O".repeat(filled).padEnd(totalBars) + "|";
}

onMounted(loadTodos);
</script>

<style scoped>
.todo-app {
  max-width: 600px;
  margin: 0 auto;
  font-family: Arial, sans-serif;
}

.add-todo input,
.add-todo select,
.add-todo button {
  margin-right: 5px;
}

.progress-section {
  margin-top: 5px;
}

.progress-bar {
  width: 100%;
  background: #eee;
  height: 10px;
  margin-top: 2px;
  border-radius: 4px;
  overflow: hidden;
}

.progress-fill {
  height: 10px;
  background: #4caf50;
}

.progress-text {
  font-family: monospace;
  margin: 0;
}
</style>
