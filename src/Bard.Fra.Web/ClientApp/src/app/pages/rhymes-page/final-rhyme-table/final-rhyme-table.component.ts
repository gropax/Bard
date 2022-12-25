import { Component, Input, OnChanges, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { RhymingWordDataSource } from '../../../data-sources/final-rhyme-data-source';
import { PhonGraphWord } from '../../../models/word-form';
import { WordFormsService } from '../../../services/word-forms-service';

@Component({
  selector: 'app-final-rhyme-table',
  templateUrl: './final-rhyme-table.component.html',
  styleUrls: ['./final-rhyme-table.component.less']
})
export class FinalRhymeTableComponent implements OnInit, OnChanges {

  @Input() word: PhonGraphWord;

  displayedColumns: string[] = ['graphemes'];
  dataSource: RhymingWordDataSource;

  @ViewChild(MatPaginator) paginator: MatPaginator;


  constructor(private graphService: WordFormsService) {
    this.dataSource = new RhymingWordDataSource(graphService);
  }

  ngOnInit() {
  }

  ngOnChanges(changes: SimpleChanges) {
    console.log(changes);
    if (changes['word'].currentValue) {
      this.dataSource.loadLessons(
        this.word.graphemes,
        this.word.syllables.replace('.', ''));
    }
  }

}
