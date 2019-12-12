import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InTransitComponent } from './in-transit.component';

describe('InTransitComponent', () => {
  let component: InTransitComponent;
  let fixture: ComponentFixture<InTransitComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InTransitComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InTransitComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
