import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GeneralCorrespondenceComponent } from './general-correspondence.component';

describe('GeneralCorrespondenceComponent', () => {
  let component: GeneralCorrespondenceComponent;
  let fixture: ComponentFixture<GeneralCorrespondenceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GeneralCorrespondenceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GeneralCorrespondenceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
