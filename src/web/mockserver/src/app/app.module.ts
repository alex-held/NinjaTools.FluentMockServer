import { Routes } from '@angular/router';
import { ChartModule } from 'primeng/chart';
import { InputTextModule } from 'primeng/inputtext';
import { SidebarModule } from 'primeng/sidebar';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { PanelModule } from 'primeng/panel';
import { ToastModule } from 'primeng/toast';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { ConfigModule } from './config/config.module';
import { MenubarComponent } from './shared/menubar/menubar.component';
import { MenubarModule } from 'primeng/menubar';
import { AdminModule } from './admin/admin.module';
import { MatPasswordStrengthModule } from '@angular-material-extensions/core';
import { ServiceWorkerModule } from '@angular/service-worker';
import { environment } from '../environments/environment';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgZorroAntdModule, NZ_I18N, zh_CN } from 'ng-zorro-antd';
import { registerLocaleData } from '@angular/common';
import zh from '@angular/common/locales/zh';
import { TestComponent } from './test/test.component';
import { TestDialogComponent } from './test-dialog/test-dialog.component';
import { MatButtonModule, MatDialogModule } from '@angular/material';

registerLocaleData(zh);


const appRoutes: Routes = [
    { path: "", component: AppComponent}
] 


@NgModule({
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        PanelModule,
        SidebarModule,
        ButtonModule,
        TableModule,
        InputTextModule,
        MenubarModule,
        ChartModule,
        ToastModule,
        BrowserModule,
        BrowserAnimationsModule,
        ConfigModule,
        MatPasswordStrengthModule,
        ServiceWorkerModule.register('ngsw-worker.js', { enabled: environment.production }),
        FormsModule,
        HttpClientModule,
        NgZorroAntdModule,
        MatButtonModule,
        MatDialogModule
      ],
    declarations: [
    AppComponent,
    MenubarComponent,
    TestComponent,
    TestDialogComponent
],

  providers: [{ provide: NZ_I18N, useValue: zh_CN }],
  bootstrap: [AppComponent],
  entryComponents: [TestDialogComponent]
})


export class AppModule { }
