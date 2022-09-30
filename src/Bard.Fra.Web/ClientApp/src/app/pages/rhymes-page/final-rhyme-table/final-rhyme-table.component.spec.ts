import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FinalRhymeTableComponent } from './final-rhyme-table.component';

describe('FinalRhymeTableComponent', () => {
  let component: FinalRhymeTableComponent;
  let fixture: ComponentFixture<FinalRhymeTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FinalRhymeTableComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FinalRhymeTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
