import { PanelModule } from 'primeng/panel';

import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SidebarModule } from 'primeng/sidebar';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { InputTextModule } from 'primeng/inputtext';
import { MenubarModule } from 'primeng/menubar';
import { ChartModule } from 'primeng/chart';
import { ToastModule } from 'primeng/toast';
import { DashboardComponent } from './dashboard.component';
import { TableComponent } from './components/table/table.component';
import { PolarAreaChartComponent } from './components/polar-area-chart/polar-area-chart.component';
import { DoughnutChartComponent } from './components/doughnut-chart/doughnut-chart.component';
import { RadarChartComponent } from './components/radar-chart/radar-chart.component';
import { PieChartComponent } from './components/pie-chart/pie-chart.component';
import { LineChartComponent } from './components/line-chart/line-chart.component';
import { BarChartComponent } from './components/bar-chart/bar-chart.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';



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
        ToastModule
      ],
    declarations: [
    DashboardComponent,
    SidebarComponent,
    TableComponent,
    PolarAreaChartComponent,
    DoughnutChartComponent,
    RadarChartComponent,
    PieChartComponent,
    LineChartComponent,
    BarChartComponent, 
  ],

  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
