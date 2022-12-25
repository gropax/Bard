import { EventEmitter } from '@angular/core';
import { AfterViewInit, Component, ElementRef, OnInit, Output, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatAutocompleteSelectedEvent, MatAutocompleteTrigger } from '@angular/material/autocomplete';
import { distinctUntilChanged, empty, interval } from 'rxjs';
import { debounceTime } from 'rxjs';
import { of } from 'rxjs';
import { tap } from 'rxjs';
import { concatMap } from 'rxjs';
import { BehaviorSubject, map } from 'rxjs';
import { Observable } from 'rxjs';
import { PhonGraphWord, WordForm } from '../../models/word-form';
import { WordFormsService } from '../../services/word-forms-service';

@Component({
  selector: 'app-phon-graph-word-selector',
  templateUrl: './phon-graph-word-selector.component.html',
  styleUrls: ['./phon-graph-word-selector.component.less']
})
export class PhonGraphWordSelectorComponent implements OnInit, AfterViewInit {

  @Output() selected = new EventEmitter();

  myControl = new FormControl();
  filteredOptions: Observable<PhonGraphWord[]>;

  @ViewChild('trigger', { read: MatAutocompleteTrigger }) trigger: MatAutocompleteTrigger;

  inputText: string;
  inputSubject = new BehaviorSubject<string>('');
  input$ = this.inputSubject.asObservable();

  loading = false;

  constructor(
    private graphService: WordFormsService) {
  }

  ngOnInit() {
  }

  ngAfterViewInit() {
    this.filteredOptions = this.input$.pipe(
      debounceTime(250),
      tap(_ => this.loading = true),
      distinctUntilChanged(),
      concatMap(text => this.updateResults(text)),
      tap(_ => this.loading = false),
    );
  }

  updateResults(text: string) {
    if (typeof text === 'string')
      return this.graphService.searchPhonGraphWord(text, 7);
    else {
      return of([]);
    }
  }

  onChange(text: string) {
    this.inputSubject.next(text);
  }

  onSelect($event: MatAutocompleteSelectedEvent) {
    this.selected.emit($event.option.value);
  }

  displayWith(wf: WordForm) {
    return wf ? wf.graphemes : '';
  }

}
