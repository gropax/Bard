import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RhymesPageComponent } from './rhymes-page.component';

describe('RhymesPageComponent', () => {
  let component: RhymesPageComponent;
  let fixture: ComponentFixture<RhymesPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RhymesPageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RhymesPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
