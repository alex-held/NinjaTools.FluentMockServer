import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminComponent } from './admin.component';
import { Routes } from '@angular/router';
import { SetupListComponent } from './setup-list/setup-list.component';
import { MenuModule } from 'primeng/components/menu/menu';

import {ToolbarModule} from 'primeng/toolbar';
import {FileUploadModule} from 'primeng/fileupload';
import {ToastModule} from 'primeng/toast';
import {MessagesModule} from 'primeng/messages';
import {MessageModule} from 'primeng/message';
import {DragDropModule} from 'primeng/dragdrop';
import {TerminalModule} from 'primeng/terminal';
import {InputMaskModule} from 'primeng/inputmask';
import {PasswordModule} from 'primeng/password';


const adminRoutes: Routes = [
  { path: 'list', component: SetupListComponent },
  { path: '', component: AdminComponent}
];


@NgModule({
  imports: [

    // PrimeNG
   // EditorModule,
    ToolbarModule, MessagesModule, MessageModule , DragDropModule, TerminalModule, InputMaskModule, PasswordModule
, FileUploadModule, ToastModule,






    CommonModule,
    MenuModule,

    RouterModule.forChild(adminRoutes),


  ],
  declarations: [
    SetupListComponent,
    AdminComponent
  ]
})


export class AdminModule { }
