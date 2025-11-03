<template>
  <div class="todo-app">
    <h1>Todo List</h1>

    <!-- Add Todo -->
    <div class="add-todo">
      <input v-model="state.newTitle" placeholder="Title" />
      <input v-model="state.newDescription" placeholder="Description" />
      <select v-model="state.newCategory">
        <option value="Work">Work</option>
        <option value="Personal">Personal</option>
        <option value="Hobby">Hobby</option>
      </select>
      <button @click="onAddTodo">Add</button>
    </div>

    <!-- Todos -->
    <ul>
      <li v-for="todo in state.todos" :key="todo.id">
        {{ todo.id }}) {{ todo.title }} - {{ todo.description }} ({{ todo.category }}) Completed: {{ todo.isCompleted }}

        <button @click="onRemove(todo.id)">Remove</button>

        <div class="progress-section">
          <ul v-if="todo.progressions.length">
            <li v-for="(p, index) in todo.progressions" :key="p.date">
              {{ formatDate(p.date) }} - {{ cumulativePercent(todo, index) }}%
              <div class="progress-bar">
                <div class="progress-fill" :style="{ width: cumulativePercent(todo, index) + '%' }"></div>
              </div>
              <pre class="progress-text">{{ progressBar(todo, index) }}</pre>
            </li>
          </ul>
          <span v-else>No progress yet</span>

          <div class="add-progress">
            <input type="number" v-model.number="todo.newPercent" placeholder="Progress %" min="1" max="100" />
            <button @click="onAddProgress(todo)">Add Progress</button>
          </div>
        </div>
      </li>
    </ul>
  </div>
</template>

<script lang="ts" setup>
import { useTodoListLogic, formatDate } from './TodoList';

const { state, onAddTodo, onRemove, onAddProgress, cumulativePercent, progressBar } = useTodoListLogic();
</script>

<style src="../assets/styles.css"></style>
