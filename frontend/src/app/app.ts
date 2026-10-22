import { Component, OnInit, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ApiService } from './api.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html'
})
export class App implements OnInit {
  private api = inject(ApiService);
  users = signal<any[]>([]);

  ngOnInit() {
    this.api.getUsers().subscribe(data => this.users.set(data));
  }

  onUserChange(event: Event) {
    this.api.userId = Number((event.target as HTMLSelectElement).value);
  }
}