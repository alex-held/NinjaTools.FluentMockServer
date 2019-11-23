import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'setup-listview',
  template: `
    <p>
      setup-listview works!

      <div class="container">
        <setup-thumbnail></setup-thumbnail>
      </div>

    <p>`,
  styles: []
})

export class SetupListviewComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}
