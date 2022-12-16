import { Input, } from '@angular/core';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-page-content',
  templateUrl: './page-content.component.html',
  styleUrls: ['./page-content.component.less']
})
export class PageContentComponent implements OnInit {

  @Input() loading!: boolean;

  constructor() { }

  ngOnInit() {
  }

}

