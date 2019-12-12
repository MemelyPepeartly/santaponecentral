import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShippingCorrespondenceComponent } from './shipping-correspondence.component';

describe('ShippingCorrespondenceComponent', () => {
  let component: ShippingCorrespondenceComponent;
  let fixture: ComponentFixture<ShippingCorrespondenceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShippingCorrespondenceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShippingCorrespondenceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
