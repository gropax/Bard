import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WordFormsPageComponent } from './word-forms-page.component';

describe('WordFormsPageComponent', () => {
  let component: WordFormsPageComponent;
  let fixture: ComponentFixture<WordFormsPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WordFormsPageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WordFormsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
