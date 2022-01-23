import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PhonGraphWordSelectorComponent } from './phon-graph-word-selector.component';

describe('PhonGraphWordSelectorComponent', () => {
  let component: PhonGraphWordSelectorComponent;
  let fixture: ComponentFixture<PhonGraphWordSelectorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PhonGraphWordSelectorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PhonGraphWordSelectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
