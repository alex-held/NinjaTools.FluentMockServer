import { PageNotFoundComponent } from './shared/error-pages/page-not-found.component';
import { AdminModule } from './admin/admin.module';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { RouterModule } from '@angular/router';
import { RestoreModule } from './restore/restore.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';


import { Routes } from '@angular/router';

const appRoutes: Routes = [

  { path: 'admin', loadChildren: () => import('./admin/admin.module').then(mod => mod.AdminModule) },
  { path: 'restore', loadChildren: () => import('./restore/restore.module').then(m => m.RestoreModule) },

  // DEFAULT
  { path: '**', component: PageNotFoundComponent },
  { path: '', redirectTo: '/admin', pathMatch: 'full'}
]


@NgModule({
  imports: [
    RouterModule.forRoot(appRoutes),
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    AdminModule,
    RestoreModule
  ],

  declarations: [
    AppComponent,
    PageNotFoundComponent
  ],

  providers: [

  ],

  bootstrap: [AppComponent]
})


export class AppModule { }
