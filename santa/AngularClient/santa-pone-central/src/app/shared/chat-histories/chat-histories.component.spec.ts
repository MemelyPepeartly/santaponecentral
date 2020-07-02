import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChatHistoriesComponent } from './chat-histories.component';

describe('ChatHistoriesComponent', () => {
  let component: ChatHistoriesComponent;
  let fixture: ComponentFixture<ChatHistoriesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChatHistoriesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChatHistoriesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
