import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WhoisInformationComponent } from './whois-information.component';

describe('WhoisInformationComponent', () => {
  let component: WhoisInformationComponent;
  let fixture: ComponentFixture<WhoisInformationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WhoisInformationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WhoisInformationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
