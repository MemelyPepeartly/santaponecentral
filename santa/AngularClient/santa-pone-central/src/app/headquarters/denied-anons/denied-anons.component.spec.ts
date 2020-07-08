import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DeniedAnonsComponent } from './denied-anons.component';

describe('DeniedAnonsComponent', () => {
  let component: DeniedAnonsComponent;
  let fixture: ComponentFixture<DeniedAnonsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DeniedAnonsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DeniedAnonsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
