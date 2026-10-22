import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

const API = 'http://localhost:8080/api';

// Single instance of the service, shared by the whole app
@Injectable({ providedIn: 'root' })
export class ApiService {
  private http = inject(HttpClient);

  // Current user (Dana by default)
  userId = 1;
  getUsers(){
    return this.http.get<any[]>(`${API}/users`);
  }
  getImports() {
    return this.http.get<any[]>(`${API}/import`);
  }

  uploadFile(file: File) {
    const form = new FormData();
    form.append('file', file);
    form.append('userId', String(this.userId));
    return this.http.post(`${API}/import`, form);
  }

  getReport(importId: number) {
    return this.http.get<any>(`${API}/import/${importId}/report`);
  }
  getConflicts() {
  return this.http.get<any[]>(`${API}/conflicts`);
}

decide(conflictId: number, accept: boolean, comment: string) {
  return this.http.post(`${API}/conflicts/decision`, {
    id: conflictId,
    userId: this.userId,
    isConflictAccepted: accept,
    comment: comment
  });
}
}