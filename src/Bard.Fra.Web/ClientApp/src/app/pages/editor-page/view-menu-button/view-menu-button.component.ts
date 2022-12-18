import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-view-menu-button',
  templateUrl: './view-menu-button.component.html',
  styleUrls: ['./view-menu-button.component.less']
})
export class ViewMenuButtonComponent implements OnInit {

  @Input() viewMode: string;
  @Input() selectedViewMode: string;
  @Output() selectEvent = new EventEmitter<string>();

  public hover = false;

  ngOnInit(): void {
  }

}
