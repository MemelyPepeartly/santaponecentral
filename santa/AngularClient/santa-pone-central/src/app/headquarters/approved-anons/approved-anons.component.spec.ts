import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ApprovedAnonsComponent } from './approved-anons.component';

describe('ApprovedAnonsComponent', () => {
  let component: ApprovedAnonsComponent;
  let fixture: ComponentFixture<ApprovedAnonsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ApprovedAnonsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ApprovedAnonsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
