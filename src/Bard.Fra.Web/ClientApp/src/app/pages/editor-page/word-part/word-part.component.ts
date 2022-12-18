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
  @Input() hoveredWord: Word | null;
  @Input() selectedWord: Word | null;
  @Output() hoverEvent = new EventEmitter<Word | null>();
  @Output() selectEvent = new EventEmitter<[Word, HTMLSpanElement]>();

  ngOnInit(): void {
  }

}
