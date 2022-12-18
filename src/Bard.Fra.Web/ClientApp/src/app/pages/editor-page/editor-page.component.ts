import { Component, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { BehaviorSubject, debounceTime, map } from 'rxjs';
import { Strophe, Word } from '../../models/text';
import { ParsingService } from '../../services/parsing.service';
import { TokenizationService } from '../../services/tokenization.service';

@Component({
  selector: 'app-editor-page',
  templateUrl: './editor-page.component.html',
  styleUrls: ['./editor-page.component.less']
})
export class EditorPageComponent implements OnInit {

  public text: string;
  public textSubject = new BehaviorSubject<string>("");

  public $text = this.textSubject.asObservable().pipe(debounceTime(300));
  public $tokens = this.$text.pipe(map(text => this.tokenizer.tokenize(text)));
  public $strophes = this.$tokens.pipe(map(tokens => this.parser.parse(tokens)));

  public strophes: Strophe[];

  public viewMode = {
    token: 'Tok',
    ipa: 'IPA',
  }
  public selectedViewMode: string = this.viewMode.token;

  constructor(
    private tokenizer: TokenizationService,
    private parser: ParsingService) {
  }

  ngOnInit(): void {
    //this.$text.subscribe(text => console.log(text));
    //this.$tokens.subscribe(tokens => console.log(tokens));
    this.$strophes.subscribe(strophes => this.strophes = strophes);
  }

  public updateText() {
    this.textSubject.next(this.text);
  }

  public selectViewMode(viewMode: string) {
    this.selectedViewMode = viewMode;
  }

}
