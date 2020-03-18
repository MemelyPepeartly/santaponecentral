import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ApinavComponent } from './apinav.component';

describe('ApinavComponent', () => {
  let component: ApinavComponent;
  let fixture: ComponentFixture<ApinavComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ApinavComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ApinavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
