import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UsernavComponent } from './usernav.component';

describe('UsernavComponent', () => {
  let component: UsernavComponent;
  let fixture: ComponentFixture<UsernavComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UsernavComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UsernavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
