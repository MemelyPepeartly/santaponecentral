import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FaqInformationComponent } from './faq-information.component';

describe('FaqInformationComponent', () => {
  let component: FaqInformationComponent;
  let fixture: ComponentFixture<FaqInformationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FaqInformationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FaqInformationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
