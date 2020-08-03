import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MessageControlPanelComponent } from './message-control-panel.component';

describe('MessageControlPanelComponent', () => {
  let component: MessageControlPanelComponent;
  let fixture: ComponentFixture<MessageControlPanelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MessageControlPanelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MessageControlPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
