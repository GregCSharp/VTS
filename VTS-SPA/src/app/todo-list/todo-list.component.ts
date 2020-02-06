import { Component, OnInit } from '@angular/core';
import { ToDoList } from '../_models/todolist';
import { ActivatedRoute } from '@angular/router';
import { TodoService } from '../_services/todo.service';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-todo-list',
  templateUrl: './todo-list.component.html',
  styleUrls: ['./todo-list.component.css']
})
export class TodoListComponent implements OnInit {
  lists: ToDoList[];

  constructor(private authService: AuthService, private todoService: TodoService,
              private alertify: AlertifyService) { }

  ngOnInit() {}

  loadToDoList() {
    this.todoService.getToDoListsForUser(this.authService.decodedToken.nameid)
    .subscribe((lists: ToDoList[]) => {
      this.lists = lists;
    }, error => {
      this.alertify.error(error);
    });
  }

}
