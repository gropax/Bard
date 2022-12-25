import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { Word } from '../../../../models/text';
import { WordPronunciation } from '../../../../models/word-form';
import { WordFormsService } from '../../../../services/word-forms-service';
import { Segment } from '../ipa-view.component';

@Component({
  selector: 'app-ipa-segment',
  templateUrl: './segment.component.html',
  styleUrls: ['./segment.component.less']
})
export class SegmentComponent implements OnInit {

  @ViewChild('span') span: ElementRef;

  @Input() segment: Segment;
  @Input() hoveredWord: Word | null;
  @Input() selectedWord: Word | null;
  @Output() hoverEvent = new EventEmitter<Word | null>();
  @Output() selectEvent = new EventEmitter<[Word, HTMLSpanElement]>();

  public pronunciations: WordPronunciation[];
  public loading: boolean;

  constructor(
    private wordFormsService: WordFormsService) {
  }

  ngOnInit(): void {
    this.loading = true;
    this.wordFormsService.getPronunciation({
      graphicalForm: this.segment.content,
    }).subscribe(pronunciations => {
      this.pronunciations = pronunciations;
      this.loading = false;
    });
  }

}
