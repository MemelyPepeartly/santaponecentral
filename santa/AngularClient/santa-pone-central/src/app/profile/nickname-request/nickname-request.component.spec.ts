import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NicknameRequestComponent } from './nickname-request.component';

describe('NicknameRequestComponent', () => {
  let component: NicknameRequestComponent;
  let fixture: ComponentFixture<NicknameRequestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NicknameRequestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NicknameRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
