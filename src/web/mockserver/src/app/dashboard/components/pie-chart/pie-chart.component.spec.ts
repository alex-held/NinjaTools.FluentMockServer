import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { ChartModule } from 'primeng/chart';


import { PieChartComponent } from './pie-chart.component';

describe('PieChartComponent', () => {
  let component: PieChartComponent;
  let fixture: ComponentFixture<PieChartComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [ 
        NoopAnimationsModule,
        ChartModule
      ],
      declarations: [ PieChartComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PieChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should compile', () => {
    expect(component).toBeTruthy();
  });
});