import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { RestoreComponent } from './restore.component';

export const restoreRoutes: Routes = [
  { path: '', component: RestoreComponent }
];




@NgModule({

  imports: [
    CommonModule,
    RouterModule.forChild(restoreRoutes)
  ],
  declarations: [
    RestoreComponent
  ],

})



export class RestoreModule { }
