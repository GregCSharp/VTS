import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { ToDoList } from '../_models/todolist';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TodoService {
  baseUrl: string = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getToDoListsForUser(userId): Observable<ToDoList[]> {
    return this.http.get<ToDoList[]>(this.baseUrl + 'users/' + userId + '/lists');
  }
}
