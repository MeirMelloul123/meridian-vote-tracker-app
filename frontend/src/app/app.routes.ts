import { Routes } from '@angular/router';
import { Imports } from './imports/imports';
import { Conflicts } from './conflicts/conflicts';

export const routes: Routes = [
  { path: '', redirectTo: 'imports', pathMatch: 'full' },
  { path: 'imports', component: Imports },
  { path: 'conflicts', component: Conflicts }
];