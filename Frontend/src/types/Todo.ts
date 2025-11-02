// src/types/todo.ts
export interface Progression {
  date: string;
  percent: number;
}

export interface TodoItem {
  id: number;
  title: string;
  description: string;
  category: string;
  isCompleted: boolean;
  progressions: Progression[];
  newPercent?: number;
}
