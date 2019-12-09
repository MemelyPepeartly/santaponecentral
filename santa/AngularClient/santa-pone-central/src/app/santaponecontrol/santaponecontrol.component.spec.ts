import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SantaponecontrolComponent } from './santaponecontrol.component';

describe('SantaponecontrolComponent', () => {
  let component: SantaponecontrolComponent;
  let fixture: ComponentFixture<SantaponecontrolComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SantaponecontrolComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SantaponecontrolComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
