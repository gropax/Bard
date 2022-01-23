import { HttpClient } from "@angular/common/http";
import { PhonGraphWord, WordForm } from "../models/word-form";
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class GraphService {
  
  constructor(protected http: HttpClient) { }

  public searchWordForm(q: string, limit: number) {
    return this.http.get<WordForm[]>('graph/word-forms/search', { params: { q, limit } });
  }

  public searchPhonGraphWord(q: string, limit: number) {
    return this.http.get<PhonGraphWord[]>('graph/phon-graph-words/search', { params: { q, limit } });
  }
}
