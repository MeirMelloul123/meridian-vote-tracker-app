import { Component, OnInit, OnDestroy, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { ApiService } from '../api.service';

@Component({
  selector: 'app-conflicts',
  imports: [],
  templateUrl: './conflicts.html',
  styleUrl: './conflicts.css',
})
export class Conflicts {
  private api = inject(ApiService);
  conflicts = signal<any[]>([]);
  message = signal('');
  private timer: any;

  ngOnInit() {
    this.load();
  }


  load() {
    this.api.getConflicts().subscribe(data => this.conflicts.set(data));
  }
  decide(conflict: any, accept: boolean, comment: string) {
    this.api.decide(conflict.id, accept, comment).subscribe({
      next: () => {
        this.message.set(accept ? 'Accepted.' : 'Rejected.');
        this.load();
      },
      error: err => {
        this.message.set(err.status === 409 ? 'Already decided by someone else.' : 'Error.');
        this.load();
      }
    });
  }
}
