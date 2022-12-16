import { Component, OnInit } from '@angular/core';
import { BehaviorSubject, debounceTime } from 'rxjs';

@Component({
  selector: 'app-editor-page',
  templateUrl: './editor-page.component.html',
  styleUrls: ['./editor-page.component.less']
})
export class EditorPageComponent implements OnInit {

  public text: string;
  public textSubject = new BehaviorSubject<string>("");
  public $text = this.textSubject.asObservable()
    .pipe(debounceTime(500));

  constructor() { }

  ngOnInit(): void {
    this.$text.subscribe(text => console.log(text));
  }

  public updateText() {
    this.textSubject.next(this.text);
  }

}
