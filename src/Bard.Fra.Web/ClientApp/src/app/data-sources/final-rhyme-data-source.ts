import { CollectionViewer, DataSource } from "@angular/cdk/collections";
import { catchError, finalize, Observable, of } from "rxjs";
import { map } from "rxjs";
import { pluck } from "rxjs";
import { BehaviorSubject } from "rxjs";
import { RhymingWords } from "../models/word-form";
import { GraphService } from "../services/word-forms-service";


/*
 * Tutorial on DataSource: https://blog.angular-university.io/angular-material-data-table/
 */
export class RhymingWordDataSource implements DataSource<RhymingWords> {

  private finalRhymeSubject = new BehaviorSubject<RhymingWords[]>([]);
  private loadingSubject = new BehaviorSubject<boolean>(false);
  public loading$ = this.loadingSubject.asObservable();

  constructor(private graphService: GraphService) {}

  connect(collectionViewer: CollectionViewer) {
    return this.finalRhymeSubject.asObservable();
  }

  disconnect(collectionViewer: CollectionViewer) {
    this.finalRhymeSubject.complete();
    this.loadingSubject.complete();
  }

  loadLessons(
    graphemes: string,
    phonemes: string,
    filter: string = '',
    sortDirection: string = 'asc',
    pageIndex: number = 0,
    pageSize: number = 10)
  {
    this.loadingSubject.next(true);

    this.graphService
      .getRhymingWords(graphemes, phonemes, filter, sortDirection, pageIndex, pageSize)
      .pipe(
        map(p => p.items),
        catchError(() => of([])),
        finalize(() => this.loadingSubject.next(false)))
      .subscribe(rhymes => this.finalRhymeSubject.next(rhymes));
  }  
}
