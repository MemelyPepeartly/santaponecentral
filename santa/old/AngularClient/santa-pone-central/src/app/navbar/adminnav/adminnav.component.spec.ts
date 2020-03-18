import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminnavComponent } from './adminnav.component';

describe('AdminnavComponent', () => {
  let component: AdminnavComponent;
  let fixture: ComponentFixture<AdminnavComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminnavComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminnavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
