// src/api.ts
export interface TodoItem {
  id: number;
  title: string;
  description: string;
  category: string;
  isCompleted: boolean;
  progressions: { date: string; percent: number }[];
}

const BASE_URL = 'http://localhost:5101/api/todolist';

export async function getTodos(): Promise<TodoItem[]> {
  const res = await fetch(BASE_URL);
  if (!res.ok) throw new Error('Failed to fetch todos');
  return res.json();
}

export async function addTodo(todo: { title: string; description: string; category: string }): Promise<void> {
  const res = await fetch(BASE_URL, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(todo)
  });
  if (!res.ok) throw new Error('Failed to add todo');
}

export async function removeTodo(id: number): Promise<void> {
  const res = await fetch(`${BASE_URL}/${id}`, {
    method: 'DELETE'
  });
  if (!res.ok) throw new Error('Failed to remove todo');
}
