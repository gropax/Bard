import { Component, Input, OnInit } from '@angular/core';
import { PhonGraphWord } from '../../models/word-form';

@Component({
  selector: 'app-rhymes-page',
  templateUrl: './rhymes-page.component.html',
  styleUrls: ['./rhymes-page.component.less']
})
export class RhymesPageComponent implements OnInit {

  selectedWord: PhonGraphWord;

  constructor() { }

  ngOnInit(): void {
  }

  onWordSelected(word: PhonGraphWord) {
    this.selectedWord = word;
  }

}
