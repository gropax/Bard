import { Component, OnInit } from '@angular/core';
import { BehaviorSubject, debounceTime, map } from 'rxjs';
import { TokenizationService } from '../../services/tokenization.service';

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

  public $tokens = this.$text.pipe(map(text => this.tokenizer.tokenize(text)));

  constructor(
    private tokenizer: TokenizationService) {
  }

  ngOnInit(): void {
    //this.$text.subscribe(text => console.log(text));
    this.$tokens.subscribe(tokens => console.log(tokens));
  }

  public updateText() {
    this.textSubject.next(this.text);
  }

}
