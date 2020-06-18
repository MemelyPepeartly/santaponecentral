/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { CorrespondenceComponent } from './correspondence.component';

describe('CorrespondenceComponent', () => {
  let component: CorrespondenceComponent;
  let fixture: ComponentFixture<CorrespondenceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CorrespondenceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CorrespondenceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
