import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IncomingCorrespondenceComponent } from './incoming-correspondence.component';

describe('IncomingCorrespondenceComponent', () => {
  let component: IncomingCorrespondenceComponent;
  let fixture: ComponentFixture<IncomingCorrespondenceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IncomingCorrespondenceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IncomingCorrespondenceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
