import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DefaultnavComponent } from './defaultnav.component';

describe('DefaultnavComponent', () => {
  let component: DefaultnavComponent;
  let fixture: ComponentFixture<DefaultnavComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DefaultnavComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DefaultnavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
