import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { PanelModule } from 'primeng/panel';
import { ToastModule } from 'primeng/toast';
import { ChartModule } from 'primeng/chart';
import { TableModule } from 'primeng/table';
import { MenubarModule } from 'primeng/menubar';

import { DashboardComponent } from './dashboard.component';
import { MenubarComponent } from './components/menubar/menubar.component';
import { BarChartComponent } from './components/bar-chart/bar-chart.component';
import { DoughnutChartComponent } from './components/doughnut-chart/doughnut-chart.component';
import { LineChartComponent } from './components/line-chart/line-chart.component';
import { PieChartComponent } from './components/pie-chart/pie-chart.component';
import { PolarAreaChartComponent } from './components/polar-area-chart/polar-area-chart.component';
import { RadarChartComponent } from './components/radar-chart/radar-chart.component';
import { TableComponent } from './components/table/table.component';

describe('DashboardComponent', () => {
  let component: DashboardComponent;
  let fixture: ComponentFixture<DashboardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [ 
        NoopAnimationsModule,
        PanelModule,
        ChartModule,
        TableModule,
        MenubarModule,
        ToastModule
      ],
      declarations: [ 
        DashboardComponent,
        MenubarComponent,
        BarChartComponent,
        DoughnutChartComponent,
        LineChartComponent,
        PieChartComponent,
        PolarAreaChartComponent,
        RadarChartComponent,
        TableComponent
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should compile', () => {
    expect(component).toBeTruthy();
  });
});