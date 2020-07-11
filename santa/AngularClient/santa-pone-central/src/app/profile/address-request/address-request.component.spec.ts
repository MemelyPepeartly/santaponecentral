import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddressRequestComponent } from './address-request.component';

describe('AddressRequestComponent', () => {
  let component: AddressRequestComponent;
  let fixture: ComponentFixture<AddressRequestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddressRequestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddressRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
