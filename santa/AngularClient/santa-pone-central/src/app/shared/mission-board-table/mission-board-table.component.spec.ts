import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MissionBoardTableComponent } from './mission-board-table.component';

describe('MissionBoardTableComponent', () => {
  let component: MissionBoardTableComponent;
  let fixture: ComponentFixture<MissionBoardTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MissionBoardTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MissionBoardTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
