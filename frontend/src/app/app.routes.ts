import { Routes } from '@angular/router';
import { Imports } from './imports/imports';

export const routes: Routes = [
  { path: '', redirectTo: 'imports', pathMatch: 'full' },
  { path: 'imports', component: Imports }
];