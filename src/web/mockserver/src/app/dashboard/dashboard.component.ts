import { Component } from '@angular/core';

import { MenuItem } from 'primeng/api';

@Component({
  selector: 'sample-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent {
  cssClass = 'p-lg-4 p-md-6 p-sm-12';
}