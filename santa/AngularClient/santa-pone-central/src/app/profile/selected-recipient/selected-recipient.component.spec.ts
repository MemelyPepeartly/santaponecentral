import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectedRecipientComponent } from './selected-recipient.component';

describe('SelectedRecipientComponent', () => {
  let component: SelectedRecipientComponent;
  let fixture: ComponentFixture<SelectedRecipientComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SelectedRecipientComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelectedRecipientComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
