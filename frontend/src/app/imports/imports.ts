import { Component, OnInit, OnDestroy, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { ApiService } from '../api.service';

@Component({
  selector: 'app-imports',
  imports: [DatePipe],
  templateUrl: './imports.html'
})
export class Imports implements OnInit, OnDestroy {
  private api = inject(ApiService);
  users= signal<any[]>([]);
  imports = signal<any[]>([]);
  message = signal('');
  private timer: any;

  ngOnInit() {
    this.load();
    this.timer = setInterval(() => this.load(), 2000);   // refresh progress every 2 s
  }

  ngOnDestroy() {
    clearInterval(this.timer);
  }

  load() {
    this.api.getImports().subscribe(data => this.imports.set(data));
  }

  onFileSelected(event: Event) {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (!file) return;

    this.message.set('Uploading...');
    this.api.uploadFile(file).subscribe({
      next: () => {
        this.message.set('File received, processing in the background.');
        this.load();
      },
      error: () => this.message.set('Upload failed.')
    });
  }
}