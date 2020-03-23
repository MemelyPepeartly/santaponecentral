/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { SmallLoadingSpinComponent } from './small-loading-spin.component';

describe('SmallLoadingSpinComponent', () => {
  let component: SmallLoadingSpinComponent;
  let fixture: ComponentFixture<SmallLoadingSpinComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SmallLoadingSpinComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SmallLoadingSpinComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
