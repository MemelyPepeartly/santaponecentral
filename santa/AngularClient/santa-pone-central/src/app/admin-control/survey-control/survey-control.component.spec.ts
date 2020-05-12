import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SurveyControlComponent } from './survey-control.component';

describe('SurveyControlComponent', () => {
  let component: SurveyControlComponent;
  let fixture: ComponentFixture<SurveyControlComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SurveyControlComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SurveyControlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
