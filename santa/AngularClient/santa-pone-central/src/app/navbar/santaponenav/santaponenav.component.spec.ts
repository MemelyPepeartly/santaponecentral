import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SantaponenavComponent } from './santaponenav.component';

describe('SantaponenavComponent', () => {
  let component: SantaponenavComponent;
  let fixture: ComponentFixture<SantaponenavComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SantaponenavComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SantaponenavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
