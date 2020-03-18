import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminHelpComponent } from './admin-help.component';

describe('AdminHelpComponent', () => {
  let component: AdminHelpComponent;
  let fixture: ComponentFixture<AdminHelpComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminHelpComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminHelpComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
