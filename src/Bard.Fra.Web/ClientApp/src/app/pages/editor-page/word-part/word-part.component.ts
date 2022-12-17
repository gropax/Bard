import { Component, ElementRef, EventEmitter, Input, OnInit, Output, Renderer2, ViewChild } from '@angular/core';
import { Word, WordPart } from '../../../models/text';

@Component({
  selector: 'app-word-part',
  templateUrl: './word-part.component.html',
  styleUrls: ['./word-part.component.less']
})
export class WordPartComponent implements OnInit {

  @ViewChild('span') span: ElementRef;

  @Input() wordPart: WordPart;
  @Input() selectedWord: Word | null;
  @Output() selectEvent = new EventEmitter<Word | null>();

  public hover = false;

  constructor(
    private renderer: Renderer2)
  {
    this.renderer.listen('window', 'click', (e:Event) => {
      if (e.target !== this.span.nativeElement) {
        this.selectEvent.emit(null);
      }
    });
  }

  ngOnInit(): void {
  }

}
