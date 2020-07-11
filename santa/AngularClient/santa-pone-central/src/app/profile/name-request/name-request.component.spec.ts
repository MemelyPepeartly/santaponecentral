import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NameRequestComponent } from './name-request.component';

describe('NameRequestComponent', () => {
  let component: NameRequestComponent;
  let fixture: ComponentFixture<NameRequestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NameRequestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NameRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
