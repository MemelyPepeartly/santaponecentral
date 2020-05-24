import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EventControlComponent } from './event-control.component';

describe('EventControlComponent', () => {
  let component: EventControlComponent;
  let fixture: ComponentFixture<EventControlComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EventControlComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EventControlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
