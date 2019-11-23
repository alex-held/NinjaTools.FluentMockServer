import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { ChartModule } from 'primeng/chart';


import { DoughnutChartComponent } from './doughnut-chart.component';

describe('DoughnutChartComponent', () => {
  let component: DoughnutChartComponent;
  let fixture: ComponentFixture<DoughnutChartComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [ 
        NoopAnimationsModule,
        ChartModule
      ],
      declarations: [ DoughnutChartComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DoughnutChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should compile', () => {
    expect(component).toBeTruthy();
  });
});