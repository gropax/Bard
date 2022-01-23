import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WordFormSelectorComponent } from './word-form-selector.component';

describe('WordFormSelectorComponent', () => {
  let component: WordFormSelectorComponent;
  let fixture: ComponentFixture<WordFormSelectorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WordFormSelectorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WordFormSelectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
