import { Component, Input, OnInit, Renderer2 } from '@angular/core';
import { Strophe, Word } from '../../../models/text';

@Component({
  selector: 'app-token-view',
  templateUrl: './token-view.component.html',
  styleUrls: ['./token-view.component.less']
})
export class TokenViewComponent implements OnInit {

  @Input() strophes: Strophe[];

  public selectedWord: Word | null = null;
  public selectedWordElem: HTMLSpanElement | null = null;

  constructor(
    private renderer: Renderer2)
  {
    this.renderer.listen('window', 'click', (e:Event) => {
      if (e.target !== this.selectedWordElem) {
        this.selectedWord = null;
        this.selectedWordElem = null;
      }
    });
  }

  ngOnInit(): void {
  }

  public selectWord($event: [Word, HTMLSpanElement]) {
    this.selectedWord = $event[0];
    this.selectedWordElem = $event[1];
  }

}
