import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NologinInformationComponent } from './nologin-information.component';

describe('NologinInformationComponent', () => {
  let component: NologinInformationComponent;
  let fixture: ComponentFixture<NologinInformationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NologinInformationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NologinInformationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
