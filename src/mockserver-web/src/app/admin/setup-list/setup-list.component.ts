import { Component, OnInit } from '@angular/core';


@Component({
  // tslint:disable-next-line: component-selector
  selector: 'setup-list',
  templateUrl: './setup-list.component.html'
})


export class SetupListComponent implements OnInit {


  ngOnInit(): void {
   console.log('init...');
  }



}


export class HttpRequst {

  public Method: string;

  public Path: string;

}

export interface ISetup {

  HttpRequst: HttpRequst ;

}
