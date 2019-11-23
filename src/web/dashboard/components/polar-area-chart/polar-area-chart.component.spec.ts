import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { ChartModule } from 'primeng/chart';


import { PolarAreaChartComponent } from './polar-area-chart.component';

describe('PolarAreaChartComponent', () => {
  let component: PolarAreaChartComponent;
  let fixture: ComponentFixture<PolarAreaChartComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [ 
        NoopAnimationsModule,
        ChartModule
      ],
      declarations: [ PolarAreaChartComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PolarAreaChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should compile', () => {
    expect(component).toBeTruthy();
  });
});