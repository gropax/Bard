import { HttpClient } from "@angular/common/http";
import { RhymingWords, PhonGraphWord, WordForm } from "../models/word-form";
import { Injectable } from '@angular/core';
import { Paginated } from "../models/paginated";

@Injectable({
  providedIn: 'root',
})
export class WordFormsService {
  
  constructor(protected http: HttpClient) { }

  public searchWordForm(q: string, limit: number) {
    return this.http.get<WordForm[]>('graph/word-forms/search', { params: { q, limit } });
  }

  public searchPhonGraphWord(q: string, limit: number) {
    return this.http.get<PhonGraphWord[]>('graph/phon-graph-words/search', { params: { q, limit } });
  }

  public getRhymingWords(graphemes: string, phonemes: string, filter: string, sortDir: string, page: number, pageSize: number) {
    return this.http.get<Paginated<RhymingWords>>('graph/final-rhymes',
      { params: { graphemes, phonemes, filter, sortDir, page, pageSize } });
  }
}
