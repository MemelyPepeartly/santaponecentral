import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CompletedAnonsComponent } from './completed-anons.component';

describe('CompletedAnonsComponent', () => {
  let component: CompletedAnonsComponent;
  let fixture: ComponentFixture<CompletedAnonsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CompletedAnonsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CompletedAnonsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
