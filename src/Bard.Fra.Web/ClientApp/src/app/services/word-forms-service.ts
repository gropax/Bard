import { HttpClient } from "@angular/common/http";
import { WordForm } from "../models/word-form";
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class WordFormsService {
  
  constructor(protected http: HttpClient) { }

  public search(q: string, limit: number) {
    return this.http.get<WordForm[]>('word-forms/search', { params: { q, limit } });
  }
}
