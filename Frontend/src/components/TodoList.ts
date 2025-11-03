import { reactive, onMounted } from 'vue';
import { TodoItem, getTodos, addTodo, removeTodo, addProgress } from '../api';

export const state = reactive({
  todos: [] as TodoItem[],
  newTitle: '',
  newDescription: '',
  newCategory: 'Work',
});

export async function loadTodos() {
  const data = await getTodos();
  state.todos = data.map(t => ({ ...t, newPercent: 0 }));
}

export async function onAddTodo() {
  if (!state.newTitle) return;
  await addTodo({
    title: state.newTitle,
    description: state.newDescription,
    category: state.newCategory
  });
  state.newTitle = '';
  state.newDescription = '';
  state.newCategory = 'Work';
  await loadTodos();
}

export async function onRemove(id: number) {
  await removeTodo(id);
  await loadTodos();
}

export async function onAddProgress(todo: TodoItem) {
  if (!todo.newPercent || todo.newPercent <= 0) return;
  await addProgress(todo.id, todo.newPercent);
  todo.newPercent = 0;
  await loadTodos();
}

export function cumulativePercent(todo: TodoItem, index: number) {
  return todo.progressions.slice(0, index + 1).reduce((acc, p) => acc + p.percent, 0);
}

export function progressBar(todo: TodoItem, index: number) {
  const percent = cumulativePercent(todo, index);
  const totalBars = 50;
  const filled = Math.round((percent / 100) * totalBars);
  return '|' + 'O'.repeat(filled).padEnd(totalBars) + '|';
}

export function formatDate(isoString: string): string {
    const date = new Date(isoString);
    return date.toLocaleString(); // e.g., 3/11/2025, 10:53:34 AM
  }

export function useTodoListLogic() {
  onMounted(loadTodos);
  return {
    state,
    onAddTodo,
    onRemove,
    onAddProgress,
    cumulativePercent,
    progressBar
  };
}
