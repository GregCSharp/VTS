import { ToDoItem } from './todoitem';

export interface ToDoList {
    id: number;
    name: string;
    createdDate: Date;
    completedDate: Date;
    isComplete: boolean;
    items?: ToDoItem[];
}