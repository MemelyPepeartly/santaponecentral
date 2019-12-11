import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WhatisInformationComponent } from './whatis-information.component';

describe('WhatisInformationComponent', () => {
  let component: WhatisInformationComponent;
  let fixture: ComponentFixture<WhatisInformationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WhatisInformationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WhatisInformationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
