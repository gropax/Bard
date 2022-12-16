import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApplicationStateService {

  private pageLoadingSubject = new BehaviorSubject<boolean>(false);
  public pageLoading$ = this.pageLoadingSubject.asObservable();

  constructor() { }
  
  public setPageLoading(loading: boolean) {
    this.pageLoadingSubject.next(loading);
  }

}
